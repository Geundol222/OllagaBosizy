using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] List<LoadingPlayer> loadingPlayers;

    private TeamSplitManager splitManager;
    private LoadingPlayer loadingPlayer;
    private Animator anim;

    private void Awake()
    {
        splitManager = FindObjectOfType<TeamSplitManager>();
        anim = GetComponent<Animator>();
        slider.maxValue = 1f;
        slider.minValue = 0f;
        slider.value = 0f;
    }

    private void Start()
    {
        splitManager.TeamSplit();
        StartCoroutine(InitRoutine());
    }

    IEnumerator InitRoutine()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            yield return new WaitUntil(() => { return player.GetClimber() != Climber.None; });

            if (player.GetPlayerTeam() == PlayerTeam.Troller)
            {
                if (player.GetClimber() == Climber.Goblin)
                {
                    loadingPlayers[2].SetNickName(player);
                }
                else if (player.GetClimber() == Climber.Ghost)
                {
                    loadingPlayers[3].SetNickName(player);
                }
            }
            else if (player.GetPlayerTeam() == PlayerTeam.Climber)
            {
                if (player.GetClimber() == Climber.Boy)
                {
                    loadingPlayers[0].SetNickName(player);
                }
                else if (player.GetClimber() == Climber.Girl)
                {
                    loadingPlayers[1].SetNickName(player);
                }
            }
        }
    }

    public void SetProgress(float progress)
    {
        slider.value = progress;
    }
}