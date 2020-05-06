using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    public Image HPImage;
    public Image NowItemImage;

    void Start()
    {
        NowItemImage.sprite = ItemDataBase.Instance.Items[ItemDataBase.Instance.SelectedItem].ItemImage;
    }

    void Update()
    {
        HPImage.rectTransform.sizeDelta = new Vector2(24 * PlayerController.Instance.HP, HPImage.rectTransform.rect.height);

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(ItemDataBase.Instance.Items.Count > ItemDataBase.Instance.SelectedItem + 1)
            {
                ItemDataBase.Instance.SelectedItem++;
            }
            else
            {
                ItemDataBase.Instance.SelectedItem = 0;
            }

            NowItemImage.sprite = ItemDataBase.Instance.Items[ItemDataBase.Instance.SelectedItem].ItemImage;
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {

        }
    }
}
