using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum Scene { LOBBY, GAME };

public class SceneManager : MonoBehaviour
{
    // private LoadingUI loadingUI;

    private Scene currentScene;
    public Scene CurrentScene { get { return currentScene; } }

    private BaseScene curScene;
    public BaseScene CurScene
    {
        get
        {
            if (curScene == null)
                curScene = FindObjectOfType<BaseScene>();

            return curScene;
        }
    }

    private void Awake()
    {
        // LoadingUI ui = Resources.Load<LoadingUI>("UI/LoadingSceneUI");
        // loadingUI = Instantiate(ui);
        // loadingUI.transform.SetParent(transform, false);
    }

    public void LoadScene(Scene scene)
    {
        currentScene = scene;

        StartCoroutine(LoadingRoutine(scene));
    }

    IEnumerator LoadingRoutine(Scene scene)
    {
        // loadingUI.FadeOut();
        yield return new WaitForSeconds(1f);
        GameManager.Sound.Clear();
        yield return new WaitUntil(() => { return GameManager.Sound.IsMuted(); });
        Time.timeScale = 0f;

        PhotonNetwork.AutomaticallySyncScene = true;

        //AsyncOperation oper = UnitySceneManager.LoadSceneAsync(index);
        PhotonNetwork.LoadLevel((int)scene);

        while (PhotonNetwork.LevelLoadingProgress < 1f)
        {
            // loadingUI.SetProgress(Mathf.Lerp(0f, 0.5f, PhotonNetwork.LevelLoadingProgress));
            yield return null;
        }

        // CurScene.LoadAsync();
        // while (CurScene.progress < 1f)
        // {
        //     // loadingUI.SetProgress(Mathf.Lerp(0.5f, 1.0f, CurScene.progress));
        //     yield return null;
        // }

        PhotonNetwork.AutomaticallySyncScene = false;

        Time.timeScale = 1f;
        // loadingUI.FadeIn();
        yield return new WaitWhile(() => { return GameManager.Sound.IsMuted(); });
    }
}
