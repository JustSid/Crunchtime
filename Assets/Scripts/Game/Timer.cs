using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float time = 1f;

    private float elapsed = 0;

    [SerializeField]
    private WirePowerAction action;

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= time)
        {
            action.OnPowerEnabledInternal();
            action.OnPowerDisabledInternal();
            elapsed %= time;
        }
        float amount = elapsed / time;
        transform.rotation = Quaternion.AngleAxis(amount * 360, Vector3.back);
    }
}
