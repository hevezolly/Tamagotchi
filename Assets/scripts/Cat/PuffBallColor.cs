using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffBallColor : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer renderer;
    [SerializeField]
    private Color standartColor;

    public Color StandartColor => standartColor;

    private Material mat;

    private void Awake()
    {
        mat = renderer.material;
    }

    public void SetColor(Color color)
    {
        mat.color = color;
    }

    public void ResetColor()
    {
        mat.color = standartColor;
    }

}
