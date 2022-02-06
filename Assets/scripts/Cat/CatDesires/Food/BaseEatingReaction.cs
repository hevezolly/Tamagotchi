using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEatingReaction : ValueReaction<float>
{
    protected Value<float> value;
    protected bool isTracking = false;

    [SerializeField]
    protected CatBehaviourRegistrator registrator;
    [SerializeField]
    protected CatBehaviourPicker picker;
    [SerializeField]
    protected FoodSelector foodSelector;

    protected Food currentFood;

    [SerializeField]
    private List<EatingProcess> EatingProcessBehaviours;
    protected Dictionary<FoodType, ParametrisedBehaviour<Food, FoodBus>> eatBehaviours;

    protected virtual void Awake()
    {
        Initiate();
        
    }

    protected virtual void OnDestroy()
    {
        if (isTracking)
            foodSelector.FoodChangedEvent -= OnFoodChanged;
    }
    protected virtual void Initiate()
    {
        eatBehaviours =
        new Dictionary<FoodType, ParametrisedBehaviour<Food, FoodBus>>();
        foreach (var p in EatingProcessBehaviours)
        {
            eatBehaviours.Add(p.type, Instantiate(p.behaviour));
            registrator.RegisterBehaviour(eatBehaviours[p.type]);
        }
    }
    public override void OnTrackingEnded()
    {
        isTracking = false;
        foodSelector.FoodChangedEvent -= OnFoodChanged;
        if (currentFood != null)
        {
            ReleaseFoodBehaviour();
        }
    }

    public override void OnTrackingStart(Value<float> value)
    {
        foodSelector.FoodChangedEvent += OnFoodChanged;
        if (foodSelector.CurrentFood != null)
            PushFoodBehaviour(foodSelector.CurrentFood);
        this.value = value;
        isTracking = true;
    }

    public override void OnValueChanged()
    { 
    }

    private void ReleaseFoodBehaviour()
    {
        var eatBehaviour = eatBehaviours[currentFood.Type];
        picker.ReleaseBehaviour(eatBehaviour);
        currentFood = null;
    }

    private void PushFoodBehaviour(Food food)
    {
        currentFood = food;
        var eatBehaviour = eatBehaviours[currentFood.Type];
        eatBehaviour.SetParam(food,
            new FoodBus((f) => value.SetValue(value.MainValue + f),
            () => foodSelector.OnFoodFinished(currentFood)));
        picker.PushBehaviour(eatBehaviour);
    }

    protected virtual void OnFoodChanged(Food food)
    {
        if (currentFood != null)
        {
            ReleaseFoodBehaviour();
        }
        if (food != null)
        {
            PushFoodBehaviour(food);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Initiate();
        }
        foreach (var ep in EatingProcessBehaviours)
        {
            if (ep.behaviour != null)
            {
                ep.behaviour.OnDrawGizmos();
            }
        }
    }
}

[System.Serializable]
public class EatingProcess
{
    public FoodType type;
    public ParametrisedBehaviour<Food, FoodBus> behaviour;
}

