using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase Instance;

    public List<ItemData> Items = new List<ItemData>();

    public int SelectedItem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            UseItem(SelectedItem);
        }
    }

    private void Start()
    {
        SelectedItem = 0;

        AddItem("슬라임", 1, 1, "슬라임의 진액", ItemTypeEnum.Consumption);
        AddItem("뼈", 1, 1, "몬스터가 남긴 뼈", ItemTypeEnum.Etc);
    }

    void AddItem(string Name, int Value, int Price, string Description, ItemTypeEnum Type)
    {
        Items.Add(new ItemData(Name, Value, Price, Description, Type, Resources.Load<Sprite>("Sprites/Items/" + Name) as Sprite));
    }
    
    public void UseItem(int ItemIndex)
    {
        switch (Items[ItemIndex].ItemType)
        {
            case ItemTypeEnum.Consumption:  UseConsumptionItem(ItemIndex);  break;
            case ItemTypeEnum.Equipment:    UseEquipmentItem(ItemIndex);    break;
            case ItemTypeEnum.Etc:          UseEtcItem(ItemIndex);          break;
            default:                                                        break;
        }
    }

    private void UseConsumptionItem(int ItemIndex)
    {
        switch(Items[ItemIndex].ItemValue)
        {
            case 1:

                if(PlayerController.Instance.HP < PlayerController.Instance.MaxHP)
                {
                    PlayerController.Instance.HP += 1;
                }

                break;
        }
    }

    private void UseEquipmentItem(int ItemIndex)
    {

    }

    private void UseEtcItem(int ItemIndex)
    {

    }
}
