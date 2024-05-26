using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SkillHolder 
{

    [SerializeField]
    BaseSpell _holdingSpell;

    float cooldownTime;
    float activeTime;

    Transform starterTransform;
    [SerializeField]
    Vector3 destPos;
    [SerializeField]
    GameObject spawnEffect;

    public BaseSpell HoldingSpell { get { return _holdingSpell; } set { _holdingSpell = value; } }

    enum SkillState
    {
        Ready,
        Active,
        CoolDown,
    }
    [SerializeField]
    SkillState _state = SkillState.Ready;
    RaycastHit _hitinfo;
    void SetHitInfo(RaycastHit hit, SkillHolder holder)
    {
        if (holder != this) return;
        if (HoldingSpell == null) return;
        if(_state== SkillState.Ready)
        {
            _hitinfo = hit;
            destPos = hit.point;
            return;
        }

        if (_state == SkillState.Active && HoldingSpell.Type == Define.SpellType.Chanelling)
        {
            _hitinfo = hit;
            destPos = hit.point;
            return;
        }
    }
    public void ChangeSpell(BaseSpell spell)
    {
        HoldingSpell = spell;
        _state = SkillState.Ready;
    }

    public void ActiveSpell()
    {
        if (_holdingSpell != null)
        {

            if (_holdingSpell.effect != null)
            {
                GameObject effect = GameObject.Instantiate(_holdingSpell.effect,starterTransform);
               
                effect.transform.position = starterTransform.position;   //holder에서 위치와 소환된 이펙트 관리?
                spawnEffect = effect;
            }
            _holdingSpell.Activate();
            _state = SkillState.Active;
            activeTime = HoldingSpell.ActivateTime;


        }
    }

    public void DeactivateChanneling()
    {
        if(spawnEffect)
        {
            GameObject.Destroy(spawnEffect);
            spawnEffect = null;
        }
    }

    public void HolderUpdate()
    {
        if (!HoldingSpell) return;

        switch (_state)
        {
            case SkillState.Ready:
                return;
            case SkillState.Active:
                if(activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                    if(HoldingSpell.Type==Define.SpellType.Chanelling)
                    {
                        //HoldingSpell.SetHitInfo()
                        //채널링 이펙트 조절
                       if(spawnEffect!= null)
                        {
                            // ControlChannelingEffect();
                            HoldingSpell.SpellUpdate(spawnEffect, _hitinfo);
                        }
                    }
                }
                else
                {
                    DeactivateChanneling();
                    _state = SkillState.CoolDown;
                    cooldownTime= HoldingSpell.CooldownTime;
                }
                break;
            case SkillState.CoolDown:
                if(cooldownTime>0)
                {
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    _state = SkillState.Ready;
                }
                break;
        }
    }

    public void ControlChannelingEffect()
    {

        Vector3 dir = destPos - spawnEffect.transform.position;

        if (dir.magnitude < 0.1f)
        {

        }
        else
        {
            float moveDist = Mathf.Clamp(5 * Time.deltaTime, 0, dir.magnitude);
            spawnEffect.transform.position += dir.normalized * moveDist;
            spawnEffect.transform.rotation = Quaternion.Slerp(spawnEffect.transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
    }

    public void SetHolderEvent(UnityEvent UpdateEvent,UnityEvent<RaycastHit, SkillHolder> hitInfoEvent,Transform starter)
    {
        UpdateEvent.AddListener(HolderUpdate);
        hitInfoEvent.AddListener(SetHitInfo);
        starterTransform = starter;
    }
}
