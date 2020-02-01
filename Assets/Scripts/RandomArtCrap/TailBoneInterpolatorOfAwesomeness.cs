using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailBoneInterpolatorOfAwesomeness : MonoBehaviour
{
    public Transform followTarget;
    public float lerpAlpha = 1f;

    Quaternion currentRotation;

    void Awake()
    {
        if(!followTarget) followTarget = transform.parent;
        currentRotation = transform.rotation;
    }

    void Update()
    {
        Quaternion.Slerp(currentRotation, followTarget.rotation, lerpAlpha * Time.deltaTime);
        currentRotation = followTarget.rotation;
    }

}
