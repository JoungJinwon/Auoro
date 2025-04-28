using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcsInfo : MonoBehaviour
{
    /*
    게임 내 모든 NPC는 ID를 가진다.
    NPC ID : 1000 ~ 1500
    */
    [SerializeField]
    private int npcId;
    [SerializeField]
    private string npcName;
    private int dialContext;
    public int talkIndex;

    public enum NpcDir { Left, Right, Front, Back }
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite[] fourDir;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        talkIndex = 0;
    }

    public int GetNpcId()
    {
        return npcId;
    }

    public string GetNpcName()
    {
        return npcName;
    }

    public int GetDialContext()
    {
        return dialContext;
    }

    public void SetNpcDir(NpcDir npcDir)
    {
        switch (npcDir)
        {
            case NpcDir.Left:
                spriteRenderer.sprite = fourDir[0];
                break;
            case NpcDir.Right:
                spriteRenderer.sprite = fourDir[1];
                break;
            case NpcDir.Front:
                spriteRenderer.sprite = fourDir[2];
                break;
            case NpcDir.Back:
                spriteRenderer.sprite = fourDir[3];
                break;
        }
        
        return;
    }

    // public int GetTalkIndex()
    // {
    //     return talkIndex;
    // }
}
