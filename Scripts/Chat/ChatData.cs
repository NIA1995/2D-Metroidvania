using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChatData : MonoBehaviour
{
    public string[] SelfSentences;

    private bool IsTalking = false;

    public Transform ChatTransform;
    public GameObject BoxPrefab;

    /* 만들어진 박스 인스턴스 */
    private GameObject BoxInstant;

    /* 대화 실행 이벤트 */
    public delegate void ChatEvent();
    public static event ChatEvent Chat;

    private void Start()
    {
        QuestSystem.StartChat += PauseSelfChat;
        QuestSystem.EndChat += StartSelfChat;

        StartCoroutine(RepeatChat());
    }

    public void Talk(string[] Data)
    {
        BoxInstant = Instantiate(BoxPrefab);
        BoxInstant.GetComponent<ChatSystem>().OnChatDialogue(Data, ChatTransform);
    }

    IEnumerator RepeatChat()
    {
        Talk(SelfSentences);

        yield return new WaitForSeconds(15.0f);

        StartCoroutine(RepeatChat());
    }

    public void StartSelfChat()
    {
        IsTalking = false;

        if (BoxInstant == null)
        {
            StartCoroutine(RepeatChat());
        }

        Chat();
    }

    public void PauseSelfChat()
    {
        StopAllCoroutines();
        Destroy(BoxInstant);

        IsTalking = true;

        Chat();
    }
}
