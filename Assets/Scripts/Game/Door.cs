using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : WirePowerAction
{
    public Transform openedTarget;
    public Transform closedTarget;

    private Transform transformTarget;

    [SerializeField]
    private bool oneShot = false;

    private Vector3 rotation = new Vector3(0, 0, 0);
    private Vector3 startRotation = new Vector3(0, 0, 0);
    private Quaternion startQuat;
    private float animationProgress = 1.0f;

    void Update()
    {
        if(animationProgress < 1.0f)
        {
            animationProgress = Mathf.Min(animationProgress + Time.deltaTime * 5, 1.0f);
            if(transformTarget) transform.rotation = Quaternion.Slerp(startQuat, transformTarget.rotation, animationProgress);
            else transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, rotation, animationProgress));
        }
    }

    protected override void OnPowerEnabled()
    {
        base.OnPowerEnabled();
        OpenDoor();
    }

    protected override void OnPowerDisabled()
    {
        base.OnPowerDisabled();

        if(!oneShot)
            CloseDoor();
    }

    private void OpenDoor()
    {
        transformTarget = openedTarget;
        AnimateToRotation(new Vector3(0, 90, 0));

        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        if(collider)
            collider.isTrigger = true;
    }

    private void CloseDoor()
    {
        transformTarget = closedTarget;
        AnimateToRotation(new Vector3(0, 0, 0));

        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        if(collider)
            collider.isTrigger = false;
    }

    private void AnimateToRotation(Vector3 newRotation)
    {
        startQuat = transform.rotation;
        startRotation = transform.rotation.eulerAngles;
        rotation = newRotation;
        animationProgress = 0.0f;
    }
}
