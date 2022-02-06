using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputHandler : MonoBehaviour
{
    public static InputHandler main;

    [SerializeField]
    private bool touch = false;

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private ScreenMovement movement;
    [SerializeField]
    [Range(0, 0.5f)]
    private float xMoveOffset;
    [SerializeField]
    private float maxMoveSpeed;

    [SerializeField]
    private List<ClickBehaviour> behaviours;

    private ClickBehaviour[] sortedBehaviours;

    private ClickBehaviour currentBehaviour;
    
    private void Awake()
    {
        main = this;
#if PLATFORM_ANDROID
        touch = true;
#endif
#if UNITY_EDITOR
        touch = false;
#endif
    }

    private bool isPress()
    {
        if (touch)
            return Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Began;
        return Input.GetMouseButtonDown(0);
    }

    private bool isRelease()
    {
        if (touch)
            return Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Ended;
        return Input.GetMouseButtonUp(0);
    }

    private Vector3 getPos()
    {
        if (touch)
            return Input.GetTouch(0).position;
        return Input.mousePosition;
    }

    public Vector3? GetPointerPosition()
    {
        if (touch && Input.touchCount == 0)
            return null;
        return cam.ScreenToWorldPoint(getPos());
    }

    private void TryMoveScreen(Vector2 pointerPosition)
    {
        var xValue = cam.ScreenToViewportPoint(pointerPosition).x;
        var t = 0f;
        var mult = 0;
        if (xValue < xMoveOffset)
        {
            t = Mathf.InverseLerp(xMoveOffset, 0, xValue);
            mult = -1;
        }
        else if (xValue > 1 - xMoveOffset)
        {
            t = Mathf.InverseLerp(1 - xMoveOffset, 1, xValue);
            mult = 1;
        }
        var offset = Mathf.Lerp(0, maxMoveSpeed, t) * mult * Time.deltaTime;
        movement.CameraMove(offset);
    }

    void Start()
    {
        sortedBehaviours = behaviours.OrderByDescending(b => b.Priority).ToArray();
    }

    private void ForcedStop()
    {
        currentBehaviour.SetFinishAction(null);
        currentBehaviour = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBehaviour != null)
        {
            var pos = getPos();
            if (!currentBehaviour.UseScreenCoordinates)
                pos = cam.ScreenToWorldPoint(pos);
            currentBehaviour.OnDrag(pos);
            if (isRelease())
            {
                if (currentBehaviour != null)
                {
                    currentBehaviour.OnRelease();
                }
                currentBehaviour = null;
            }
        }
        else if (isPress())
        {
            var screenPos = getPos();
            var worldPos =  cam.ScreenToWorldPoint(screenPos);
            foreach (var b in sortedBehaviours)
            {
                var pos = screenPos;
                if (!b.UseScreenCoordinates)
                    pos = worldPos;
                if (b.CheckClick(pos))
                {
                    currentBehaviour = b;
                    currentBehaviour.SetFinishAction(ForcedStop);
                    currentBehaviour.OnClick(pos);
                    break;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (currentBehaviour != null && currentBehaviour.EnableScrolling)
        {
            TryMoveScreen(getPos());
        }
    }

    private void OnDrawGizmos()
    {
        if (cam == null)
            return;
        Gizmos.color = Color.yellow;
        var lt = cam.ViewportToWorldPoint(new Vector2(xMoveOffset, 1));
        var lb = cam.ViewportToWorldPoint(new Vector2(xMoveOffset, 0));
        var rt = cam.ViewportToWorldPoint(new Vector2(1 - xMoveOffset, 1));
        var rb = cam.ViewportToWorldPoint(new Vector2(1 - xMoveOffset, 0));
        Gizmos.DrawLine(lt, lb);
        Gizmos.DrawLine(rt, rb);
    }
}
