using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFood : Food
{
    [SerializeField]
    private float hungerPerSecond;

    private void Awake()
    {
        CanBeEaten = true;
    }
    public override float Bite(float deltaTime)
    {
        return hungerPerSecond * deltaTime;
    }

    public override Vector2 GetClosestPosition(Vector2 target)
    {
        return transform.position;
    }
}
