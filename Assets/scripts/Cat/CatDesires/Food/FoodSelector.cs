using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSelector : MonoBehaviour
{
    private HashSet<Food> trackedFood = new HashSet<Food>();
    private Food currentFood;
    public Food CurrentFood => currentFood;
    private LinkedList<Food> possibleFood = new LinkedList<Food>();

    public event System.Action<Food> FoodChangedEvent;
    private void OnFoodAdded(Food food)
    {
        if (trackedFood.Contains(food))
            return;
        trackedFood.Add(food);
        if (currentFood == null)
        {
            SelectFood(food);
        }
        else
        {
            possibleFood.AddFirst(food);
        }
    }

    public void OnFoodFinished(Food food)
    {
        OnFoodRemoved(food);
    }

    private void SelectFood(Food food)
    {
        currentFood = food;
        FoodChangedEvent?.Invoke(currentFood);
    }

    private void OnFoodRemoved(Food food)
    {
        trackedFood.Remove(food);
        if (food == currentFood)
        {
            if (possibleFood.Count > 0)
            {
                var newFood = possibleFood.Last.Value;
                possibleFood.RemoveLast();
                SelectFood(newFood);
            }
            else
            {
                SelectFood(null);
            }
            
        }
        else
        {
            possibleFood.Remove(food);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Food>(out var food))
        {
            if (food.CanBeEaten)
                OnFoodAdded(food);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Food>(out var food))
        {
            if (trackedFood.Contains(food))
            {
                OnFoodRemoved(food);
            }
        }
    }
}
