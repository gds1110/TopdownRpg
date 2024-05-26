using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum SkillSlot
{
    Main,
    Sub,
    One,
    Two,
    Three,
    Four,
    Five,
}

public class SpellManager : MonoBehaviour
{
    #region SlotsRegion
    [SerializeField]
    SkillHolder _mainHolder = new SkillHolder();
    [SerializeField]
    SkillHolder _subHolder = new SkillHolder();
    [SerializeField]
    SkillHolder slotOne = new SkillHolder();
    [SerializeField]
    SkillHolder slotTwo = new SkillHolder();
    [SerializeField]
    SkillHolder slotThree = new SkillHolder();
    [SerializeField]
    SkillHolder slotFour = new SkillHolder();
    [SerializeField]
    SkillHolder slotFive = new SkillHolder();
    
    public SkillHolder MainSpell => _mainHolder;
    public SkillHolder SubSpell => _subHolder;
    public SkillHolder SlotOne => slotOne;
    public SkillHolder SlotTwo => slotTwo;
    public SkillHolder SlotThree => slotThree;
    public SkillHolder SlotFour => slotFour;
    public SkillHolder SlotFive => slotFive;
    #endregion

    public UnityEvent _slotUpdateEvent = null;
    public UnityEvent<RaycastHit,SkillHolder> _slotHitInfoEvent = null;
    public SkillHolder SelectSkill;

    public UnityEvent<RaycastHit, SkillHolder> SlotHitEvent => _slotHitInfoEvent;
    [SerializeField]
    Transform spellStarter;
    public Transform SpellStarter => spellStarter;

    private void Start()
    {
        MainSpell.SetHolderEvent(_slotUpdateEvent, SlotHitEvent, SpellStarter);
        SubSpell.SetHolderEvent(_slotUpdateEvent, SlotHitEvent, SpellStarter);
        SlotOne.SetHolderEvent(_slotUpdateEvent, SlotHitEvent, SpellStarter);
        SlotTwo.SetHolderEvent(_slotUpdateEvent, SlotHitEvent, SpellStarter);
        SlotThree.SetHolderEvent(_slotUpdateEvent, SlotHitEvent, SpellStarter);
        SlotFour.SetHolderEvent(_slotUpdateEvent, SlotHitEvent, SpellStarter);
        SlotFive.SetHolderEvent(_slotUpdateEvent, SlotHitEvent, SpellStarter);


    }

    private void Update()
    {
        
        _slotUpdateEvent?.Invoke();

    }

    //애니메이션에 연결되어있음.
    public void OnActiveHolder(int slotNum)
    {
        SelectSkill.ActiveSpell();   
    }


    public void SetSkillInSlot(SkillSlot slot,BaseSpell spell)
    {
        switch (slot)
        {
            case SkillSlot.Main:
                MainSpell.HoldingSpell = spell;
                break;
            case SkillSlot.Sub:
                SubSpell.HoldingSpell = spell;
                break;
            case SkillSlot.One:
                slotOne.HoldingSpell = spell;
                break;
            case SkillSlot.Two:
                slotTwo.HoldingSpell = spell;   
                break;
            case SkillSlot.Three:
                slotThree.HoldingSpell = spell; 
                break;
            case SkillSlot.Four:
                SlotFour.HoldingSpell = spell;  
                break;
            case SkillSlot.Five:
                slotFive.HoldingSpell= spell;
                break;
        }
    }

    public SkillHolder GetSkillHolder(SkillSlot slot)
    {
        switch (slot)
        {
            case SkillSlot.Main:
                return MainSpell;
            case SkillSlot.Sub:
                return SubSpell;
            case SkillSlot.One:
                return slotOne;
            case SkillSlot.Two:
                return slotTwo;
            case SkillSlot.Three:
                return slotThree;
            case SkillSlot.Four:
                return slotFour;
            case SkillSlot.Five:
                return slotFive;
        }

        return MainSpell;
    }


}
