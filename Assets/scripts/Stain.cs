using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stain : MonoBehaviour
{
    [SerializeField]
    private Vector2Int resolution;

    [SerializeField]
    private Vector2 gridDimentions;
    [SerializeField]
    private Vector2 gridOffset;
    [SerializeField]
    private SpriteRenderer renderer;
    [SerializeField]
    private GameObject maskObj;
    

    private HashSet<Vector2> activePoints;

    private int layerOrder;

    public void Initiate(int order)
    {
        activePoints = new HashSet<Vector2>(GetPoints());
        layerOrder = order;
        renderer.sortingOrder = order;
    }

    public void TryWipe(Vector2 position, float radius)
    {
        var removeCandidates = new List<Vector2>();
        foreach (var activePoint in activePoints)
        {
            if (Vector2.Distance(position, activePoint) < radius)
                removeCandidates.Add(activePoint);
        }
        foreach (var candidate in removeCandidates)
        {
            activePoints.Remove(candidate);
        }
        PlaceMask(position, radius);
        if (activePoints.Count == 0)
            RemoveStain();
    }

    private void PlaceMask(Vector2 position, float radius)
    {
        var mask = Instantiate(maskObj, transform);
        mask.transform.position = position;
        var maskComponent = mask.GetComponent<SpriteMask>();
        maskComponent.isCustomRangeActive = true;
        maskComponent.frontSortingLayerID = renderer.sortingLayerID;
        maskComponent.frontSortingOrder = layerOrder;
        maskComponent.backSortingLayerID = renderer.sortingLayerID;
        maskComponent.backSortingOrder = layerOrder-1;
        var scale = maskObj.transform.localScale;
        scale.x *= 2 * radius / transform.lossyScale.x;
        scale.y *= 2 * radius / transform.lossyScale.y;
        mask.transform.localScale = scale;
    }

    private void RemoveStain()
    {
        Destroy(gameObject);
    }

    private IEnumerable<Vector2> GetPoints()
    {
        var step = new Vector2(gridDimentions.x / (resolution.x-1),
            gridDimentions.y / (resolution.y-1));
        var start = gridOffset - gridDimentions / 2;
        for (var x = 0; x < resolution.x; x++)
        {
            for (var y = 0; y < resolution.y; y++)
            {
                var point = start + new Vector2(x * step.x, y * step.y);
                yield return transform.TransformPoint(point);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var p in GetPoints())
        {
            if (Application.isPlaying)
            {
                Gizmos.color = (activePoints.Contains(p) ? Color.yellow : Color.red);
            }
            Gizmos.DrawSphere(p, 0.05f);
        }
    }

    private void OnValidate()
    {
        resolution.x = Mathf.Max(0, resolution.x);
        resolution.y = Mathf.Max(0, resolution.y);
    }
}
