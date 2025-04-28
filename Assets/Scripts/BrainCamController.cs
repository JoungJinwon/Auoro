using UnityEngine;

public class BrainCamController : MonoBehaviour
{
    public static BrainCamController Instance {get; private set;} // Singletone 인스턴스

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
