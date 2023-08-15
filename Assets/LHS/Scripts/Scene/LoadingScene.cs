using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] LoadingUI loadingUI;

    float progress;

    private void Awake()
    {
        progress = 0.0f;
    }

    private void Start()
    {
        StartCoroutine(LoadingRoutine());
    }

    IEnumerator LoadingRoutine()
    {
        while(progress < 1f)
        {
            progress += Time.deltaTime * 0.2f;
            loadingUI.SetProgress(Mathf.Lerp(0f, 1f, progress / 1f));

            yield return null;
        }

        if (PhotonNetwork.IsMasterClient)
            GameManager.Scene.LoadScene(Scene.GAME);
        yield return new WaitForSecondsRealtime(0.1f);
    }
}
