using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishableFood : Food
{
    [SerializeField]
    private float maxFood;
    [SerializeField]
    private float eatingSpeed;
    private float currentFood;

    [SerializeField]
    private Vector2 foodCenterOffset;

    public event System.Action<float> BiteEvent;

    private void Awake()
    {
        currentFood = maxFood;
        CanBeEaten = true;
    }

    public override float Bite(float deltaTime)
    {
        var delta = eatingSpeed * deltaTime;
        var realDelta = Mathf.Min(delta, currentFood);
        currentFood -= realDelta;
        if (currentFood <= 0)
            CanBeEaten = false;
        BiteEvent?.Invoke(currentFood / maxFood);
        return realDelta;
    }

    public override Vector2 GetClosestPosition(Vector2 target)
    {
        return transform.position + transform.TransformVector(foodCenterOffset);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GetClosestPosition(transform.position), 0.1f);
    }
}
