using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClickBehaviour : ScriptableObject
{
    [SerializeField]
    private int priority;
    [SerializeField]
    private bool enableScrolling;
    public bool EnableScrolling => enableScrolling;
    [SerializeField]
    private bool useScreenCoordinates;

    public bool UseScreenCoordinates => useScreenCoordinates;
    public int Priority => priority;

    private System.Action finishAction;

    public abstract bool CheckClick(Vector3 position);
    public abstract void OnClick(Vector3 position);
    public virtual void OnDrag(Vector3 position) { }
    
    public virtual void OnRelease() { }

    protected void ForcedFinish()
    {
        finishAction?.Invoke();
    }

    public void SetFinishAction(System.Action action)
    {
        finishAction = action;
    }
}
