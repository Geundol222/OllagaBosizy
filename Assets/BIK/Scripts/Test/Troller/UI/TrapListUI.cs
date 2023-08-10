using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrapListUI : GameSceneUI
{
    private Debuff[] debuffs;
    private TMP_Text debuffText;
    private Image[] debuffIcons;

    private void Start()
    {
        if (GameManager.Team.GetTeam() == PlayerTeam.Troller)
        {
            debuffText = GetComponentInChildren<TMP_Text>();
            debuffIcons = GetComponentsInChildren<Image>();
        }
        else
            Destroy(gameObject);
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

        for (int i = 0; i < debuffIcons.Length - 1; i++)
        {
            debuffIcons[i].sprite = GameManager.Resource.Load<Sprite>($"UI/{debuffs[i].state.ToString()}");
        }
    }

}
