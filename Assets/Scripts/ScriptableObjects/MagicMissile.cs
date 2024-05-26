using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicMissile", menuName = "SpellSO/MagicMissile", order = int.MaxValue)]

public class MagicMissile : BaseSpell
{

    Transform owner;

    GameObject missile;

    public override void Activate()
    {
        base.Activate();

      //  GameObject go = Instantiate(missile);
        

    }

}
