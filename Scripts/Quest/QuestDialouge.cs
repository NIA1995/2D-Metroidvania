using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestDialouge : MonoBehaviour, IPointerDownHandler
{
    public Text DialougeText;
    public Text QuestName;
    public GameObject NextText;

    public CanvasGroup DialougeGroup;
    public Queue<string> Sentences;

    private Button Accept;
    private Button Cancel;
    private Button Clear;

    private string CurrentSentence;

    public float TypingSpeed = 0.1f;
    private bool IsTyping;

    public static QuestDialouge Instance = null;

    private QuestSystem NowQuest;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Sentences = new Queue<string>();
    }

    public void OnDialouge(QuestSystem TargetQuest, string[] Lines)
    {
        NowQuest = TargetQuest;

        Accept = transform.Find("Accept").GetComponent<Button>();
        Cancel = transform.Find("Cancel").GetComponent<Button>();
        Clear = transform.Find("Clear").GetComponent<Button>();

        Sentences.Clear();

        foreach (var Item in Lines)
        {
            Sentences.Enqueue(Item);
        }

        DialougeGroup.alpha = 1;
        DialougeGroup.blocksRaycasts = true;

        NextSentence();
    }

    public void NextSentence()
    {
        if(Sentences.Count != 0)
        {
            CurrentSentence = Sentences.Dequeue();
            IsTyping = true;
            NextText.SetActive(false);
            StartCoroutine(TypingAnimation(CurrentSentence));
        }
        else
        {
            SetButtonState();
        }
    }

    private void SetButtonState()
    {
        if (!NowQuest.IsAccept)
        {
            QuestName.text = NowQuest.QuestName;

            Accept.gameObject.SetActive(true);
            Cancel.gameObject.SetActive(true);
            Clear.gameObject.SetActive(false);

            Accept.onClick.AddListener(NowQuest.QuestAccept);
            Accept.onClick.AddListener(CloseChatBox);

            Clear.onClick.AddListener(CloseChatBox);
            Clear.onClick.AddListener(NowQuest.QuestClear);

            Cancel.onClick.AddListener(CloseChatBox);
        }
        else if (NowQuest.IsAccept && !NowQuest.IsClear)
        {
            QuestName.text = "";

            Accept.gameObject.SetActive(false);
            Cancel.gameObject.SetActive(false);

            Clear.gameObject.SetActive(true);
        }
        else
        {
            QuestName.text = "";

            Accept.gameObject.SetActive(false);
            Cancel.gameObject.SetActive(false);
            Clear.gameObject.SetActive(false);

            CloseChatBox();
        }
    }

    public void CloseChatBox()
    {
        DialougeGroup.alpha = 0;
        DialougeGroup.blocksRaycasts = false;

        Accept.gameObject.SetActive(false);
        Cancel.gameObject.SetActive(false);
        Clear.gameObject.SetActive(false);
    }

    IEnumerator TypingAnimation(string Line)
    {
        DialougeText.text = "";

        foreach (var Item in Line.ToCharArray())
        {
            DialougeText.text += Item;

            yield return new WaitForSeconds(TypingSpeed);
        }
    }

    void Update()
    {
        if(DialougeText.text.Equals(CurrentSentence))
        {
            NextText.SetActive(true);
            IsTyping = false;
        }
    }

    public void OnPointerDown(PointerEventData EventData)
    {
        if (!IsTyping)
        {
            NextSentence();
        }
    }
}
