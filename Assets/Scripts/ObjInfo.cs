using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInfo : MonoBehaviour
{
    /*
    게임 내 모든 상호작용 가능한 오브젝트(NPC 제외)는 ID를 가진다.
    아이템 ID : 0 ~ 999
    */
    [SerializeField]
    private int objId;
    [SerializeField]
    private string objName;
    [SerializeField]
    private int objNum;

    public int GetObjId()
    {
        return objId;
    }

    public string GetObjName()
    {
        return objName;
    }

    public int GetObjNum()
    {
        return objNum;
    }
}
