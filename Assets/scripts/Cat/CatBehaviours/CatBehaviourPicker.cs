using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehaviourPicker : MonoBehaviour
{
    [SerializeField]
    private CatBehaviourActivator activator;
    [SerializeField]
    private CatBehaviour idle;
    private LinkedList<CatBehaviour> behavioursPriority = new LinkedList<CatBehaviour>();
    private HashSet<CatBehaviour> trackedBehaviours = new HashSet<CatBehaviour>();

    private void Awake()
    {
    }

    public void PushBehaviour(CatBehaviour behaviour)
    {
        if (trackedBehaviours.Contains(behaviour))
            return;
        if (behavioursPriority.Count == 0)
        {
            AddWithHighestPriority(behaviour);
            return;
        }
        LinkedListNode<CatBehaviour> previusNode = null;
        var currentNode = behavioursPriority.First;
        while (true)
        {
            if (currentNode == null || currentNode.Value.Priority < behaviour.Priority)
                break;
            previusNode = currentNode;
            currentNode = currentNode.Next;
        }

        if (previusNode == null)
        {
            AddWithHighestPriority(behaviour);
        }
        else
        {
            trackedBehaviours.Add(behaviour);
            behavioursPriority.AddAfter(previusNode, behaviour);
        }
    }

    private void AddWithHighestPriority(CatBehaviour behaviour)
    {
        behavioursPriority.AddFirst(behaviour);
        trackedBehaviours.Add(behaviour);
        activator.SetBehaviour(behaviour);
    }

    private void FinishCurrentBehaviour()
    {
        var behaviour = behavioursPriority.First.Value;
        trackedBehaviours.Remove(behaviour);
        behavioursPriority.RemoveFirst();
        if (behavioursPriority.Count != 0)
        {
            activator.SetBehaviour(behavioursPriority.First.Value);
        }
        else
        {
            activator.RemoveBehaviour();
        }
    }

    public void ReleaseBehaviour(CatBehaviour behaviour)
    {
        if (!trackedBehaviours.Contains(behaviour))
            return;
        if (behavioursPriority.First.Value == behaviour)
        {
            FinishCurrentBehaviour();
            return;
        }
        var current = behavioursPriority.First;
        while (current.Next != null)
        {
            current = current.Next;
            if (current.Value == behaviour)
                break;
        }
        behavioursPriority.Remove(current);
        trackedBehaviours.Remove(behaviour);
    }

    private void OnDestroy()
    {
        if (behavioursPriority.Count != 0)
            behavioursPriority.First.Value.Disactivate();
    }
}
