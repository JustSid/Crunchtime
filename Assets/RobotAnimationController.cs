using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimationController : MonoBehaviour
{
    public Transform headTransform;
    public Transform bodyTransform;
    public Transform armTransform_L;
    public Transform armTransform_R;

    private float alpha = 0f;
    private float alphaB = 0f;

    private Vector3 headStart;
    private Vector3 bodyStart;
    private Vector3 armStart_L;
    private Vector3 armStart_R;

    void Start()
    {
        headStart = headTransform.localPosition;
        bodyStart = bodyTransform.localPosition;
        armStart_L = armTransform_L.localPosition;
        armStart_R = armTransform_R.localPosition;
    }

    void Update()
    {
        alpha += Time.deltaTime * 2;
        alphaB += Time.deltaTime * 2;
        headTransform.localPosition = headStart +  Vector3.up * Mathf.Sin(alpha) * -0.000699566f;
        bodyTransform.localPosition = -(bodyStart +  Vector3.forward * Mathf.Sin(alpha) * -0.00199566f);
        armTransform_L.localPosition = armStart_L +  Vector3.up * Mathf.Sin(alphaB) * -0.00071159f;
        armTransform_R.localPosition = armStart_R +  Vector3.up * Mathf.Sin(alphaB) * -0.00071159f;
    }
}
