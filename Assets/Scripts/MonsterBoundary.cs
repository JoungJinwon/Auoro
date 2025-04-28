using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterBoundary : MonoBehaviour
{
    private Monster monster;

    private void Awake()
    {
        monster = transform.parent.GetComponent<Monster>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        StartCoroutine(monster.DetectPlayer(other));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        monster.DetectExitPlayer(other);
    }
}
