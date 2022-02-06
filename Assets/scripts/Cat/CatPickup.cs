using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPickup : MonoBehaviour, IPickuppuble
{
    [SerializeField]
    private CatBehaviour PickUpBehaviourObj;
    private CatBehaviour pickUp;

    [SerializeField]
    private CatBehaviourRegistrator registrator;
    [SerializeField]
    private CatBehaviourPicker picker;

    private void Awake()
    {
        pickUp = Instantiate(PickUpBehaviourObj);
        registrator.RegisterBehaviour(pickUp);
    }

    public void OnDrop()
    {
        picker.ReleaseBehaviour(pickUp);
    }

    public void OnPickUp()
    {
        picker.PushBehaviour(pickUp);
    }
}
