using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : MonoBehaviour
{
    [SerializeField]
    private FoodType type;
    public FoodType Type => type;
    public bool CanBeEaten { get; protected set; }
    public abstract float Bite(float deltaTime);
    public abstract Vector2 GetClosestPosition(Vector2 target);
}

public enum FoodType
{
    dry,
    bottled
}
