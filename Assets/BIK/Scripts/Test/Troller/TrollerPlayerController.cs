using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrollerPlayerController : MonoBehaviourPun
{
    private PlayerInput inputAction;
    private GameObject[] allPlatformList;
    private DebuffManager debuffManager { get { return GameManager.TrollerData.debuffManager; } }
    int platformList_index = 0;

    private void Awake()
    {
        inputAction = GetComponent<PlayerInput>();

        if (GameManager.Team.GetTeam() == PlayerTeam.Troller)
        {
            if (photonView.IsMine)
            {
                GameManager.Resource.Instantiate<Canvas>("Troller/TrapList");
                GameManager.Resource.Instantiate<Canvas>("Troller/LeftTrap");
                debuffManager.DebuffQueueInit();
            }
            else
            {
                Destroy(inputAction);
                //Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        if (!photonView.IsMine)
            return;

        allPlatformList = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject go in allPlatformList)
        {
            go.name = $"Platform_{platformList_index++}";
        }
    }

    public void ClearBothPlatform()
    {
        GameManager.TrollerData.currentPlatform = null;
        GameManager.TrollerData.prevPlatform = null;
    }

    public void ClearCurrentPlatform()
    {
        GameManager.TrollerData.currentPlatform = null;
    }

    public void SetCurrentPlatform(Platform platform)
    {
        GameManager.TrollerData.currentPlatform = platform;
    }

    public void SetPrevPlatform(Platform platform)
    {
        GameManager.TrollerData.prevPlatform = platform;
    }
}
