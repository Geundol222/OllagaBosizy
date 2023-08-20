using System.Collections;
using TMPro;
using UnityEngine;

public class LeftTrapUI : GameSceneUI
{
    [SerializeField] private TMP_Text countText;
    private Coroutine trollerDataCoroutine;
    private Coroutine textUICoroutine;


    // Start is called before the first frame update
    private void Start()
    {
        if (GameManager.Team.GetTeam() == PlayerTeam.Troller)
        {
            trollerDataCoroutine = StartCoroutine(SetTrollerDataCoroutine());
        }
        else
            Destroy(gameObject);
    }

    IEnumerator SetTrollerDataCoroutine()
    {
        yield return new WaitUntil(() => { return GameManager.TrollerData.trollerPlayerController != null; });
        SetCurrentTrapCountInfo();
        yield break;
    }

    public void SetCurrentTrapCountInfo()
    {
        countText.text = $"{GameManager.TrollerData._setTrapPlatforms.Count} / {GameManager.TrollerData.maxSetTrapPlatforms}";
    }
}
