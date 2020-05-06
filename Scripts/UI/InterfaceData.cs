using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceData : MonoBehaviour
{
    public CanvasGroup Group;

    public Text LevelText;

    public Slider HPSlider;
    public Slider MPSlider;
    public Slider EXPSlider;

    private void Start()
    {
        EnemyController.Dead += UIRefresh;

        Group.alpha = 1;

        //LevelText.text = "LV." + PlayerController.Instance.Level;

        HPSlider.maxValue = PlayerController.Instance.MaxHP;
        //MPSlider.maxValue = PlayerController.Instance.MaxMP;
        //EXPSlider.maxValue = PlayerController.Instance.MaxExp;
    }

    private void Update()
    {
        HPSlider.value = PlayerController.Instance.HP;
        //MPSlider.value = PlayerController.Instance.MP;
        //EXPSlider.value = PlayerController.Instance.Exp;
    }

    private void UIRefresh()
    {
        //LevelText.text = "LV." + PlayerController.Instance.Level;

        HPSlider.value = PlayerController.Instance.HP;
        //MPSlider.value = PlayerController.Instance.MP;
        //EXPSlider.value = PlayerController.Instance.Exp;

        HPSlider.maxValue = PlayerController.Instance.MaxHP;
        //MPSlider.maxValue = PlayerController.Instance.MaxMP;
        //EXPSlider.maxValue = PlayerController.Instance.MaxExp;
    }
}
