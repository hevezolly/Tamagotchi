using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPid
{
    public float pFactor, iFactor, dFactor;

    private float integral;
    private float lastError;

    public FloatPid(float pFactor, float iFactor, float dFactor)
    {
        this.pFactor = pFactor;
        this.iFactor = iFactor;
        this.dFactor = dFactor;
    }

    public float Update(float currentError, float timeFrame)
    {
        integral += currentError * timeFrame;
        var deriv = (currentError - lastError) / timeFrame;
        lastError = currentError;
        return currentError * pFactor
            + integral * iFactor
            + deriv * dFactor;
    }
}