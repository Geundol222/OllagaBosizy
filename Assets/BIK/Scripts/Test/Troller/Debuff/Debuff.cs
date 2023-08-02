using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Debuff_State { None,NoColider,Surface,ZeroSpeed,Length }

public class Debuff : MonoBehaviour,IDebuff
{
    public Debuff_State state;
    public Platform platform;

    public Debuff(int index)
    {
        this.state = (Debuff_State)index;
    }

    public void DebugCurrentState()
    {
        Debug.Log(state.ToString());
    }

    public void SetState(int index)
    {
        state = (Debuff_State) index;
    }

    public void SetTrap(Platform platform)
    {
        this.platform = platform;
        Debug.Log($"플랫폼에 {state} 함정 설치");
        // 각 state에 따른  AddComponent 또는 Player 수치 변화 함수 호출 
        // 아래 소스를 대리자를 사용, platform 내에서 구현되게 로직 수정
        switch (state)
        {
            case Debuff_State.NoColider: platform.gameObject.GetComponent<PlatformEffector2D>().enabled = false; break; // 1번(충돌체 없애기)일 경우
            case Debuff_State.Surface: //platform.gameObject.GetComponent<SurfaceEffector2D>().enabled = true; break; // 2번(표면이펙트)일 경우
            default: Debug.Log($"{state}는 현재 구현예정"); break;
            // 3번(플레이어 속도 순식간에 0으로 만들기)인 경우
        }
    }

    public void DisableTrap()
    {
        state = Debuff_State.None;
        platform = null;
    }

    public IDebuff clone()
    {
        return this.MemberwiseClone() as IDebuff;
    }
}
