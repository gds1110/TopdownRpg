using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityAnimationEvent : UnityEvent<string> { };

[RequireComponent(typeof(Animator))]
public class StateManager : MonoBehaviour
{
    public UnityAnimationEvent OnStart;
    public UnityAnimationEvent OnComplete;
    public UnityEvent OnSubComplete;

    [SerializeField]
    public Define.State StateType;

    protected SpellManager _spellManager;
    protected SkillHolder _spell;

    public BaseState _prevState;
    [SerializeField]
    protected BaseState _currentState;


    public Animator _anim;

    public float _moveSpeed;
    public Vector3 _destPos = Vector3.zero;

    public MoveState _moveState  ;
    public IdleState _idleState  ;
    public SkillState _skillState ;
                                  
    protected GameObject _lockTarget;

    public BaseState CurrentState => _currentState;
    public SpellManager SpellManager => _spellManager;
    public GameObject LockTarget => _lockTarget;
    public SkillHolder CurrentSpell => _spell;

    public virtual void Init()
    {
    }

    protected virtual void Start()
    {
        Init();
        _anim = GetComponent<Animator>();  

        //for (int i = 0; i < _anim.runtimeAnimatorController.animationClips.Length; i++)
        //{
        //    AnimationClip clip = _anim.runtimeAnimatorController.animationClips[i];
            

        //    AnimationEvent startEvt = new AnimationEvent();
        //    AnimationEvent completeEvt = new AnimationEvent();
        //    startEvt.time = 0;
        //    startEvt.functionName = "OnStartHandle";
        //    startEvt.stringParameter = clip.name;

        //    completeEvt.time = clip.length;
        //    completeEvt.functionName = "OnCompleteHandle";
        //    completeEvt.stringParameter = clip.name;

        //    clip.AddEvent(startEvt);
        //    clip.AddEvent(completeEvt);

        //}
        SwitchState(_idleState);

    }

    private void Update()
    {
    }

    public virtual void SwitchState(BaseState state)
    { 
    //{
    //    if (state == CurrentState) return;

        if(CurrentState!=null)
        {
            _prevState = _currentState;
            _currentState.ExitState(this);
        }
        _currentState = state;
        _currentState.EnterState(this);
    }
   
    public void IUpdate()
    {
       
    }

    public void OnStartHandle(string name)
    {
        Debug.Log($"StartAnimation {name}");
        OnStart?.Invoke(name);
    }
    public void OnCompleteHandle(string name)
    {
        Debug.Log($"EndAnimation {name}");
        OnComplete?.Invoke(name);
        OnSubComplete?.Invoke();
    }
}
