using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gggg : MonoBehaviour
{
    public LogImage logImage;
    void Start()
    {
        logImage.gameObject.SetActive(true);
        logImage.SetText(PhotonNetwork.LocalPlayer.GetPhotonTeam().Name);
    }
}
