using System.Collections;
using TMPro;
using UnityEngine;

public class LeftTrapUI : GameSceneUI
{
    private TMP_Text[] texts;
    private Coroutine trollerDataCoroutine;
    private Coroutine textUICoroutine;


    // Start is called before the first frame update
    private void Start()
    {
        if (GameManager.Team.GetTeam() == PlayerTeam.Troller)
        {
            trollerDataCoroutine = StartCoroutine(SetTrollerDataCoroutine());
            textUICoroutine = StartCoroutine(FindUITexts());
        }
        else
            Destroy(gameObject);
    }
    IEnumerator FindUITexts()
    {
        yield return new WaitUntil(() => { return gameObject.GetComponentsInChildren<TMP_Text>() != null; });
        texts = GetComponentsInChildren<TMP_Text>();
        yield break;
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
