using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatSystem : MonoBehaviour
{
    public Queue<string> ChatList;

    public string CurrentChat;
    public TextMeshPro Chat;

    public GameObject Box;

    public void OnChatDialogue(string[] Lines, Transform ChatTransform)
    {
        if(ChatList == null)
        {
            transform.position = ChatTransform.position;

            ChatList = new Queue<string>();
            ChatList.Clear();

            foreach (var Line in Lines)
            {
                ChatList.Enqueue(Line);
            }
        }

        StartCoroutine(SelfDialogueProgress(ChatTransform));
    }

    IEnumerator SelfDialogueProgress(Transform ChatTransform)
    {
        yield return null;

        while (ChatList.Count > 0)
        {
            CurrentChat = ChatList.Dequeue();
            Chat.text = CurrentChat;

            float ChatxValue = Chat.preferredWidth;
            ChatxValue = (ChatxValue > 3) ? 3 : ChatxValue + 0.3f;

            Box.transform.localScale = new Vector2(ChatxValue, Chat.preferredHeight + 0.3f);

            transform.position = new Vector2(ChatTransform.position.x, ChatTransform.position.y + Chat.preferredHeight / 2);

            yield return new WaitForSeconds(5f);
        }

        Destroy(gameObject);
    }
}
