using System.Collections;
using TMPro;
using UnityEngine;

public class LeftTrapUI : GameSceneUI
{
    private TMP_Text[] texts;
    private Coroutine trollerDataCoroutine;

    private void Awake()
    {
        texts = GetComponentsInChildren<TMP_Text>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        trollerDataCoroutine = StartCoroutine(SetTrollerDataCoroutine());
    }

    IEnumerator SetTrollerDataCoroutine()
    {
        yield return new WaitUntil(() => { return GameManager.TrollerData.trollerPlayerController != null; });
        SetCurrentTrapCountInfo();
        yield break;
    }

    public void SetCurrentTrapCountInfo()
    {
        texts[1].text = $"{GameManager.TrollerData._setTrapPlatforms.Count} / {GameManager.TrollerData.maxSetTrapPlatforms}";
    }
}
