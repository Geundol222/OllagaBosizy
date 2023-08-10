using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneTest : BaseSceneTest
{
	protected override IEnumerator LoadingRoutine()
	{
		progress = 0.0f;
		Debug.Log("맵 생성");
		yield return new WaitForSeconds(1f);

		progress = 0.2f;
		Debug.Log("발판 생성");
		yield return new WaitForSeconds(1f);

		progress = 0.4f;
		Debug.Log("등반자 배치");
		yield return new WaitForSeconds(1f);

		progress = 0.6f;
		Debug.Log("방해자 배치");
		yield return new WaitForSeconds(1f);

		progress = 1.0f;
		Debug.Log("Scene Loading 완료");
		yield return new WaitForSeconds(0.5f);
	}
}