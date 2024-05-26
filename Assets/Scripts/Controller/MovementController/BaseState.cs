using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public abstract class BaseState 
{
    public abstract void EnterState(StateManager manager);
    public abstract void UpdateState(StateManager manager); 
    public abstract void ExitState(StateManager manager);

    public abstract void Init(StateManager manager);
}

public struct SpellData
{
  public SpellData(Define.SpellType type,Define.MouseEvent input)
    {
        spellType = type;
        inputType = input;
    }
    Define.SpellType spellType;
    Define.MouseEvent inputType;

   public Define.SpellType SpellType =>spellType;
   public Define.MouseEvent InputType =>inputType;

    

}

public class SkillState : BaseState
{
    StateManager _manager;
    UnityEvent animCompleteEvent;
    int curruntHash = 0;
    SpellData data;

    readonly int MeeleHash = Animator.StringToHash("Attack");
    readonly int MagicHash = Animator.StringToHash("Magic");
    readonly int ChannelingHash = Animator.StringToHash("StartChanneling");

    bool isChanneling = false;

    public override void EnterState(StateManager manager)
    {
        manager.StateType = Define.State.Skill;

        manager._anim.CrossFade(curruntHash, 0.1f);

        

    }
    public void OnComplete()
    {
        if(_manager.StateType == Define.State.Skill)
            _manager.SwitchState(_manager._idleState);

    }
    public override void ExitState(StateManager manager)
    {
       // _manager.OnSubComplete.RemoveListener(OnComplete);

        //manager.SwitchState(manager._idleState);
    }

    public override void UpdateState(StateManager manager)
    {
        if(data.SpellType!=Define.SpellType.Chanelling)
        {
            AnimatorStateInfo stateinfo = manager._anim.GetCurrentAnimatorStateInfo(0);
            float normalizedTime = stateinfo.normalizedTime;
            if(normalizedTime>=0.99f)
            {
                OnComplete();
            }
        }

    }

    internal void SetSkillData(SpellData spellData, StateManager manager)
    {
        data = spellData;
        switch (data.SpellType)
        {
            case Define.SpellType.Interact:
            case Define.SpellType.Meele:
                curruntHash = MeeleHash;
                break;
            case Define.SpellType.Arange:
                curruntHash = MagicHash;
                break;
            case Define.SpellType.Chanelling:
                curruntHash = ChannelingHash;
                manager._anim.SetBool("IsChanneling", true);
                break;
        }

        // switch(spellData.)
    }

    public override void Init(StateManager manager)
    {

        _manager = manager;
            // 채널링 종료 이벤트
            Managers.Input.PressdEvent.AddListener(ispress =>
            {
                //manager._anim.SetBool("IsChanneling", ispress);
                //if (ispress == false && manager.StateType == Define.State.Skill)
                //    _manager.SwitchState(_manager._idleState);
                if(manager._anim.GetBool("IsChanneling"))
                {
                    manager._anim.SetBool("IsChanneling", ispress);
                    _manager.SwitchState(_manager._idleState);
                }
            });
        
    }
}

public class IdleState : BaseState
{
    public override void EnterState(StateManager manager)
    {
        manager.StateType = Define.State.Idle;
        if (manager._prevState != this)
            manager._anim.CrossFade("Idle", 0.1f);
        manager._anim.SetBool("Idle", true);
    }

    public override void ExitState(StateManager manager)
    {
        manager._anim.SetBool("Idle", false);
    }

    public override void Init(StateManager manager)
    {
    

    }

    public override void UpdateState(StateManager manager)
    {

    }
}

public class MoveState : BaseState
{
    public override void EnterState(StateManager manager)
    {
        manager.StateType = Define.State.Moving;

        if (manager._prevState != this)
        {
            manager._anim.CrossFade("Running", 0.1f);

            manager._anim.SetBool("Running", true);
        }
    }

    public override void ExitState(StateManager manager)
    {
        manager._anim.SetBool("Running", false);
    }

    public override void Init(StateManager manager)
    {
        
    }

    public override void UpdateState(StateManager manager)
    {
        Vector3 dir = manager._destPos - manager.transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.1f)
        {
            manager.SwitchState(manager._idleState);
        }
        else
        {
            float moveDist = Mathf.Clamp(10 * Time.deltaTime, 0, dir.magnitude);
            manager. transform.position += dir.normalized * moveDist;
            manager.transform.rotation = Quaternion.Slerp(manager.transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }
}