using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterGenerator : MonoBehaviour
{
    private Dictionary<int, string> _monsters_dict;
    List<Monster> monsterPool;

    private void Awake()
    {
        _monsters_dict = new Dictionary<int, string>();
    }

    private void GenerateMonsters()
    {

    }
}
