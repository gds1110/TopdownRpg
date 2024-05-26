using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BaseSpell",menuName ="SpellSO/CreateSpell",order =int.MaxValue)]
public class BaseSpell : ScriptableObject
{


    [Header("Spell Data")]
    public string spellName;
    public string displayName;


    [SerializeField]
    Define.SpellType type;
    [SerializeField]
    Define.MouseEvent inputType;

    [SerializeField]
    float distance;
    [SerializeField]
    float cooldownTime;
    [SerializeField]
    float activateTime;
    [SerializeField]
    float damge = 1f;
    [SerializeField]
    float castingTime = 0f; 
    [SerializeField]
    int cost = 0;

    public GameObject effect;  //orgin

    public GameObject spawnEffect = null;
    public Vector3 destPos;

    public float Distance => distance;
    public float CooldownTime => cooldownTime;   
    public float ActivateTime => activateTime;   
    public float Damge => damge;    
    public float CastingTime => castingTime;
    public int Cost => cost;
    public Define.SpellType Type => type;
   
    public Define.MouseEvent InputType=>inputType;
    [Header ("Spell UI Data")]

    [SerializeField]
    Texture2D spellTexture;


    public virtual void Activate() { } 
    public virtual void Activate(Transform[] transforms) { }
    public virtual void Activate(RaycastHit hit) { }
    public virtual void Deactivate() { }   
    public virtual void SetHitInfo(RaycastHit hit) { }
    public virtual GameObject GetEffectPrefab() { return effect; }
    public virtual void SpellUpdate(GameObject go,RaycastHit hit) { }
}
