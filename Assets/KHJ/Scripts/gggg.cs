using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gggg : MonoBehaviour
{
    void Start()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.GetPhotonTeam().Name);
    }
}
