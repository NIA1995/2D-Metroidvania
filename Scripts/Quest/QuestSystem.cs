using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestSystem : MonoBehaviour, IPointerDownHandler
{
    public Text DialougeText;
    public GameObject NextText;

    public CanvasGroup DialougeGroup;
    public Queue<string> Sentences;

    public Button Accept;
    public Button Cancel;
    public Button Clear;

    private string CurrentSentence;

    public float TypingSpeed = 0.1f;
    private bool IsTyping;

    public static QuestSystem Instance = null;

    public delegate void ChatStart();
    public static event ChatStart StartChat;

    public delegate void ChatEnd();
    public static event ChatEnd EndChat;

    private QuestData NowQuest;

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

    public void OnDialouge(QuestData Target, string[] Lines)
    {
        NowQuest = Target;

        Accept = transform.Find("Accept").GetComponent<Button>();
        Cancel = transform.Find("Cancel").GetComponent<Button>();
        Clear = transform.Find("Clear").GetComponent<Button>();

        if (!NowQuest.IsAccept)
        {
            Clear.gameObject.SetActive(false);

            Accept.onClick.AddListener(NowQuest.QuestAccept);
            Accept.onClick.AddListener(CloseChatBox);

            Clear.onClick.AddListener(CloseChatBox);
            Clear.onClick.AddListener(NowQuest.QuestClear);

            Cancel.onClick.AddListener(CloseChatBox);
        }
        else
        {
            Accept.gameObject.SetActive(false);
            Cancel.gameObject.SetActive(false);

            Clear.gameObject.SetActive(true);          
        }

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
    }

    public void CloseChatBox()
    {
        DialougeGroup.alpha = 0;
        DialougeGroup.blocksRaycasts = false;
        EndChat();
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
        StartChat();

        if (!IsTyping)
        {
            NextSentence();
        }
    }
}
