using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour
{
    private bool isDead;
    private bool isAttacking;
    private float knockbackForce;
    private float hpBarTimer;
    private Vector2 monsterNextMove;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private RectTransform rect_hpBar;
    private GameObject hpBarObject;

    [SerializeField]
    private string monsterName;
    private float monsterHp;
    [SerializeField]
    private float monsterMaxHp;
    [SerializeField]
    private int monsterAttack;
    [SerializeField]
    private int attackRange;
    [SerializeField]
    private int attackInterval;
    [SerializeField]
    private int monsterDeffense;
    [SerializeField]
    private int monsterMoveSpeed;
    [SerializeField]
    private int respawnTime;
    [SerializeField]
    private int monsterExp;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();

        monsterHp = monsterMaxHp;
        isDead = false;
        knockbackForce = 10f;
        StartCoroutine(MonsterMove());
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;
        
        Move();
    }

    public void Attack()
    {
        _animator.SetBool("IsAttacking", true);
        GameManager.Instance.player.GetDamaged(monsterAttack);
    }

    public void Damaged(int damage)
    {
        StartCoroutine(KnockbackCor());
        StartCoroutine(EnableHpBar());

        if (monsterHp > damage)
        {
            monsterHp -= damage;
        }
        else
        {
            monsterHp = 0;
            StartCoroutine(Dead());
        }
    }

    private void Move()
    {
        _rigidbody.velocity = monsterNextMove * monsterMoveSpeed * 0.5f;
    }

    private IEnumerator KnockbackCor()
    {
        float timer = 0f;
        float knockbackTime = 1f;

        while (timer <= knockbackTime)
        {
            timer += Time.deltaTime;
            _rigidbody.AddForce((transform.position - GameManager.Instance.player.transform.position).normalized * knockbackForce, ForceMode2D.Impulse);
        }
        yield break;
    }

    private IEnumerator Dead()
    {
        float timer = 0f;
        float deadTime = 1f;

        Debug.Log(monsterName + "(을)를 처치했습니다");
        isDead = true;

        GameManager.Instance.player.GetExp(monsterExp);
        
        while (timer <= deadTime)
        {
            timer += Time.deltaTime;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b
                , Mathf.Lerp(1f, 0f, timer / deadTime));
        }

        gameObject.SetActive(false);

        yield break;
    }

    private void respawn()
    {

    }

    public void DetectExitPlayer(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(MonsterMove());
            isAttacking = false;
            _animator.SetBool("IsAttacking", false);
        }
    }
    
    public IEnumerator DetectPlayer(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(MonsterMove());

            monsterNextMove = other.transform.position - transform.position;
            _spriteRenderer.flipX = (monsterNextMove.x < 0) ? false : true;

            if (monsterNextMove.magnitude < 1)
            {
                monsterNextMove = Vector2.zero;
                if (!isAttacking)
                {
                    isAttacking = true;
                    yield return StartCoroutine(AttackCoroutine());
                }

                yield break;
            }

            isAttacking = false;
            _animator.SetBool("IsAttacking", false);
        }
    }



    private IEnumerator AttackCoroutine()
    {
        if (isDead)
        {
            isAttacking = false;
            yield break;
        }

        Attack();
        yield return new WaitForSeconds(attackInterval);
        StartCoroutine(AttackCoroutine());
    }


    public IEnumerator MonsterMove()
    {
        monsterNextMove = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        yield return new WaitForSeconds(2f);

        monsterNextMove = Vector2.zero;
        yield return new WaitForSeconds(4f);
        
        StartCoroutine(MonsterMove());
    }

    private IEnumerator EnableHpBar()
    {
        if (hpBarObject != null)
        {
            hpBarTimer = 0f;
            yield break;
        }

        hpBarTimer = 0f;
        float hpBarMaxTime = 3f;
        
        hpBarObject = UIManager.Instance.GenerateEnemyHPbar();
        rect_hpBar = hpBarObject.GetComponent<RectTransform>();
        Slider hpSlider = hpBarObject.GetComponentInChildren<Slider>();
        
        hpSlider.value = monsterHp / monsterMaxHp;

        while (hpBarTimer <= hpBarMaxTime)
        {
            hpBarTimer += Time.deltaTime;

            rect_hpBar.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, -1f, 0f));
            hpSlider.value = monsterHp / monsterMaxHp;

            yield return null;
        }

        Destroy(hpBarObject);
        yield break;
    }
}
