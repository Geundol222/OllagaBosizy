using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapListUI : InGameUI
{
    private Debuff[] debuffs;

    private void Awake()
    {
        Debug.Log("Awake");
    }

    private void Start()
    {
        Debug.Log("Start");
    }
    public void UpdateList(Debuff[] debuffs)
    {
        this.debuffs = debuffs;
    }
    public void SetTrapListUI()
    {

    }

}
