using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tornado ", menuName = "SpellSO/ChannelingSpell/Tornado", order = int.MaxValue)]

public class SpellChannelingTornado : BaseSpell
{

    public override void Activate()
    {
        base.Activate();
        
    }

    public override void SetHitInfo(RaycastHit hit)
    {

        destPos = hit.point;
    }

    public override void SpellUpdate(GameObject go, RaycastHit hit)
    {
        if (go == null) return;
        if (go.transform.parent != null) go.transform.parent = null;
        Vector3 dest = hit.point;
        Vector3 dir = dest - go.transform.position;

        if (dir.magnitude < 0.1f)
        {
          
        }
        else
        {
            float moveDist = Mathf.Clamp(3 * Time.deltaTime, 0, dir.magnitude);
            go.transform.position += dir.normalized * moveDist;
            go.transform.rotation = Quaternion.Slerp(go.transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

    }


}
