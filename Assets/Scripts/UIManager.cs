using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    private float opt_audio_HighlighterPreset, opt_ctrl_HighlighterPreset, opt_svld_HighlighterPreset; // Options 버튼 Preset

    // UI Components in Play Scene
    [SerializeField]
    private GameObject playCanvas; // Play Canvas 오브젝트
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private Button pauseBtn; // 메뉴 오픈 버튼
    [SerializeField]
    private Slider etherSlider; // 에터 슬라이더
    [SerializeField]
    private TextMeshProUGUI etherValueText;
    [SerializeField]
    private Slider expSlider; // 경험치 슬라이더
    [SerializeField]
    private TextMeshProUGUI expValueText;
    [SerializeField]
    private GameObject enemyHpBar;

    // Menu Window & Tap Images
    [SerializeField]
    private GameObject menuSet; // 전체 메뉴 화면 세트 (배경 포함)
    [SerializeField]
    private GameObject menu; // 메뉴 창 오브젝트
    [SerializeField]
    private Sprite[] menuImages; // 메뉴 탭 이미지 (7개)
    private Image menuImage; // 현재 보여줄 메뉴 탭 이미지를 넣게 될 이미지 컴포넌트
    [SerializeField]
    private Image[] inSettings; // Options 버튼에 의해 활성화 될 설정 탭 (2개)
    [SerializeField]
    private GameObject settingsObjects;
    [SerializeField]
    private GameObject optHighlighter; // Settings 탭 내 Options 버튼 하이라이터
    private RectTransform optHighlighterRT; // Options 버튼 하이라이터 Y 값 Preset (3개)
    
    // Menu - profile Tap UI Objects
    [SerializeField]
    private TextMeshProUGUI profileLevelText;
    [SerializeField]
    private TextMeshProUGUI profileNicknameText;
    [SerializeField]
    private TextMeshProUGUI profileTitleText;

    // In Conversation
    public Image dialoguePanel; // 대화 창
    public TextMeshProUGUI curText; // 현재 대화 텍스트

    public static UIManager Instance { get; private set;} // Singletone 인스턴스

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        opt_audio_HighlighterPreset = 72.8f;
        opt_ctrl_HighlighterPreset = 40.2f;
        opt_svld_HighlighterPreset = 7.6f;

        // pauseBtn.onClick.AddListener(GameManager.Instance.EscapeControl);
        SceneManager.sceneLoaded += OnSceneLoadedUI;

        playCanvas.SetActive(false);
        menuSet.SetActive(false);

        menuImage = menu.GetComponent<Image>();
        optHighlighterRT = optHighlighter.GetComponent<RectTransform>();
    }

    public void OnProfileTabClicked()
    {
        if (settingsObjects.activeSelf)
            settingsObjects.SetActive(false);
            
        profileLevelText.text = "Lv. " + GameManager.Instance.player.Level.ToString();
        profileNicknameText.text = GameManager.Instance.player.Nickname;
        profileTitleText.text = GameManager.Instance.player.Title;

        if (menuImage != menuImages[0])
            menuImage.sprite = menuImages[0];
    }

    public void OnInventoryTabClicked()
    {
        if (settingsObjects.activeSelf)
            settingsObjects.SetActive(false);

        if (menuImage != menuImages[1])
            menuImage.sprite = menuImages[1];
    }

    public void OnStatusTabClicked()
    {
        if (settingsObjects.activeSelf)
            settingsObjects.SetActive(false);

        if (menuImage != menuImages[2])
            menuImage.sprite = menuImages[2];
    }

    public void OnSkillsTabClicked()
    {
        if (settingsObjects.activeSelf)
            settingsObjects.SetActive(false);

        if (menuImage != menuImages[3])
            menuImage.sprite = menuImages[3];
    }

    public void OnSteleTabClicked()
    {
        if (settingsObjects.activeSelf)
            settingsObjects.SetActive(false);

        if (menuImage != menuImages[4])
            menuImage.sprite = menuImages[4];
    }

    public void OnQuestTabClicked()
    {
        if (settingsObjects.activeSelf)
            settingsObjects.SetActive(false);

        if (menuImage != menuImages[5])
            menuImage.sprite = menuImages[5];
    }

    public void OnSettingsTabClicked()
    {
        if (menuImage != menuImages[6])
        {
            menuImage.sprite = menuImages[6];
            settingsObjects.SetActive(true);
        }
    }

    public void OnAudioButtonClicked()
    {
        if (menuImage != menuImages[6])
        {
            optHighlighterRT.anchoredPosition = new Vector2(optHighlighterRT.anchoredPosition.x, opt_audio_HighlighterPreset);

            if (inSettings[0].gameObject.activeSelf || inSettings[1].gameObject.activeSelf)
            {
                inSettings[0].gameObject.SetActive(false);
                inSettings[1].gameObject.SetActive(false);
            }
        }
    }

    public void OnControlsButtonClicked()
    {
        if (menuImage != menuImages[6])
        {
            optHighlighterRT.anchoredPosition = new Vector2(optHighlighterRT.anchoredPosition.x, opt_ctrl_HighlighterPreset);

            if (inSettings[1].gameObject.activeSelf) // save/load 창이 활성화 돼있다면, 꺼준다
                inSettings[1].gameObject.SetActive(false);
            if (!inSettings[0].gameObject.activeSelf) // controls 창이 이미 활성화된 상태가 아니라면, 켜준다
                inSettings[0].gameObject.SetActive(true);
        }
    }

    public void OnSaveAndLoadButtonClicked()
    {
        if (menuImage != menuImages[6])
        {
            optHighlighterRT.anchoredPosition = new Vector2(optHighlighterRT.anchoredPosition.x, opt_svld_HighlighterPreset);
            
            if (inSettings[0].gameObject.activeSelf) // controls 창이 활성화 돼있다면, 꺼준다
                inSettings[0].gameObject.SetActive(false);
            if (!inSettings[1].gameObject.activeSelf) // save/load 창이 이미 활성화된 상태가 아니라면, 켜준다
                inSettings[1].gameObject.SetActive(true);
        }
    }
    
    // Game Manager의 EscapeControl()에서 사용
    public void OnMenubookClicked()
    {
        if (menuSet.activeSelf == false)
            menuSet.SetActive(true);
        else
            menuSet.SetActive(false);
    }

    public void UpdateLevelUI()
    {
        levelText.text = "Lvl. " + GameManager.Instance.player.Level;
    }

    public void UpdateEtherUI()
    {
        etherSlider.value = GameManager.Instance.player.GetEtherRate();
        etherValueText.text = GameManager.Instance.player.CurEther + " / " + GameManager.Instance.player.MaxEther;
    }

    public void UpdateExpUI()
    {
        expSlider.value = GameManager.Instance.player.GetExpRate();
        Debug.Log("현재 Exp: " + GameManager.Instance.player.Exp);
        expValueText.text = GameManager.Instance.player.Exp + " / " + GameManager.Instance.player.MaxExp;
    }

    private void OnSceneLoadedUI(Scene scene, LoadSceneMode mode)
    {
        // 플레이 씬 진입 시
        if (scene.buildIndex > 1)
        {            
            // Play Canvas가 비활성화돼있으면 활성화한다.
            if (!playCanvas.activeSelf)
                playCanvas.SetActive(true);
            
            UpdateLevelUI();
            UpdateEtherUI();
            UpdateExpUI();
        }
        else
            playCanvas.SetActive(false);
    }

    public GameObject GenerateEnemyHPbar()
    {
        return Instantiate(enemyHpBar, playCanvas.transform);
    }
}
