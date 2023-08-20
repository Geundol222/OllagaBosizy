using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;           //NICKNAME을 받아 변경되는 text
    [SerializeField] TMP_Text playerReady;          //유저의 레디 여부를 보여주는 text
    [SerializeField] Image image;                   //유저 본인의 Entry인지 눈으로 잘 볼 수 있도록 하기위해 image교체를 위해 받아옴
    [SerializeField] Sprite meSprite;               //유저 본인의 Entry인지 눈으로 잘 볼 수 있도록 하기위해 image교체를 위해 받아옴

    //플레이어 이름을 받아서 텍스트화 해주는 함수
    public void SetPlayer(Player player)
    {
        playerName.text = player.NickName;
        playerReady.text = player.GetReady() ? "준비 완료!" : "";
    }

    //팀을 설정해주는 함수
    public void SetPlayerTrollerTeam()
    {
        if(!GameManager.Team.SetTeam(PlayerTeam.Troller))
            GameManager.Team.SwitchTeam(PlayerTeam.Troller);
    }

    public void SetPlayerClimberTeam()
    {
        if(!GameManager.Team.SetTeam(PlayerTeam.Climber))
            GameManager.Team.SwitchTeam(PlayerTeam.Climber);
    }

    //플레이어 프로퍼티가 변경되면 실행되는 함수
    public void ChangeCustomProperty(PhotonHashtable property)
    {
        if (property.TryGetValue(CustomProperty.READY, out object value))
        {
            bool ready = (bool)value;
            playerReady.text = ready ? "준비 완료!" : "";
        }
    }

    //방을 나가면 팀을 초기화 해주는 함수
    public void LeaveRoom()
    {
        GameManager.Team.LeaveTeam();
    }
    
    //유저 본인의 playerentry가 뭔지확실하게 알리기 위한 함수
    public void Sprite()
    {
        image.sprite = meSprite;
    }
}