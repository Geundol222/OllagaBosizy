using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    private LoadingUI loadingUI;
    private int nextIndex = 0;

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
        LoadingUI ui = Resources.Load<LoadingUI>("UI/LoadingSceneUI");
        loadingUI = Instantiate(ui);
        loadingUI.transform.SetParent(transform, false);
    }

    public void NextScene()
    {
        int index = UnitySceneManager.GetActiveScene().buildIndex;
        if (UnitySceneManager.sceneCountInBuildSettings - 1 > index)
            nextIndex = index + 1;
        else
            nextIndex = index;

        LoadScene(nextIndex);
    }

    public void LoadScene(int index)
    {
        StartCoroutine(LoadingRoutine(index));
    }

    IEnumerator LoadingRoutine(int index)
    {
        loadingUI.FadeOut();
        yield return new WaitForSeconds(1f);
        GameManager.Sound.Clear();
        yield return new WaitUntil(() => { return GameManager.Sound.IsMuted(); });
        Time.timeScale = 0f;

        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(index);

        while (!oper.isDone)
        {
            loadingUI.SetProgress(Mathf.Lerp(0f, 0.5f, oper.progress));
            yield return null;
        }

        GameManager.Pool.InitPool();
        GameManager.UI.InitUI();
        GameManager.Sound.InitSound();
        GameManager.Sound.FadeInAudio();

        CurScene.LoadAsync();
        while (CurScene.progress < 1f)
        {
            loadingUI.SetProgress(Mathf.Lerp(0.5f, 1.0f, CurScene.progress));
            yield return null;
        }

        Time.timeScale = 1f;
        loadingUI.FadeIn();
        yield return new WaitWhile(() => { return GameManager.Sound.IsMuted(); });
    }
}
