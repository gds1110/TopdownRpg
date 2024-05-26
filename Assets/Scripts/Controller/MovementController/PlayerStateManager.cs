using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerStateManager : StateManager
{


    public UnityAction<SkillHolder> animTimingAction = null;
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    

    // Start is called before the first frame update
    protected override void Start()
    {
        
        base.Start();
        Init();
        Managers.Input.MouseAction -= InputEvt;
        Managers.Input.MouseAction += InputEvt;
   
    }
    public override void Init()
    {
        base.Init();
        _spellManager = GetComponent<SpellManager>();
        _moveState = new MoveState();
        _idleState = new IdleState();
        _skillState = new SkillState();
        _moveState.Init(this);
        _idleState.Init(this);
        _skillState.Init(this); 

    }

    void Update()
    {
        _currentState.UpdateState(this);
       
    }
    public void InputEvt(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);

        //if (raycastHit)
        //{
        //    ActionEvt(evt, hit);
        //    MovementEvt(evt, hit);
        //}

        if(raycastHit)
        {

            switch (evt)
            {
                case Define.MouseEvent.LPress:
                case Define.MouseEvent.LPointerDown:
                    _spell = SpellManager.GetSkillHolder(SkillSlot.Main);
                    SpellManager.SelectSkill = SpellManager.GetSkillHolder(SkillSlot.Main);
                    if (CheckLockTarget())
                    {
                        ActionEvt(evt, hit);
                        break;
                    }
                    MovementEvt(evt, hit);
                    break;
                case Define.MouseEvent.RPointerDown:
                    _spell = SpellManager.GetSkillHolder(SkillSlot.Sub);
                    SpellManager.SelectSkill = SpellManager.GetSkillHolder(SkillSlot.Sub);
                    ActionEvt(evt, hit);
                    RotateCharacter(hit);
                    break;
                case Define.MouseEvent.RPress:
                    if (_anim.GetBool("IsChanneling"))
                    {
                        _spellManager.SlotHitEvent?.Invoke(hit, CurrentSpell);
                        RotateCharacter(hit);
                      
                    }
                    break;
            }
        }

    }
    public void RotateCharacter(RaycastHit hit)
    {
        Vector3 dir = hit.point - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 50 * Time.deltaTime);
    }


    public bool ActionEvt(Define.MouseEvent evt, RaycastHit hitInfo)
    {
        bool retBool = true;

        if (_anim.GetBool("IsChanneling"))
        {
            return false;
        }
        if (CurrentSpell == null) return false;
        SpellData spellData = new SpellData(_spell.HoldingSpell.Type, evt);
        _skillState.SetSkillData(spellData, this);
        SwitchState(_skillState);
        _spellManager.SlotHitEvent?.Invoke(hitInfo,CurrentSpell);
        return retBool;
    }


    public void MovementEvt(Define.MouseEvent evt, RaycastHit hitInfo)
    {

        switch (evt)
        {
            case Define.MouseEvent.LPress:
                _destPos = hitInfo.point;
                SwitchState(_moveState);
                break;
            case Define.MouseEvent.LPointerDown:
                _destPos = hitInfo.point;
                SwitchState(_moveState);
                break;
            case Define.MouseEvent.LPointerUp:
                break;
            case Define.MouseEvent.LClick:
                break;
        }
    }


    bool CheckLockTarget()
    {
        bool ret = false;
        if(LockTarget!=null)
        {
            float distance = (LockTarget.transform.position - transform.position).magnitude;
            if(distance<=_spell.HoldingSpell.Distance)
            {
                ret = true;
            }
        }
        return ret;
    }
    bool CheckMoveOrAction(Define.MouseEvent evt)
    {
        bool ret = false;
        switch (evt)
        {
            case Define.MouseEvent.LPress:
                break;
            case Define.MouseEvent.LPointerDown:
                break;
            case Define.MouseEvent.LPointerUp:
                break;
            case Define.MouseEvent.LClick:
                break;
            case Define.MouseEvent.RPress:

            case Define.MouseEvent.RPointerDown:

            case Define.MouseEvent.RPointerUp:

            case Define.MouseEvent.RClick:
                ret = true;
                break;
        }
        if(_lockTarget)
        {
            float dist = (transform.position - _lockTarget.transform.position).magnitude;
            if(dist<=_spell.HoldingSpell.Distance)
            {
                ret = true;
            }
            else
            {
                ret = false;
                Vector3 dir = (_lockTarget.transform.position - transform.position).normalized;
                float distToMove = dist-_spell.HoldingSpell.Distance;
                Vector3 moveVector = dir * distToMove;
                _destPos = moveVector;
                SwitchState(_moveState);
            }
        }

        return ret;
    }

    public void OnHolder(SkillHolder holder)
    {
        animTimingAction?.Invoke(holder);
    }
}
