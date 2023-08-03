using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrapListUI : GameSceneUI
{
    private Debuff[] debuffs;
    private TMP_Text[] debuffTexts;

    private void Start()
    {
        debuffTexts = GetComponentsInChildren<TMP_Text>();
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
        if (debuffTexts == null)
            debuffTexts = GetComponentsInChildren<TMP_Text>();

        for (int i = 0; i < debuffTexts.Length; i++)
        {
            debuffTexts[i].text = debuffs[i].state.ToString();
        }
    }

}
