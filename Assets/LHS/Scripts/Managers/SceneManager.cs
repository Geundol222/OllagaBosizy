using Photon.Pun;
using System.Collections;
using UnityEngine;

public enum Scene { LOBBY, LOADING, GAME, TEMP, SCORE };

public class SceneManager : MonoBehaviour
{
    private Scene currentScene;
    public Scene CurrentScene { get { return currentScene; } }

    public void LoadScene(Scene scene)
    {
        currentScene = scene;
        StartCoroutine(LoadingRoutine(scene));
    }

    IEnumerator LoadingRoutine(Scene scene)
    {
        GameManager.Sound.Clear();
        yield return new WaitUntil(() => { return GameManager.Sound.IsMuted(); });

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel((int)scene);

        while (PhotonNetwork.LevelLoadingProgress < 1f)
        {
            yield return null;
        }

        GameManager.Sound.FadeInAudio();
        yield return new WaitWhile(() => { return GameManager.Sound.IsMuted(); });

        yield break;
    }
}
