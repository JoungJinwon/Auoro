using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class VirtualCamController : MonoBehaviour
{
    private CinemachineVirtualCameraBase cameraBase;

    public static VirtualCamController Instance { get; private set;} // Singletone 인스턴스

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    
        cameraBase = GetComponent<CinemachineVirtualCameraBase>();

        SceneManager.sceneLoaded += SetTransform;
    }

    private void SetTransform(Scene scene, LoadSceneMode mode)
    {
        cameraBase.Follow = GameManager.Instance.player.transform;
    }
}
