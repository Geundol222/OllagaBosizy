using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Debuff_State { None,NoColider,Surface,Spring,Ice,Length }

[Serializable]
public class Debuff : IDebuff
{
    public Debuff_State state;
    
    public Debuff(int index = 0)
    {
        this.state = (Debuff_State)index;
    }

    public void DebugCurrentState()
    {
        Debug.Log(state.ToString());
    }

    public void SetState(int index)
    {
        state = (Debuff_State) index;
    }
    public IDebuff clone()
    {
        return this.MemberwiseClone() as IDebuff;
    }
}
