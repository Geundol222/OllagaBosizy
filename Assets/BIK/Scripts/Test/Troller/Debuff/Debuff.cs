using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Debuff_State { NoColider,HeavyGravity,Surface,ZeroSpeed,Length }

public class Debuff : MonoBehaviour,IDebuff
{
    public Debuff_State state;

    public Debuff(int index)
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

    public void SetTrap(Platform platform)
    {
        Debug.Log($"ÇÃ·§Æû¿¡ {state} ÇÔÁ¤ ¼³Ä¡");
    }

    public IDebuff clone()
    {
        return this.MemberwiseClone() as IDebuff;
    }
}
