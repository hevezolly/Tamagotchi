using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedStainApplyer : MonoBehaviour
{
    public void ApplyStain(float time, GameObject Stain, StainableSurface surface, float position)
    {
        StartCoroutine(WaitAndApply(time, Stain, surface, position));
    }

    private IEnumerator WaitAndApply(float time, GameObject Stain, StainableSurface surface, float position)
    {
        yield return new WaitForSeconds(time);
        surface.ApplyStain(Stain, position);
        Destroy(gameObject);
    }
}
