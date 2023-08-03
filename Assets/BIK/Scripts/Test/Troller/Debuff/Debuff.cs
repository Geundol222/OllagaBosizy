using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Debuff_State { None,NoColider,Surface,ZeroSpeed,Spring,Ice,Length }

public class Debuff : IDebuff
{
    public Debuff_State state;
    public Platform platform;
    public PhysicsMaterial2D[] debuff_PhysicsMaterials;

    public Debuff(int index = 0)
    {
        this.state = (Debuff_State)index;
        debuff_PhysicsMaterials = new PhysicsMaterial2D[(int)Debuff_State.Length];
        InitPhysicsList();
    }

    public void InitPhysicsList()
    {
        debuff_PhysicsMaterials[(int)Debuff_State.Ice] = GameManager.Resource.Load<PhysicsMaterial2D>("Debuff/Ice");
        debuff_PhysicsMaterials[(int)Debuff_State.Spring] = GameManager.Resource.Load<PhysicsMaterial2D>("Debuff/Spring");
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
        if (this.platform != platform)
        {
            this.platform = platform;
        }
        BoxCollider2D boxCollider2D = platform.GetComponent<BoxCollider2D>();
        SurfaceEffector2D surfaceEffector2D = platform.GetComponent<SurfaceEffector2D>();

        Debug.Log($"플랫폼에 {state} 함정 설치");
        // 각 state에 따른  AddComponent 또는 Player 수치 변화 함수 호출 
        // 아래 소스를 대리자를 사용, platform 내에서 구현되게 로직 수정
        switch (state)
        {
            case Debuff_State.NoColider: boxCollider2D.isTrigger = true; break; // 1번(충돌체 없애기)일 경우
            case Debuff_State.Surface: surfaceEffector2D.enabled = true; break; // 2번(표면이펙트)일 경우
            case Debuff_State.Spring: boxCollider2D.sharedMaterial = debuff_PhysicsMaterials[(int)Debuff_State.Spring]; break;
            case Debuff_State.Ice: boxCollider2D.sharedMaterial = debuff_PhysicsMaterials[(int)Debuff_State.Ice]; break;
            case Debuff_State.None:
                {
                    boxCollider2D.sharedMaterial = null;
                    boxCollider2D.isTrigger = false;
                    surfaceEffector2D.enabled = false;
                    break;
                }
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
