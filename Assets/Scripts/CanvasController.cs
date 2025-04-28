using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    private bool isPlayCanvas;
    public static CanvasController PcInstance {get; private set;} // Singletone 인스턴스
    public static CanvasController LcInstance {get; private set;} // Singletone 인스턴스

    private void Awake()
    {
        switch (isPlayCanvas)
        {
            case true:
                if (PcInstance == null)
                {
                    PcInstance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                    Destroy(gameObject);
                break;
            case false:
                if (LcInstance == null)
                {
                    LcInstance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                    Destroy(gameObject);
                break;
        }
    }
}
