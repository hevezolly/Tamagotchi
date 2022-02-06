using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionCanvasPosition : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Transform rotator;
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private float canvasWidth;
    private Vector2 flipedOffset => new Vector2(-offset.x, offset.y);

    private Vector2 leftOffset => ((offset.x < 0) ? offset : flipedOffset);
    private Vector2 rightOffset => ((offset.x < 0) ? flipedOffset : offset);
    private bool isAdjusting = false;
    private Vector2 realOffset;
    private void AdjustCanvasPosition()
    {
        var edges = ScreenMovement.main;
        var pos = transform.position + (Vector3)realOffset;
        var edge = pos.x +
            ((pos.x < edges.mainCam.transform.position.x) ? 
            -canvasWidth : canvasWidth);
        var offsetToCompensate = (pos.x < edges.mainCam.transform.position.x) ?
            -edges.WidthHeight.x / 2 + canvasWidth :
            edges.WidthHeight.x / 2 - canvasWidth;
        if (edges.IsPositionOutsideScreen(edge))
        {
            var x = edges.mainCam.transform.position.x + offsetToCompensate;
            pos = new Vector3(x, pos.y, pos.z);
            if (offsetToCompensate < 0 && realOffset == leftOffset &&
                pos.x >= transform.position.x + rightOffset.x)
                realOffset = rightOffset;
            else if (offsetToCompensate > 0 && realOffset == rightOffset &&
                pos.x <= transform.position.x + leftOffset.x)
                realOffset = leftOffset;
        }
        canvas.transform.position = pos;
    }

    public void StartAdjusting()
    {
        isAdjusting = true;
        if (Camera.main.transform.position.x > transform.position.x)
        {
            canvas.transform.position = transform.position + (Vector3)rightOffset;
            realOffset = rightOffset;
        }
        else
        {
            canvas.transform.position = transform.position + (Vector3)leftOffset;
            realOffset = leftOffset;
        }
    } 
    public void StopAdjusting()
    {
        isAdjusting = false;
    }

    private void LateUpdate()
    {
        if (!isAdjusting)
            return;
        AdjustCanvasPosition();
        var forward = Vector3.forward;
        var up = rotator.transform.position - transform.position;
        rotator.rotation = Quaternion.LookRotation(forward, up);
    }

    private void OnDrawGizmos()
    {
        if (canvas == null)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(canvas.transform.position - Vector3.right * canvasWidth,
            canvas.transform.position + Vector3.right * canvasWidth);
        
        Gizmos.DrawSphere((Vector2)transform.position + offset, 0.1f);
        Gizmos.DrawSphere((Vector2)transform.position + flipedOffset, 0.1f);
    }
}
