using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float time = 1f;

    [SerializeField]
    private float startOffset = 0;

    private float elapsed = 0;

    private void Awake()
    {
        elapsed = startOffset;
    }

    public List<WirePowerAction> actions = new List<WirePowerAction>();
    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= time)
        {
            foreach (WirePowerAction action in actions)
            {
                action.OnPowerEnabledInternal();
                action.OnPowerDisabledInternal();
            }
            elapsed %= time;
        }
        float amount = elapsed / time;
        transform.rotation = Quaternion.AngleAxis(amount * 360, Vector3.back);
    }
}
