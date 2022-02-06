using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(PuffBallCreator))]
public class PuffBallVisual : MonoBehaviour
{
    private MeshFilter filter;
    private MeshRenderer renderer;
    private PuffBallCreator puffBall;

    private Mesh mesh;

    private int numOfVertices;
    [SerializeField]
    private float edgeOffset;
    [SerializeField]
    private float uvOffset;
    [SerializeField]
    private Vector2 uvBottomLeft;
    [SerializeField]
    private Vector2 uvTopRight;

    private void Start()
    {
        filter = GetComponent<MeshFilter>();
        renderer = GetComponent<MeshRenderer>();
        puffBall = GetComponent<PuffBallCreator>();
        ApplyVisuals();
    }

    public void ApplyVisuals()
    {
        numOfVertices = puffBall.segments.Length + 1;
        if (mesh == null)
            mesh = new Mesh();
        mesh.vertices = ApplyVertices(puffBall.segments
            .Select(s => s.gameObject.transform.position).ToArray());
        mesh.uv = ApplyUV(puffBall.segments.Select(s => s.gameObject.transform.position));
        mesh.triangles = ApplyTringles();
        filter.mesh = mesh;
    }

    private Vector2[] ApplyUV(IEnumerable<Vector3> positions)
    {
        var uv = new Vector2[numOfVertices];
        var offset = Vector2.one * (puffBall.Radius + puffBall.InnerRadius - uvOffset);
        var bottomLeft = (Vector2)transform.position - offset;
        var topRight = (Vector2)transform.position + offset;
        var sx = Mathf.InverseLerp(bottomLeft.x, topRight.x, transform.position.x);
        var sy = Mathf.InverseLerp(bottomLeft.y, topRight.y, transform.position.y);
        uv[0] = new Vector2(Mathf.Lerp(uvBottomLeft.x, uvTopRight.x, sx),
            Mathf.Lerp(uvBottomLeft.y, uvTopRight.y, sy));
        var i = 1;
        foreach (var p in positions)
        {
            var x = Mathf.InverseLerp(bottomLeft.x, topRight.x, p.x);
            var y = Mathf.InverseLerp(bottomLeft.y, topRight.y, p.y);
            uv[i] = new Vector2(Mathf.Lerp(uvBottomLeft.x, uvTopRight.x, x),
            Mathf.Lerp(uvBottomLeft.y, uvTopRight.y, y));
            i++;
        }
        return uv;
    }

    private Vector3[] ApplyVertices(Vector3[] positions)
    {
        var vertices = new Vector3[numOfVertices];
        vertices[0] = Vector2.zero;
        for (var l = 0; l < puffBall.NumberOfLayers; l++)
        {
            for (var i = 0; i < puffBall.NumberOfSegments; i++)
            {
                var p = positions[l * puffBall.NumberOfSegments + i];
                if (l < puffBall.NumberOfLayers - 1)
                {
                    vertices[l * puffBall.NumberOfSegments + i + 1] = 
                        transform.InverseTransformPoint(p);
                }
                else
                {
                    var offset = p - transform.position;
                    offset *= (offset.magnitude + puffBall.InnerRadius + edgeOffset) / offset.magnitude;
                    var point = transform.position + offset;
                    vertices[l * puffBall.NumberOfSegments + i + 1] = 
                        transform.InverseTransformPoint(point);
                }
            }
        }
        return vertices;
    }

    private int[] ApplyTringles()
    {
        var t = new List<int>();
        for (var i = 1; i <= puffBall.NumberOfSegments-1; i++)
        {
            t.Add(0);
            t.Add(i+1);
            t.Add(i);
        }
        t.Add(0);
        t.Add(1);
        t.Add(puffBall.NumberOfSegments);

        for (var layer = 1; layer < puffBall.NumberOfLayers; layer++)
        {
            for (var i = 1; i <= puffBall.NumberOfSegments-1; i++)
            {
                t.Add((layer - 1) * puffBall.NumberOfSegments + i);
                t.Add(layer * puffBall.NumberOfSegments + i + 1);
                t.Add(layer * puffBall.NumberOfSegments + i);

                t.Add((layer - 1) * puffBall.NumberOfSegments + i);
                t.Add((layer - 1) * puffBall.NumberOfSegments + i + 1);
                t.Add(layer * puffBall.NumberOfSegments + i + 1);
            }
            t.Add((layer - 1) * puffBall.NumberOfSegments + puffBall.NumberOfSegments);
            t.Add(layer * puffBall.NumberOfSegments + 1);
            t.Add(layer * puffBall.NumberOfSegments + puffBall.NumberOfSegments);


            t.Add((layer - 1) * puffBall.NumberOfSegments + puffBall.NumberOfSegments);
            t.Add((layer - 1) * puffBall.NumberOfSegments + 1);
            t.Add(layer * puffBall.NumberOfSegments + 1);
        }

        return t.ToArray();
    }

    public void BuildMeshEditor()
    {
        filter = GetComponent<MeshFilter>();
        renderer = GetComponent<MeshRenderer>();
        puffBall = GetComponent<PuffBallCreator>();
        numOfVertices = puffBall.NumberOfLayers * puffBall.NumberOfSegments + 1;
        if (mesh == null)
            mesh = new Mesh();
        var v = new List<Vector3>();
        var angle = Mathf.PI * 2 / puffBall.NumberOfSegments;
        var rad = puffBall.Radius / puffBall.NumberOfLayers;
        for (var j = 0; j < puffBall.NumberOfLayers; j++)
        {
            var offset = new Vector2(Mathf.Cos(0), Mathf.Sin(0)) * rad;
            var first = (Vector2)transform.position + offset;
            v.Add(first);
            for (var i = 1; i < puffBall.NumberOfSegments; i++)
            {
                offset = new Vector2(Mathf.Cos(angle * i), Mathf.Sin(angle * i)) * rad;
                v.Add((Vector2)transform.position + offset);
            }
            rad += puffBall.Radius / puffBall.NumberOfLayers;
        }
        mesh.vertices = ApplyVertices(v.ToArray());
        mesh.uv = ApplyUV(v);
        mesh.triangles = ApplyTringles();
        filter.sharedMesh = mesh;
    }

    private void Update()
    {
        mesh.vertices = ApplyVertices(puffBall.segments
            .Select(s => s.gameObject.transform.position).ToArray());
    }
}
