using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeDirection : MonoBehaviour
{
    [SerializeField]
    private EyeData eye1;
    [SerializeField]
    private EyeData eye2;

    [SerializeField]
    private float maxDistance;

    private enum TargetType { Transform, Position, Direction, Mouse}
    private TargetType type;
    private Transform target;
    private Vector2 targetPos;
    private Vector2 offset;
    
    private void DrawEye(EyeData data)
    {
        Gizmos.DrawWireSphere((Vector2)data.eyeContainer.position + data.eyeCenterOffset, 
            data.maxMoveDistance);
        Gizmos.DrawWireSphere((Vector2)data.eyeContainer.position + data.eyeCenterOffset,
            maxDistance);
    }

    public void SetLookTargetPosition(Vector2 positon)
    {
        type = TargetType.Position;
        targetPos = positon;
    }

    public void SetLookTargetDirection(Vector2 direction)
    {
        type = TargetType.Direction;
        offset = direction;
    }

    public void SetLookTarget(Transform target)
    {
        type = TargetType.Transform;
        this.target = target;
    }

    public void FollowPointer()
    {
        type = TargetType.Mouse;
    }

    private void SetLookPosition(Vector2 position)
    {
        SetEyePos(eye1, position);
        SetEyePos(eye2, position);
    }

    private void SetLookDirection(Vector2 dir)
    {
        SetEyeDir(eye1, dir);
        SetEyeDir(eye2, dir);
    }

    private void SetEyeDir(EyeData eye, Vector2 dir)
    {
        var length = Mathf.Lerp(0, eye.maxMoveDistance, dir.magnitude);
        var eyeOffset = dir.normalized * length;
        var eyePos = (Vector2)eye.eyeContainer.position + eye.eyeCenterOffset + eyeOffset;
        eye.eye.position = eyePos;
    }

    private void SetEyePos(EyeData eye, Vector2 pos)
    {
        var offset = pos - ((Vector2)eye.eyeContainer.position + eye.eyeCenterOffset);
        offset = offset.normalized * Mathf.Min(offset.magnitude, maxDistance) / maxDistance;
        SetEyeDir(eye, offset);
    }

    private void Start()
    {
        FollowPointer();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        DrawEye(eye1);
        DrawEye(eye2);
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case TargetType.Direction:
                SetLookDirection(offset);
                return;
            case TargetType.Mouse:
                var pos = InputHandler.main.GetPointerPosition();
                if (pos == null)
                    SetLookDirection(Vector2.zero);
                else
                    SetLookPosition(pos.Value);
                return;
            case TargetType.Position:
                SetLookPosition(targetPos);
                return;
            case TargetType.Transform:
                SetLookPosition(target.transform.position);
                return;
            default:
                return;
        }
    }

    [System.Serializable]
    private class EyeData
    {
        public Transform eye;
        public Transform eyeContainer;
        public float maxMoveDistance;
        public Vector2 eyeCenterOffset;
    }
}
