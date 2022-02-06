using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoFoodReaction : BaseEatingReaction
{
    [Header("Hunger")]
    [SerializeField]
    private CatBehaviour NoFoodBehaviourObj;
    private CatBehaviour noFoodBehaviour;
    [SerializeField]
    [Min(0)]
    private float angryHungerBuff;
    [SerializeField]
    private CatBehaviour FoodSearchBehaviourObj;
    private CatBehaviour foodSearchBehaviour;

    private float maxValueToEmote;
    [SerializeField]
    private float maxValueToSearch;

    [Header("visuals")]
    [SerializeField]
    private PuffBallEmotions emotions;
    [SerializeField]
    private PuffBallDesireIndicator desireIndicator;
    [SerializeField]
    private Sprite bigHungerIcon;
    [SerializeField]
    private Sprite smallHungerIcon;
    [SerializeField]
    [Range(0, 1)]
    private float minAlpha;
    private IconReference hungerIcon;

    private bool isSearching;

    private HashSet<CatBehaviour> activeBehaviours = new HashSet<CatBehaviour>();

    protected override void Initiate()
    {
        base.Initiate();
        if (NoFoodBehaviourObj != null)
        {
            noFoodBehaviour = Instantiate(NoFoodBehaviourObj);
            registrator.RegisterBehaviour(noFoodBehaviour);
        }
        if (FoodSearchBehaviourObj != null)
        {
            foodSearchBehaviour = Instantiate(FoodSearchBehaviourObj);
            registrator.RegisterBehaviour(foodSearchBehaviour);
        }
    }


    public override void OnValueChanged()
    {
        if (value.MainValue < maxValueToSearch &&
            !isSearching)
            EnableSearch();
        else if (value.MainValue > maxValueToSearch &&
            isSearching)
            DisableSearching();
        var alpha = Mathf.Lerp(minAlpha, 1,
            Mathf.Abs(value.MainValue - maxValueToEmote) /
            Mathf.Abs(value.MinValue - maxValueToEmote));
        hungerIcon.SetAlpha(alpha);
        if (value.MainValue == value.MinValue)
        {
            value.SetValue(value.MainValue + angryHungerBuff);
            if (noFoodBehaviour != null)
                picker.PushBehaviour(noFoodBehaviour);
        }
    }

    private void EnableEmoting()
    {
        emotions.RequestEmotion(new EmotionRequest() { sprite = bigHungerIcon });
        hungerIcon = desireIndicator.AddDesire(smallHungerIcon, minAlpha);
    }

    private void DisableEmoting()
    {
        hungerIcon.RemoveImage();
        hungerIcon = null;
    }

    private void EnableSearch()
    {
        if (foodSearchBehaviour != null)
        {
            picker.PushBehaviour(foodSearchBehaviour);
            activeBehaviours.Add(foodSearchBehaviour);
        }
        
        isSearching = true;
    }

    private void DisableSearching()
    {
        if (foodSearchBehaviour != null)
        {
            picker.ReleaseBehaviour(foodSearchBehaviour);
            activeBehaviours.Remove(foodSearchBehaviour);
        }
        isSearching = false;
    }

    protected override void OnFoodChanged(Food food)
    {
        if (currentFood != null) 
        {
            var eatBehaviour = eatBehaviours[currentFood.Type];
            activeBehaviours.Remove(eatBehaviour);
        }
        base.OnFoodChanged(food);
        if (food != null)
        {
            var eatBehaviour = eatBehaviours[currentFood.Type];
            activeBehaviours.Add(eatBehaviour);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (FoodSearchBehaviourObj != null)
            foodSearchBehaviour.OnDrawGizmos();
        if (NoFoodBehaviourObj != null)
            foodSearchBehaviour.OnDrawGizmos();
    }

    public override void OnTrackingStart(Value<float> value)
    {
        maxValueToEmote = value.MainValue;
        base.OnTrackingStart(value);
        EnableEmoting();
    }

    public override void OnTrackingEnded()
    {
        base.OnTrackingEnded();
        foreach (var b in activeBehaviours)
        {
            picker.ReleaseBehaviour(b);
        }
        activeBehaviours.Clear();
        DisableEmoting();
    }    
}
