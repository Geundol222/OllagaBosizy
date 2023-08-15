using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScene : MonoBehaviour
{
    TeamSplitManager split;

    private void Awake()
    {
        split = FindObjectOfType<TeamSplitManager>();
    }

    private void Start()
    {
        split.TeamSplit();
        StartCoroutine(InitRoutine());
    }

    IEnumerator InitRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        if (PhotonNetwork.IsMasterClient)
            GameManager.Scene.LoadScene(Scene.GAME);
    }
}
