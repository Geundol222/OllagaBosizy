using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingPlayer : MonoBehaviour
{
    [SerializeField] TMP_Text nickNameText;

    public void SetNickName(Player player)
    {
        nickNameText.text = player.NickName;
    }
}
