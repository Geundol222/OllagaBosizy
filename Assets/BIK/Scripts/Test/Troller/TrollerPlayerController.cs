using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollerPlayerController : MonoBehaviourPun
{
    private Platform currentPlatform;
    public Platform _currentPlatform { get { return currentPlatform; } }
    private Platform prevPlatform;
    public Platform _prevPlatform { get { return prevPlatform; } }

    /// <summary>
    ///  230730 TODO  00:54 
    /// 클릭한 발판의 SetTrapUI 그리고 이전에 클릭한 SetTrapUI를 비교 
    /// 서로 다르다면 이전에 클릭한 SetTrapUI를 Close 해주고 클릭한 발판의 SetTrapUI를 출력
    /// 
    /// 근데 이전 발판 현재 발판은 DataManager에서 관리해줘야 할 것 같은데 ..
    /// </summary>
    // Start is called before the first frame update

    public void ClearBothPlatform()
    {
        currentPlatform = null;
        prevPlatform = null;
    }

    public void ClearCurrentPlatform()
    {
        currentPlatform = null;
    }

    public void SetCurrentPlatform(Platform platform)
    {
        currentPlatform = platform;
    }

    public void SetPrevPlatform(Platform platform)
    {
        prevPlatform = platform;
    }
}
