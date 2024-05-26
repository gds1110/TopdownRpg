using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DragonBreath ", menuName = "SpellSO/ChannelingSpell/DragonBreath", order = int.MaxValue)]
public class SpellDragonBreath : BaseSpell
{
    public override void SpellUpdate(GameObject go, RaycastHit hit)
    {
        if (go == null) return;
        

        Vector3 dest = hit.point;
        Vector3 dir = hit.point - go.transform.position;
        dir.y = 0;
        go.transform.rotation = Quaternion.Slerp(go.transform.rotation, Quaternion.LookRotation(dir), 50 * Time.deltaTime);
        //Vector3 localsize = go.transform.localScale;
        //localsize.z = Mathf.Clamp(dir.magnitude, 0f, Distance);
        //go.transform.localScale = localsize;
    }

}
