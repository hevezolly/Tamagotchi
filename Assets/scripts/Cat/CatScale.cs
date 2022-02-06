using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatScale : MonoBehaviour
{
    [SerializeField]
    private PuffBallCreator creator;

    private Dictionary<BodyPart, float> verticalStrength = new Dictionary<BodyPart, float>();
    private Dictionary<BodyPart, float> horizontalStrength = new Dictionary<BodyPart, float>();

    private float verticalStandart;
    private float horizontalStandart;

    public Vector2 Scale
    {
        get
        {
            if (!Application.isPlaying)
                return Vector2.one;
            return scale;
        }
    }

    private Vector2 scale;

    private void Start()
    {
        scale = Vector2.one;
        verticalStandart = 0f;
        horizontalStandart = 0f;
        foreach (var part in creator.segments)
        {
            var offset = part.gameObject.transform.position - transform.position;
            var vs = Mathf.Abs(Vector2.Dot(offset.normalized, Vector2.up));
            var hs = Mathf.Abs(Vector2.Dot(offset.normalized, Vector2.right));
            verticalStandart += offset.magnitude * vs;
            horizontalStandart += offset.magnitude * hs;
            verticalStrength[part] = vs;
            horizontalStrength[part] = hs;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateScale();  
    }

    private void CalculateScale()
    {
        var currentVertical = 0f;
        var currentHorizontal = 0f;
        foreach (var part in creator.segments)
        {
            var offset = part.gameObject.transform.position - transform.position;
            currentVertical += offset.magnitude * verticalStrength[part];
            currentHorizontal += offset.magnitude * horizontalStrength[part];
        }
        var y = currentVertical / verticalStandart;
        var x = currentHorizontal / horizontalStandart;
        scale = new Vector2(x, y);
    }
}
