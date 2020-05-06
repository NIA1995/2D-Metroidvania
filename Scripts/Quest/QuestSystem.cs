using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 현재는 필요하지 않음 */
public enum QuestTypeEnum
{
    KillMonster,
    ExchangeItem,
    TalkNPC,
}

public class QuestSystem : MonoBehaviour
{
    [Header("퀘스트 데이터")]
    public int QuestCode;
    public int TargetObjectCode;
    
    [Space]
    public bool IsAccept = false;
    public bool IsClear = false;

    [Header("퀘스트 정보")]
    public string QuestName;
    public int QuestExp;
    public int QuestGold;

    [Header("퀘스트 진행률")]
    public int TargetObjectCount = 0;
    public int NowTargetCount = 0;
    /* 퀘스트 진행률은 플레이어가 가지고 있는게 맞지 않을까? */

    [Header("퀘스트 대화")]
    public string[] CommonSentence;
    public string[] QuestAcceptSentence;
    public string[] QuestClearSentence;

    [Space(20)]
    public GameObject HudImage;

    private void OnMouseDown()
    {
        if(QuestDialouge.Instance.DialougeGroup.alpha == 0)
        {
            if (!IsAccept && !IsClear)
            {
                QuestDialouge.Instance.OnDialouge(this, QuestAcceptSentence);
            }
            else if (IsAccept && !IsClear)
            {
                QuestDialouge.Instance.OnDialouge(this, QuestClearSentence);
            }
            else
            {
                QuestDialouge.Instance.OnDialouge(this, CommonSentence);
            }
        }     
    }

    public void QuestAccept()
    {
        if(!IsAccept && !IsClear)
        {
            IsAccept = true;

            PlayerController.Instance.AcceptQuestList.Add(this);
        }
    }

    public void QuestClear()
    {
        if(TargetObjectCount <= NowTargetCount)
        {
            IsClear = true;

            //PlayerController.Instance.Exp += QuestExp;
            PlayerController.Instance.Gold += QuestGold;

            PlayerController.Instance.AcceptQuestList.Remove(this);

            GameObject Hud = Instantiate(HudImage);
            Hud.GetComponent<HudText>().Alpha = Color.yellow;
            Hud.GetComponent<HudText>().TargetString = "Clear!\n" + "Get Gold : " + QuestGold + "\n" + "Get Exp : " + QuestExp;
            Hud.GetComponent<HudText>().transform.position = PlayerController.Instance.transform.position;
 
            enabled = false;
        }
        else
        {
            GameObject Hud = Instantiate(HudImage);
            Hud.GetComponent<HudText>().Alpha = Color.red;
            Hud.GetComponent<HudText>().TargetString = "Fail! \n" + QuestName + "\n" + NowTargetCount + " / " + TargetObjectCount;
            Hud.GetComponent<HudText>().transform.position = PlayerController.Instance.transform.position;
        }
    }
}
