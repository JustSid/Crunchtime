using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTo : ReactsToPlayerTriggerBase
{
    public Transform TargetRotation;
    public float deltaAngle = 360;
    private float alpha;
    private Quaternion startQuaternion;
    private bool lerp;
    protected override void DoTheThing(PlayerTrigger trigger)
    {
        if(!TargetRotation) return;
        startQuaternion = transform.rotation;
        lerp = true;
        alpha = 0;
    }

    void Update()
    {
        if(lerp)
        {
            alpha += Time.deltaTime * deltaAngle;
            transform.rotation = Quaternion.Slerp(startQuaternion, TargetRotation.rotation, alpha);
            if(alpha >= 1) lerp = false;
        }
    }
}
