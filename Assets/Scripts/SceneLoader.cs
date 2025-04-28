using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private bool fadeDone;
    private int nextSceneIndex;
    private GameObject loadingCircleObject;
    private GameObject canvas;
    private GameObject bg;
    [SerializeField]
    private GameObject loadCanvas;
    private Image fadeImage;
    private Image loadingCircle;
    private TextMeshProUGUI loadingText;

    public static SceneLoader Instance { get; private set;} // Singletone 인스턴스

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        fadeImage = loadCanvas.GetComponentInChildren<Image>();
    }

    public void LoadScene(int sceneIndex)
    {
        nextSceneIndex = sceneIndex;
        loadCanvas.SetActive(true);
        StartCoroutine(LoadSceneWithFade());
    }
    
    private IEnumerator LoadSceneWithFade()
    {
        // Fade Out을 먼저 실행
        yield return StartCoroutine(FadeOut());

        // 로딩 씬으로 전환
        StartCoroutine(BeforeLoadScene());
    }

    private IEnumerator FadeOut()
    {
        fadeDone = false;

        float elapsedTime = 0.0f;
        float fadeTime = 0.5f;

        while (elapsedTime <= fadeTime)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Vector4(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.Lerp(0f, 1f, elapsedTime / fadeTime));
            yield return null;
        }

        fadeDone = true;
        yield break;
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitUntil(() => fadeDone);
        loadCanvas.SetActive(true);

        float elapsedTime = 0.0f;
        float fadeTime = 0.5f;

        while (elapsedTime <= fadeTime)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Vector4(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.Lerp(1f, 0f, elapsedTime / fadeTime));
            yield return null;
        }

        yield break;
    }

    /*
     로딩 씬의 필요한 객체들을 찾아 private 변수에 할당한다. 
     씬이 완전히 로드된 후 객체들을 찾기 위해 코루틴을 사용한다
    */
    private IEnumerator BeforeLoadScene()
    {
        AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync(1); // Loading Scene index = 1
        yield return new WaitUntil(() => loadingSceneOp.isDone);

        loadCanvas.SetActive(false);

        canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            bg = canvas.transform.Find("Background").gameObject;
            if (bg != null)
            {
                loadingCircleObject = bg.transform.Find("Loading Circle").gameObject;
                if (loadingCircleObject != null)
                {
                    loadingCircle = loadingCircleObject.GetComponent<Image>();
                    loadingText = loadingCircleObject.transform.Find("Loading Value Text").GetComponent<TextMeshProUGUI>();

                    if (loadingCircle != null && loadingText != null)
                    {
                        loadingCircle.fillAmount = 0.0f;
                        loadingText.text = "0%";
                    }
                }
            }
        }
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneIndex);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            loadingText.text = (op.progress * 100 + 9).ToString() + "%";

            if (op.progress < 0.9f)
            {
                loadingCircle.fillAmount = Mathf.Lerp(0.0f, op.progress, timer);
                if (loadingCircle.fillAmount >= op.progress)
                    timer = 0f;
            }
            else
            {
                loadingCircle.fillAmount = Mathf.Lerp(loadingCircle.fillAmount, 1f, timer);
                if (loadingCircle.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield return StartCoroutine(FadeIn());
                    loadCanvas.SetActive(false);
                    yield break;
                }
            }
        }

    }
}
