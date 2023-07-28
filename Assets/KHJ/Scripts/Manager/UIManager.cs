using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private static EventSystem eventSystem;
    private Canvas popUpCanvas;
    private Canvas windowCanvas;
    private Canvas inGameCanvas;
    private Stack<PopUpUI> popUpStack;

    private void Awake()
    {
        eventSystem = GameManager.Resource.Instantiate<EventSystem>("UI/EventSystem");
        eventSystem.transform.parent = transform;
        popUpCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        popUpCanvas.gameObject.name = "PopUpCanvas";
        popUpCanvas.sortingOrder = 100;
        popUpStack = new Stack<PopUpUI>();

        inGameCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        inGameCanvas.gameObject.name = "InGameCanvas";
        inGameCanvas.sortingOrder = 0;
    }

    public void StartScene()
    {
        popUpCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        popUpCanvas.gameObject.name = "PopUpCanvas";
        popUpCanvas.sortingOrder = 100;
        popUpStack = new Stack<PopUpUI>();

        inGameCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        inGameCanvas.gameObject.name = "InGameCanvas";
        inGameCanvas.sortingOrder = 0;
    }

    public T OpenPopUpUI<T>(T popUpUI) where T : PopUpUI
    {
        if (popUpStack.Count > 0)
        {
            PopUpUI prevUI = popUpStack.Peek();
            prevUI.gameObject.SetActive(false);
        }
        T ui = GameManager.Pool.GetUI(popUpUI);
        ui.transform.SetParent(popUpCanvas.transform, false);

        popUpStack.Push(ui);

        Time.timeScale = 0;

        return ui;
    }

    public void OpenPopUpUI(string path)
    {
        PopUpUI ui = GameManager.Resource.Load<PopUpUI>(path);
        OpenPopUpUI(ui);
    }

    public void ClosePopUpUI()
    {
        PopUpUI ui = popUpStack.Pop();
        GameManager.Pool.ReleaseUI(ui.gameObject);

        if (popUpStack.Count > 0)
        {
            PopUpUI curUI = popUpStack.Peek();
            curUI.gameObject.SetActive(true);
        }

        if (popUpStack.Count == 0)
        {
            Time.timeScale = 1f;
        }
    }


    public T ShowInGameUI<T>(T ingameui) where T : InGameUI
    {
        T ui = GameManager.Pool.GetUI(ingameui);
        ui.transform.SetParent(inGameCanvas.transform.transform, false);

        return ui;
    }

    public T ShowInGameUI<T>(string path) where T : InGameUI
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowInGameUI(ui);
    }

    public void CloseInGameUI<T>(T inGameUI) where T : InGameUI
    {
        GameManager.Pool.ReleaseUI(inGameUI.gameObject);
    }
}
