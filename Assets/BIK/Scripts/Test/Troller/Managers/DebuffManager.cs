using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    public void ClimberStepOnPlatform(Platform platform)
    {
        if(platform._currentDebuffState != Debuff_State.None)
        {
            platform.StartDebuffCountDown();
        } else
        {
            Debug.Log("디버프가 없는 발판");
        }
    }
}
