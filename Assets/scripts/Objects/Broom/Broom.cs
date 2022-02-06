using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Broom : MonoBehaviour, IPickuppuble
{
    [SerializeField]
    private float CleanRadius;
    [SerializeField]
    private Vector2 CleanSurfaceOffset;
    [SerializeField]
    private float checkDelay;
    [SerializeField]
    private LayerMask stainsLayer;

    private Coroutine checkLoop;

    public event System.Action PickUpEvent;
    public event System.Action DropEvent;

    private Vector2 lastPos;

    public void OnPickUp()
    {
        PickUpEvent?.Invoke();
        checkLoop = StartCoroutine(CheckLoop());
    }

    public void OnDrop()
    {
        DropEvent?.Invoke();
        StopCoroutine(checkLoop);
    }

    private void CheckStains(Vector2 point)
    {
        
        var stains = Physics2D.OverlapCircleAll(point, CleanRadius, stainsLayer)
            .Select(s => s.gameObject.GetComponent<Stain>())
            .Where(s => s != null);
        foreach (var s in stains)
        {
            s.TryWipe(point, CleanRadius);
        }
    }

    private IEnumerator CheckLoop()
    {
        lastPos = transform.TransformPoint(CleanSurfaceOffset);
        while (true)
        {
            var point = transform.TransformPoint(CleanSurfaceOffset);
            if (Vector2.Distance(point, lastPos) > 0.0001)
                CheckStains(point);
            lastPos = point;
            yield return new WaitForSeconds(checkDelay);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.TransformPoint(CleanSurfaceOffset), CleanRadius);
    }


}
