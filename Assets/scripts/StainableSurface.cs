using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StainableSurface : MonoBehaviour
{
    private const int MaxLayerOrder = 32767;
    private const int MinLayerOrder = -32768;
    [SerializeField]
    private Vector2 areaCenterOffset;
    [SerializeField]
    private Vector2 areaSize;
    [SerializeField]
    private Vector2Int orderRange;
    public GameObject stain;
    private Vector2Int _previusOrderRange;
    private int currentOrder;

    private void Awake()
    {
        currentOrder = orderRange.x;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + (Vector3)areaCenterOffset, areaSize);
        
    }

    private void UpdateOrder()
    {
        currentOrder++;
        if (currentOrder > orderRange.y)
            currentOrder = orderRange.x;
    }

    public GameObject ApplyStain(GameObject stainObject, float xPos)
    {
        var renderer = stainObject.GetComponent<SpriteRenderer>();
        var height = Mathf.Max(renderer.sprite.rect.height / 
            stainObject.transform.localScale.y / renderer.sprite.pixelsPerUnit, areaSize.y);
        var scale = areaSize.y / height;
        var pos = transform.position + (Vector3)areaCenterOffset;
        pos.x = xPos;
        var inst = Instantiate(stainObject, pos, Quaternion.identity);
        inst.transform.localScale = new Vector3(
            inst.transform.localScale.x,
            scale,
            inst.transform.localScale.z);
        inst.GetComponent<Stain>().Initiate(currentOrder);
        UpdateOrder();
        return inst;
    }

    private void Update()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.S))
        {
            ApplyStain(stain, pos.x);
        }
    }

    private void OnValidate()
    {
        if (orderRange.y != _previusOrderRange.y)
        {
            orderRange.y = Mathf.Clamp(orderRange.y, orderRange.x, MaxLayerOrder); 
        }
        else if (orderRange.x != _previusOrderRange.x)
        {
            orderRange.x = Mathf.Clamp(orderRange.x, MinLayerOrder, orderRange.y);
        }
        _previusOrderRange = orderRange;
    }
}
