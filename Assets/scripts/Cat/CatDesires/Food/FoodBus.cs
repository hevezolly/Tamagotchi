using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FoodBus
{
    private Action<float> eatFood;
    private Action foodFinished;
    public FoodBus(Action<float> eatFood, Action foodFinished)
    {
        this.eatFood = eatFood;
        this.foodFinished = foodFinished;
    }

    public void OnEatFood(float amount) => eatFood(amount);
    public void OnFoodFinished() => foodFinished();
}
