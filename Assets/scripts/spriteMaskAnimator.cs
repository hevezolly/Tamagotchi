using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteMaskAnimator : MonoBehaviour
{
    [SerializeField]
    private SpriteMask mask;
    public Sprite sprite;

    private void Awake()
    {
        sprite = mask.sprite;
    }

    private void Update()
    {
        if (mask.sprite != sprite)
        {
            mask.sprite = sprite;
        }
    }
}
