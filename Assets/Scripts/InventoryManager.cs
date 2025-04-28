using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> boundaryList;
    private Dictionary<int, int> itemDict; // 아이템을 저장할 자료구조

    private SpriteRenderer otherSpriteRenderer;

    public static InventoryManager Instance; // 싱글톤 인스턴스

    private void Awake()
    {
        // 싱글톤 인스턴스 생성
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // 아이템 획득 영역 저장소 생성
        boundaryList = new List<GameObject>();
        // 아이템 자료구조 생성
        itemDict = new Dictionary<int, int>();
    }

    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            boundaryList.Add(other.gameObject);
            Debug.Log(other.name + "을 획득할까?");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Npc"))
        {
            float gapY;
            otherSpriteRenderer = other.GetComponent<SpriteRenderer>();
            gapY = other.transform.position.y - transform.position.y;
            
            if (gapY > 1.0f)
                otherSpriteRenderer.sortingOrder = -1;
            else
                otherSpriteRenderer.sortingOrder = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
     {
        if (other.gameObject.tag == "Item")
            boundaryList.Remove(other.gameObject);
     }

     public void AddItem(int itemID, int itemNum)
    {
        if (itemDict.ContainsKey(itemID))
            itemDict[itemID] += itemNum;
        else
            itemDict.Add(itemID, itemNum);

        Debug.Log(GetName(itemID) + " " + itemNum + "개를 획득했다.");
    }

    private string GetName(int itemId)
    {
        switch (itemId)
        {
            case 0:
                return "통나무";
            default:
                return "알 수 없는 아이템";
        }
    }
}
