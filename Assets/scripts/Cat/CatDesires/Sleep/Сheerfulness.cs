using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Сheerfulness : FloatValue
{
    [SerializeField]
    private float valueDecreaseDelay;

    private bool needToUpdateValue = true;
    private Coroutine delay;

    public override void SetValue(float newValue)
    {
        if (newValue > mainValue)
        {
            needToUpdateValue = false;
            if (delay != null)
            {
                StopCoroutine(delay);
            }
            delay = StartCoroutine(StartDelayCountdown());
        }
        base.SetValue(newValue);
    }

    private IEnumerator StartDelayCountdown()
    {
        yield return new WaitForSeconds(valueDecreaseDelay);
        needToUpdateValue = true;
        delay = null;
    }

    protected override void Update()
    {
        if (needToUpdateValue)
            base.Update();
    }
}
