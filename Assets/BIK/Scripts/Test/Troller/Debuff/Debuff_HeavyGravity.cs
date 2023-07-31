using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Debuff_HeavyGravity : MonoBehaviour, IDebuff
{
    // 현재 디버프를 복제
    public IDebuff clone()
    {
        return this.MemberwiseClone() as IDebuff;
    }
}
