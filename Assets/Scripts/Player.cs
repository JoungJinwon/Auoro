using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public bool IsInPortal {get; private set;} // 플레이어가 포탈에 위치해 있는가?
    public bool IsMoving {get; private set;} // 플레이어가 현재 움직이고 있는지의 여부
    public int Level {get; private set;} // 플레이어 레벨
    public int AttackPower {get; private set;} // 플레이어 공격력
    public int DefensePower {get; private set;} // 플레이어 방어력
    public int AbsorptionRate {get; private set;} // 플레이어 에터 흡수량
    public int ConsumptionRate {get; private set;} // 플레이어 에터 소모량
    public int Money {get; private set;} // 플레이어 머니
    public int Gold {get; private set;} // 플레이어 골드
    public int LinkedPortalSceneNum {get; private set;} // 포탈에 연결된 씬의 인덱스
    public int LinkedPortalID {get; private set;} // 연결된 씬 내의 포탈 ID
    public float CurEther {get; private set;} // 플레이어 현재 에터
    public float MaxEther {get; private set;} // 플레이어 최대 에터
    public float Exp {get; private set;} // 플레이어 경험치
    public float MaxExp {get; private set;} // 플레이어 최대 경험치
    public float CriticalRate {get; private set;} // 플레이어 크리티컬 확률
    public string Nickname {get; private set;} // 플레이어 닉네임
    public string Title {get; private set;} // 플레이어 칭호

    public Vector2 lastLookAt; // 플레이어 입력에 의해 마지막으로 바라보고 있던 방향

    private bool isDashing; // 플레이어가 현재 대시 중인지의 여부
    [SerializeField]
    private float playerSpeed;
    private float lateYCheck;

    private Vector2 nextPos; // 플레이어가 다음에 움직일 거리

    private Rigidbody2D rigid;
    private Animator anim;
    private Skills skills;
    
    private SpriteRenderer spriteRenderer;
    
    public float LateYCheck
    {
        get {
            return lateYCheck;
        }
        set {
            if (value != 0.0f)
                lateYCheck = value;
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        skills = GetComponent<Skills>();

        Level = 1;
        Exp = 0;
        MaxExp = 100;
        MaxEther = 100;
        CurEther = 100;
        AttackPower = 100;
        DefensePower = 100;
        AbsorptionRate = 1;
        ConsumptionRate = 10;
        Money = 0;
        Gold = 0;
        CriticalRate = 5.0f;
        Nickname = "Auoro";
        Title = "초보자";

        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        nextPos = new Vector2(InputManager.Instance.X, InputManager.Instance.Y);
        if (!GameManager.Instance.IsTalking)
            rigid.MovePosition(rigid.position + nextPos * Time.fixedDeltaTime * playerSpeed);
    }

    private void Update()
    {
        // 캐릭터가 움직이고 있는지 확인, Ray를 쏘기 위한 lastLookAt 값 저장
        if (nextPos != Vector2.zero)
        {
            lastLookAt = nextPos;
            LateYCheck = lastLookAt.y;
            IsMoving = true;
        }
        else 
            IsMoving = false;

        // 캐릭터가 보는 좌우 방향 조정
        if(InputManager.Instance.X != 0)     
            spriteRenderer.flipX = (InputManager.Instance.X < 0.0f) ? true : false;

        // Player Animator 파라미터 설정
        anim.SetBool("IsMoving", IsMoving);

        if (InputManager.Instance.X == 0)
            anim.SetFloat("InputX", lastLookAt.x);
        else
            anim.SetFloat("InputX", InputManager.Instance.X);

        if (InputManager.Instance.Y == 0)
            anim.SetFloat("InputY", lastLookAt.y);
        else
            anim.SetFloat("InputY", InputManager.Instance.Y);
        
        anim.SetFloat("LateYCheck", LateYCheck);
        

        // 플레이어 대시
        if (InputManager.Instance.Dash && IsMoving && !InputManager.Instance.IsBlockingInput)
            Dash();
        else if (!isDashing && !InputManager.Instance.IsBlockingInput)
            playerSpeed = 5.0f;
    }

    // 플레이어 대시 로직
    private void Dash()
    {
        playerSpeed = 10.0f;
        anim.SetTrigger("Dash");
        rigid.MovePosition(rigid.position + lastLookAt * playerSpeed);
        StartCoroutine(InputManager.Instance.BlockInput(0.4f));
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
        StartCoroutine(InputManager.Instance.BlockInput(0.55f));
        StartCoroutine(skills.Swing(AttackPower));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 포털 이동 로직
        if (other.gameObject.CompareTag("Portal"))
        {
            IsInPortal = true;
            Portal portal = other.GetComponent<Portal>();
            LinkedPortalSceneNum = portal.GetLinkedSceneNum();
            LinkedPortalID = portal.GetLinkedPortalId();
            Debug.Log("포탈을 통해 이동하겠습니까?");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 포털 이동 로직
        if (other.gameObject.CompareTag("Portal"))
        {
            IsInPortal = false;
        }
    }

    public float GetEtherRate()
    {
        float etherRate = CurEther / MaxEther;
        return etherRate;
    }

    public void GetExp(int exp)
    {
        Exp += exp;

        while (Exp >= MaxExp)
        {
            Exp -= MaxExp;
            Level++;
        }

        UIManager.Instance.UpdateExpUI();
    }
    
    public float GetExpRate()
    {
        float expRate = Exp / MaxExp;
        return expRate;
    }

    public void GetDamaged(int damage)
    {
        int realDamage = damage;
        
        if (CurEther > damage)
        {
            CurEther -= damage;
            Debug.Log(damage + "의 피해를 입었다");
        }
        else
        {
            CurEther = 0.0f;
            Debug.Log(Nickname + "는 에터를 모두 소진해 쓰러졌다!");
        }

        UIManager.Instance.UpdateEtherUI();
    }

}