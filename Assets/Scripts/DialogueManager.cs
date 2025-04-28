using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance; // Singletone
    public string[] dialogues;

    private Dictionary<Vector2, string[]> dialogueDict; // key는 dialogue Context이며, value는 실제 대화 내용이다.

    // [SerializeField]
    // private TextMeshProUGUI currentDialogueText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        dialogueDict = new Dictionary<Vector2, string[]>();

        GenerateDialogues();
    }

    // 대화 내용을 생성한다.
    private void GenerateDialogues()
    {
        // 릴리
        dialogueDict.Add(new Vector2(1000, 0), new string[]{
            "우로, 늦잠 잤네?",
            "오늘은 레비스 씨가 너에게 할 말이 있다고 하시던데",
            "참, 레비스 씨의 연구실은 마을 호수 뒤편에 있는 거 알지?",
            });

        // 이장
        dialogueDict.Add(new Vector2(1001, 0), new string[]{
            "그래, 마을 생활엔 적응했느냐?",
            "작은 마을이지만 이곳의 사람들은 서로를 도우며 마치 가족처럼 지낸단다",
            "도움이 필요한 사람이 보인다면 말을 걸어보려무나",
            "너도 이제 어엿한 이 마을의 가족이니까 말이야!",
            });

        // 헤르마
        dialogueDict.Add(new Vector2(1010, 0), new string[]{
            "왔구나, 우로",
            "내가 너를 부른 것은 다름이 아니라 줄 선물이 있어서야!",
            "자, 우선 이 검을 받아",
            "그 검으로 너 스스로를 지키도록 해",
            });
        
        // 케이티
        dialogueDict.Add(new Vector2(1002, 0), new string[]{
            "옆에 있는 가게에서 무기와 여러 물건들을 살 수 있어!",
            "...시골 동네라 물건은 많이 없지만",
            });

        // 리즈
        dialogueDict.Add(new Vector2(1003, 0), new string[]{
            "에오스 마을은 이 대륙에서 가장 작은 마을 중 하나야",
            "그렇기에 마을 사람들끼리 더욱 가족처럼 지낼 수 있는거지",
            "그나저나 이 작은 마을에 레비스처럼 대단한 사람이 산다니, ",
            "정말 대단한 일이야!"
            });

    }

    public string[] GetDialogues(int _id, int dialContext)
    {
        Vector2 key = new Vector2(_id, dialContext);
        if (dialogueDict.ContainsKey(key))
            return dialogueDict[key];
        else
            return null;
    }
}
