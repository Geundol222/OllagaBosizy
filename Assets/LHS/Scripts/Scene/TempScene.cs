using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempScene : MonoBehaviour
{
    [SerializeField] TMP_Text loadingText;

    private int textCount;

    private void Start()
    {
        textCount = 0;
        GameManager.Team.TeamSplit();
        StartCoroutine(InitRoutine());
    }

    IEnumerator InitRoutine()
    {
        while (textCount < 3)
        {
            loadingText.text = "ÆÀ º¯°æÁß.";
            yield return new WaitForSeconds(0.2f);
            loadingText.text = "ÆÀ º¯°æÁß..";
            yield return new WaitForSeconds(0.2f);
            loadingText.text = "ÆÀ º¯°æÁß...";
            yield return new WaitForSeconds(0.2f);
            textCount++;
        }
        

        if (PhotonNetwork.IsMasterClient)
            GameManager.Scene.LoadScene(Scene.GAME);
    }
}
