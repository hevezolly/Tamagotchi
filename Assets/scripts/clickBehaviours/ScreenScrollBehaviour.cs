using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "screen scroll", menuName = "Click Behaviours/scroll")]
public class ScreenScrollBehaviour : ClickBehaviour
{
    private Vector3 prevPos;
    [SerializeField]
    private float minOffsetToMove;

    private float camWidth;
    public override bool CheckClick(Vector3 position)
    {
        return true;

    }

    public override void OnClick(Vector3 position)
    {
        prevPos = position;
        camWidth = (Camera.main.ViewportToWorldPoint(Vector2.one) -
            Camera.main.ViewportToWorldPoint(Vector2.zero)).x;
    }

    public override void OnDrag(Vector3 position)
    {
        var screenOffset = (position - prevPos).x;
        var offset = (screenOffset * camWidth / Screen.width);
        Debug.DrawRay(Camera.main.transform.position, Vector3.right * offset);
        ScreenMovement.main.CameraMove(-offset);
        prevPos = position;
    }
}
