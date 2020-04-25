using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCController : MonoBehaviour
{
    public string Name;
    public Transform TextPosition;
    public GameObject NameBoxPrefab;

    private TextMeshPro NameText;
    private Transform NameBoxSize;

    void Start()
    {
        GameObject NameBox = Instantiate(NameBoxPrefab);
        NameText = NameBox.GetComponentInChildren<TextMeshPro>();
        NameText.text = Name;

        NameBoxSize = NameBox.gameObject.transform.Find("ChatBox").transform;

        NameBoxSize.transform.localScale = new Vector2(NameText.preferredWidth + 0.1f, NameText.preferredHeight + 0.1f);
        NameBox.transform.position = new Vector2(TextPosition.position.x, TextPosition.position.y + NameText.preferredHeight / 2);
    }
}
