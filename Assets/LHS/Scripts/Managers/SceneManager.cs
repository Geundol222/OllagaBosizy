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
        yield return new WaitForSeconds(1f);
        GameManager.Sound.Clear();

        PhotonNetwork.LoadLevel((int)scene);

        while (PhotonNetwork.LevelLoadingProgress < 1f)
        {
            yield return null;
        }
        yield break;
    }
}
