using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private int portalId; // 현재 씬 내의 포탈 ID. 포탈 ID는 하나의 씬 내에서 고유하며, 0부터 시작한다.
    [SerializeField]
    private int linkedSceneIdx; // 연결된 씬의 인덱스
    [SerializeField]
    private int linkedPortalId; // 연결된 씬 내의 포탈 ID

    public bool IsBlocked;
    public Vector2 PortalCoordinate {get; private set;}

    private void Awake()
    {
        PortalCoordinate = (Vector2)transform.position;
    }

    public int GetPortalId()
    {
        return portalId;
    }

    public int GetLinkedSceneNum()
    {
        return linkedSceneIdx;
    }

    public int GetLinkedPortalId()
    {
        return linkedPortalId;
    }
}
