using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviourPun
{
    [SerializeField] Slider slider;
    [SerializeField] List<Transform> spawnPoints;

    private LoadingPlayer loadingPlayer;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("LoadingInitRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void LoadingInitRPC()
    {
        StartCoroutine(InitRoutine());
    }

    IEnumerator InitRoutine()
    {
        yield return new WaitUntil(() => { return PhotonNetwork.LocalPlayer.GetClimber() != Climber.None; });

        if (PhotonNetwork.LocalPlayer.GetPlayerTeam() == PlayerTeam.Troller)
        {
            if (PhotonNetwork.LocalPlayer.GetClimber() == Climber.Goblin)
            {
                loadingPlayer = PhotonNetwork.Instantiate("Loading/Goblin", spawnPoints[2].transform.position, spawnPoints[2].transform.rotation).GetComponent<LoadingPlayer>();
                loadingPlayer.gameObject.transform.parent = transform;
                loadingPlayer.SetNickName(PhotonNetwork.LocalPlayer);
            }
            else if (PhotonNetwork.LocalPlayer.GetClimber() == Climber.Ghost)
            {
                loadingPlayer = PhotonNetwork.Instantiate("Loading/Ghost", spawnPoints[3].transform.position, spawnPoints[3].transform.rotation).GetComponent<LoadingPlayer>();
                loadingPlayer.gameObject.transform.parent = transform;
                loadingPlayer.SetNickName(PhotonNetwork.LocalPlayer);
            }
        }
        else if (PhotonNetwork.LocalPlayer.GetPlayerTeam() == PlayerTeam.Climber)
        {
            if (PhotonNetwork.LocalPlayer.GetClimber() == Climber.Boy)
            {
                loadingPlayer = PhotonNetwork.Instantiate("Loading/Boy", spawnPoints[0].transform.position, spawnPoints[0].transform.rotation).GetComponent<LoadingPlayer>();
                loadingPlayer.gameObject.transform.parent = transform;
                loadingPlayer.SetNickName(PhotonNetwork.LocalPlayer);
            }
            else if (PhotonNetwork.LocalPlayer.GetClimber() == Climber.Girl) 
            {
                loadingPlayer = PhotonNetwork.Instantiate("Loading/Girl", spawnPoints[1].transform.position, spawnPoints[1].transform.rotation).GetComponent<LoadingPlayer>();
                loadingPlayer.gameObject.transform.parent = transform;
                loadingPlayer.SetNickName(PhotonNetwork.LocalPlayer);
            }
        }

        yield break;
    }

    public void FadeIn()
    {
        anim.SetBool("IsActive", true);
    }

    public void FadeOut()
    {
        anim.SetBool("IsActive", false);
    }

    public void SetProgress(float progress)
    {
        slider.value = progress;
    }
}