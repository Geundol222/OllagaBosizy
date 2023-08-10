using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrapListUI : GameSceneUI
{
    private Debuff[] debuffs;
    private TMP_Text[] debuffTexts;
    private Image[] debuffIcons;

    private void Start()
    {
        debuffTexts = GetComponentsInChildren<TMP_Text>();
        debuffIcons = GetComponentsInChildren<Image>();
    }

    public void UpdateList(Debuff[] debuffs)
    {
        this.debuffs = debuffs;
        SetTrapListUI();
    }

    public void SetTrapListUI()
    {
        if (debuffs == null)
            return;
        if (debuffIcons == null)
        {
            debuffIcons = GetComponentsInChildren<Image>();
        }

        Debug.Log($"디버프 아이콘 길이 {debuffIcons.Length}");
        for (int i = 0; i < debuffIcons.Length; i++)
        {
            debuffIcons[i].sprite = GameManager.Resource.Load<Sprite>($"UI/{debuffs[i].state.ToString()}");
        }
    }

}
