using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "standartEating", menuName = "Cat Behaviours/standart eating")]
public class NormalFoodEatingBehaviour : ParametrisedBehaviour<Food, FoodBus>
{
    private Food food;
    private FoodBus updateHunger;
    [SerializeField]
    private MouthAnimationReference mouthEatAnim;
    [SerializeField]
    private EyesAnimationReference EyesAnim;

    private int eyesHash;
    private int mouthHash;
    public override void SetParam(Food param1, FoodBus param2)
    {
        food = param1;
        updateHunger = param2;
    }

    public override void Activate()
    {
        eyeAnimator.Play(EyesAnim);
        mouthAnimator.ForcePlay(mouthEatAnim);
    }

    public override void Update()
    {
        var pos = food.GetClosestPosition(mainObjectTransform.position);
        eyeDirection.SetLookTargetPosition(pos);
        FaceMove.SetTarget(pos);
        var foodAmount = food.Bite(Time.deltaTime);
        updateHunger.OnEatFood(foodAmount);
        if (!food.CanBeEaten)
        {
            updateHunger.OnFoodFinished();
        }
    }
}
