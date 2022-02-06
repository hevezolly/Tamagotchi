using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMovement : MonoBehaviour
{
    public static ScreenMovement main;

    [SerializeField]
    private Camera cam;

    public Camera mainCam => cam;

    [SerializeField]
    private float worldWidth;
    [SerializeField]
    private float worldCenterOffset;

    public Vector2 WidthHeight
    {
        get
        {
            if (!Application.isPlaying)
            {
                return cam.ViewportToWorldPoint(new Vector3(1, 1))
                - cam.ViewportToWorldPoint(new Vector3(0, 0));
            }
            return _wh;
        }
    }
    private Vector2 _wh;

    public bool IsPositionOutsideScreen(float x)
    {
        var offset = x - cam.transform.position.x;
        return (WidthHeight.x / 2 - Mathf.Abs(offset)) < 0;
    }

    private Vector3 WorldCenter
    {
        get
        {
            if (!Application.isPlaying)
                return cam.transform.position + Vector3.right * worldCenterOffset;
            return _center;
        }
    }
    private Vector3 _center;
    // Start is called before the first frame update 

    private void Awake()
    {
        if (main == null)
            main = this;
        _center = cam.transform.position + Vector3.right * worldCenterOffset;
        _wh = cam.ViewportToWorldPoint(new Vector3(1, 1))
                - cam.ViewportToWorldPoint(new Vector3(0, 0));
    }

    private void OnValidate()
    {
        if (cam != null)
        {
            worldWidth = Mathf.Max(worldWidth, WidthHeight.x);
            var value = Mathf.Abs(worldWidth - WidthHeight.x) / 2;
            worldCenterOffset = Mathf.Clamp(worldCenterOffset, -value, value);
        }        
    }

    private void OnDrawGizmos()
    {
        if (cam != null) 
        {
            Gizmos.color = Color.red;
            var center = WorldCenter;
            Gizmos.DrawLine(center - Vector3.right * worldWidth / 2 - Vector3.up * WidthHeight.y / 2,
                center - Vector3.right * worldWidth / 2 + Vector3.up * WidthHeight.y / 2);
            Gizmos.DrawLine(center + Vector3.right * worldWidth / 2 - Vector3.up * WidthHeight.y / 2,
                center + Vector3.right * worldWidth / 2 + Vector3.up * WidthHeight.y / 2);
            Gizmos.DrawLine(center - Vector3.right * worldWidth / 2 + Vector3.up * WidthHeight.y / 2,
                center + Vector3.right * worldWidth / 2 + Vector3.up * WidthHeight.y / 2);
            Gizmos.DrawLine(center - Vector3.right * worldWidth / 2 - Vector3.up * WidthHeight.y / 2,
                center + Vector3.right * worldWidth / 2 - Vector3.up * WidthHeight.y / 2);

            Gizmos.DrawLine(WorldCenter - Vector3.right * (worldWidth - WidthHeight.x) / 2, 
                WorldCenter + Vector3.right * (worldWidth - WidthHeight.x) / 2);
        }
    }

    public void CameraMove(float offset)
    {
        var pos = cam.transform.position.x + offset;
        SetCameraPosition(pos);
    }

    public void SetCameraPosition(float pos)
    {
        var maxOffset = (worldWidth - WidthHeight.x) / 2;
        pos = Mathf.Clamp(pos, (WorldCenter.x - maxOffset), (WorldCenter.x + maxOffset));
        cam.transform.position = new Vector3(pos, cam.transform.position.y, cam.transform.position.z);
    }
}
