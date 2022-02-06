using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagedFoodVisual : MonoBehaviour
{
    [SerializeField]
    private FinishableFood food;
    [SerializeField]
    private SpriteRenderer renderer;

    [System.Serializable]
    private class SpriteData
    {
        public Sprite sprite;
        public Vector2 threshold;
    }
    [SerializeField]
    private List<SpriteData> sprites;

    private void Awake()
    {
        food.BiteEvent += SetSprite;
    }

    private void SetSprite(float value)
    {
        foreach (var s in sprites)
        {
            if (s.threshold.x <= value && s.threshold.y > value)
            {
                renderer.sprite = s.sprite;
                return;
            }
        }
    }

    private void OnDestroy()
    {
        food.BiteEvent -= SetSprite;
    }
}
