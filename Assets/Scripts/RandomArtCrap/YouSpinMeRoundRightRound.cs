using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouSpinMeRoundRightRound : MonoBehaviour
{
    public float rate = 10;
    public Vector3 localAxis;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(localAxis * rate * Time.deltaTime, Space.Self);
    }
}
