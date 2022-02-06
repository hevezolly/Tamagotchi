using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : FloatValue
{
    [SerializeField]
    private float overeatingHungerRate;
    [SerializeField]
    private float fastHungerThreshold;
    private float standartHungerRate;
    private bool isFastRate = false;

    private bool isEating = false;
    public override void SetValue(float newValue)
    {
        var delta = newValue - mainValue;
        if (delta > 0)
            isEating = true;
        base.SetValue(newValue);
    }

    protected override void Awake()
    {
        base.Awake();
        standartHungerRate = valueChangeRate;
    }

    protected override void Update()
    {
        if (!isEating)
            base.Update();
        isEating = false;
        if (mainValue > fastHungerThreshold && !isFastRate)
        {
            valueChangeRate = overeatingHungerRate;
            isFastRate = true;
        }
        else if (mainValue <= fastHungerThreshold && isFastRate)
        {
            valueChangeRate = standartHungerRate;
            isFastRate = false;
        }
    }
}
