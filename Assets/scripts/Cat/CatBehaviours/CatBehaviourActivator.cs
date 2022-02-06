using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehaviourActivator : MonoBehaviour
{
    private CatBehaviour currentBehaviour;

    public void SetBehaviour(CatBehaviour behaviour)
    {
        if (currentBehaviour == behaviour)
            return;
        if (currentBehaviour != null)
            currentBehaviour.Disactivate();
        if (behaviour != null)
            behaviour.Activate();
        currentBehaviour = behaviour;
    }

    public void RemoveBehaviour()
    {
        SetBehaviour(null);
    }

    private void Update()
    {
        if (currentBehaviour != null)
            currentBehaviour.Update();
    }

    private void LateUpdate()
    {
        if (currentBehaviour != null)
            currentBehaviour.LateUpdate();
    }

    private void FixedUpdate()
    {
        if (currentBehaviour != null)
            currentBehaviour.FixedUpdate();
    }
}
