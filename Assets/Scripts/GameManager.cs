using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    private float rayLength; // 레이 길이
    private Rigidbody2D playerRigid; // 플레이어 RigidBody

    public bool IsTalking {get; private set;} // 대화 중인지의 여부
    public bool IsPaused {get; private set;} // 일시 정지 확인 변수
    public GameObject InteractObject {get; private set;}
    public NpcsInfo InteractObjectInfo {get; private set;}
    public Player player; // 플레이어 객체
    public static GameManager Instance {get; private set;} // Singletone 인스턴스

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        IsTalking = false;
        rayLength = 1.6f;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (player == null)
            return;

        // Ray 발사!
        Debug.DrawRay(playerRigid.position, player.lastLookAt * rayLength, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(playerRigid.position, player.lastLookAt, rayLength, LayerMask.GetMask("Interactable"));

        if (hit.collider != null) // hit 대상이 존재할 경우
        {
            // hit 대상이 NPC가 맞다면, 해당 NPC의 Dial Context를 가져와서 Dialogue Manager로부터 Dialogues를 받아온다.
            if(hit.collider.gameObject.CompareTag("Npc"))
            {
                InteractObject = hit.collider.gameObject;
                InteractObjectInfo = InteractObject.GetComponent<NpcsInfo>();

                DialogueManager.Instance.dialogues
                 = DialogueManager.Instance.GetDialogues(InteractObjectInfo.GetNpcId(), InteractObjectInfo.GetDialContext());
            }
        }
        else // hit 대상이 존재하지 않을 경우, 변수들을 리셋
        {
            InteractObject = null;
            InteractObjectInfo = null;
            DialogueManager.Instance.dialogues = null;
        }

        // 아이템 먹기
        if (InventoryManager.Instance.boundaryList.Any(i => i.CompareTag("Item")) && InputManager.Instance.Action)
        {
            GameObject item = InventoryManager.Instance.boundaryList[0];
            ObjInfo itemInfo = item.GetComponent<ObjInfo>();
            InventoryManager.Instance.AddItem(itemInfo.GetObjId(), itemInfo.GetObjNum());
            item.SetActive(false);
        }
    }

    public void SceneChange(int sceneIndex)
    {
        SceneLoader.Instance.LoadScene(sceneIndex);
    }

    private void GamePause()
    {
        IsPaused = true;
        Time.timeScale = 0.0f;
    }

    private void GameResume()
    {
        IsPaused = false;
        Time.timeScale = 1.0f;
    }

    public void GameExit()
    {
        Debug.Log("게임을 종료합니다");
        Application.Quit();
    }

    public void HandleAction()
    {
        Debug.Log("Action Clicked!");
        
        // Case 1: 대화를 띄운다
        if (DialogueManager.Instance.dialogues != null && !IsTalking)
        {
            ConsumeAction();
            StartCoroutine(ShowDialogue());
        }
        // Case 2: 포털을 통해 이동한다
        else if (player.IsInPortal)
        {
            ConsumeAction();
            SceneChange(player.LinkedPortalSceneNum);
        }
        // Case 3: 플레이어 공격을 실행한다
        else if (InputManager.Instance.Action && !player.IsMoving && !InputManager.Instance.IsBlockingInput && !IsTalking)
        {
            ConsumeAction();
            player.Attack();
        }
    }

    private IEnumerator ShowDialogue()
    {
        // NPC 방향 설정
        switch (player.lastLookAt)
        {
            case Vector2 r when r.Equals(Vector2.right):
                InteractObjectInfo.SetNpcDir(NpcsInfo.NpcDir.Left);
                break;
            case Vector2 l when l.Equals(Vector2.left):
                InteractObjectInfo.SetNpcDir(NpcsInfo.NpcDir.Right);
                break;
            case Vector2 u when u.Equals(Vector2.up):
                InteractObjectInfo.SetNpcDir(NpcsInfo.NpcDir.Front);
                break;
            case Vector2 d when d.Equals(Vector2.down):
                InteractObjectInfo.SetNpcDir(NpcsInfo.NpcDir.Back);
                break;
        }
        
        IsTalking = true;
        int talkIndex = 0;
        UIManager.Instance.dialoguePanel.gameObject.SetActive(true);

        while (talkIndex < DialogueManager.Instance.dialogues.Length)
        {
            UIManager.Instance.curText.text = DialogueManager.Instance.dialogues[talkIndex];
            yield return new WaitUntil(() => ConsumeAction());
            talkIndex++;
            InputManager.Instance.Action = false;
        }

        IsTalking = false;
        UIManager.Instance.dialoguePanel.gameObject.SetActive(false);
    }

    // Action 소비 메서드
    public bool ConsumeAction()
    {
        if (InputManager.Instance.Action)
        {
            InputManager.Instance.Action = false;
            return true;
        }

        return false;
    }

    // Escape 처리 함수
    public void EscapeControl()
    {
        UIManager.Instance.OnMenubookClicked();
            if (!IsPaused)
                GamePause();
            else
                GameResume();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 플레이 씬 진입 시
        if (scene.buildIndex > 1)
        {
            // 플레이어 객체 할당
            if (player == null)
            {
                player = FindObjectOfType<Player>();
                playerRigid = player.GetComponent<Rigidbody2D>();

                if (UIManager.Instance.dialoguePanel != null)
                    UIManager.Instance.dialoguePanel.gameObject.SetActive(false);
            }
            
            // 플레이어가 다음 포탈에 위치하도록 하는 로직
            Portal[] portal = GameObject.Find("Portals").GetComponentsInChildren<Portal>();
            for (int i = 0; i < portal.Length; i++)
            {
                if (portal[i].GetPortalId() == player.LinkedPortalID)
                {
                    Debug.Log("포털 ID: " + i + ", 위치: " + portal[i].transform.position);
                    player.transform.position = portal[i].transform.position;
                    break;
                }
            }
        }
    }
}
