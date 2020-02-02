using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekeyBoi : MonoBehaviour
{
    public Transform followTarget;
    public float travelSpeed = 100f;
    public float maxDistance = 100f;
    public bool cameratracksTarget = false;
    public bool useLerp = false;

    private Camera cam;

    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (followTarget != null)
        {
            if (cameratracksTarget) cam.transform.LookAt(followTarget);
            transform.position = useLerp ? Vector3.Lerp(transform.position, followTarget.position, travelSpeed * Time.deltaTime) : Vector3.MoveTowards(transform.position, followTarget.position, travelSpeed * Time.deltaTime);
            float dist = Vector3.Distance(followTarget.position, transform.position);
            if (dist > maxDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, followTarget.position, dist - maxDistance);
            }
        }
    }

    public void SnapToTarget()
    {
        transform.position = followTarget.position;
    }
}
