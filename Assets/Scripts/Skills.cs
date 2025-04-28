using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    private bool isSkillUsing;
    private Vector2 skillRangeStart;
    private Vector2 size_Swing;
    private Vector2 dir;

    private void Awake()
    {
        
    }

    private void Update()
    {
        dir = new Vector2(GameManager.Instance.player.lastLookAt.x, GameManager.Instance.player.lastLookAt.y);
        skillRangeStart = new Vector2(transform.position.x, transform.position.y) + dir;
    }

    // OnDrawGizmos()는 Scene 창에서 눈으로 확인하기 위함
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(skillRangeStart, size_Swing);
    }

    public IEnumerator Swing(int atk)
    {
        isSkillUsing = true;

        switch (dir)
        {
            case Vector2 r when r.Equals(Vector2.right):
                size_Swing = new Vector2(3, 2);
                skillRangeStart.y -= size_Swing.y * 0.5f;
                break;
            case Vector2 l when l.Equals(Vector2.left):
                size_Swing = new Vector2(-3, 2);
                skillRangeStart.y -= size_Swing.y * 0.5f;
                break;
            case Vector2 u when u.Equals(Vector2.up):
                size_Swing = new Vector2(2, 3);
                skillRangeStart.x -= size_Swing.x * 0.5f;
                break;
            case Vector2 d when d.Equals(Vector2.down):
                size_Swing = new Vector2(2, -3);
                skillRangeStart.x -= size_Swing.x * 0.5f;
                break;
        }

        Debug.Log("size_Swing: " + size_Swing);
        Debug.Log("Swing Attack!");
        Collider2D[] enimies = Physics2D.OverlapBoxAll(skillRangeStart, size_Swing, 0f);

        if (enimies.Length != 0)
        {
            for (int i = 0; i < enimies.Length; i++)
            {
                if (enimies[i].CompareTag("Monster"))
                    enimies[i].GetComponent<Monster>().Damaged(atk);
            }
        }

        yield return new WaitForSeconds(1);
        isSkillUsing = false;
    }
}
