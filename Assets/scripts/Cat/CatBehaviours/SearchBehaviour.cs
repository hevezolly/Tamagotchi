using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "food search", menuName = "Cat Behaviours/foodSearch")]
public class SearchBehaviour : CatBehaviour
{
    [SerializeField]
    private float SearchRadius;
    [SerializeField]
    private float maxJumpDist;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private LayerMask possibleFood;
    [SerializeField]
    private LayerMask sightBlock;
    [SerializeField]
    private float minVelToJump;
    [SerializeField]
    private float searchDelays;

    private float lastSearchTime;

    private Food target;
    private float distance;
    private void SearchFood()
    {
        lastSearchTime = Time.time;
        var colliders = Physics2D.OverlapCircleAll(mainObjectTransform.position, SearchRadius,
            possibleFood);
        target = null;
        var minDist = float.MaxValue;
        foreach (var food in colliders
            .Select(c => c.gameObject.GetComponent<Food>())
            .Where(f => f != null)
            .Where(f => f.CanBeEaten))
        {
            var dist = Vector2.Distance(mainObjectTransform.position, food.transform.position);
            if (dist >= minDist)
                continue;
            var hits = Physics2D.RaycastAll(mainObjectTransform.position,
                food.transform.position - mainObjectTransform.position, 
                dist, sightBlock & possibleFood);
            var isBlocked = false;
            foreach (var h in hits)
            {
                if (h.collider.gameObject != food.gameObject)
                {
                    isBlocked = true;
                    break;
                }
            }
            if (isBlocked)
                continue;
            minDist = dist;
            target = food;
        }
        distance = minDist;
    }

    private void SetEyes()
    {
        if (target != null)
        {
            eyeDirection.SetLookTarget(target.transform);
        }
        else
        {
            eyeDirection.FollowPointer();
        }
    }

    private Vector2 GetVelocity(float maxHeight, Vector2 start, Vector2 dest)
    {
        var g = Physics2D.gravity.y;
        if (dest.y >= start.y)
        {
            var v = new Vector2();
            var px = (dest - start).x;
            var py = (dest - start).y;
            var s1 = Mathf.Sqrt(-2 * maxHeight / g);
            var s2 = Mathf.Sqrt(2 * (py - maxHeight) / g);
            v.x = px / (s1 + s2);
            v.y = Mathf.Sqrt(-2 * g * maxHeight);
            return v;
        }
        return Vector2.zero;
    }

    private void Jump()
    {
        var dir = (Vector2)(target.transform.position - mainObjectTransform.position);
        dir = dir.normalized * Mathf.Min(maxJumpDist, dir.magnitude);
        var end = (Vector2)mainObjectTransform.position + dir;
        end.y = Mathf.Max(end.y, mainObjectTransform.position.y);
        var h = end.y - mainObjectTransform.position.y + jumpHeight;
        var v = GetVelocity(h, mainObjectTransform.position, end);
        if (v != Vector2.zero)
            movement.SetVelocity(v);
    }

    private void SetEmotions()
    {
        eyeAnimator.PlayIdle();
        mouthAnimator.PlayIdle();
    }

    public override void Activate()
    {
        SearchFood();
        SetEmotions();
        SetEyes();
    }

    public override void Update()
    {
        if (target != null)
        {
            if (movement.IsOnGround && rb.velocity.magnitude <= minVelToJump)
            {
                Jump();
            }
        }
        if (Time.time > lastSearchTime + searchDelays)
        {
            SearchFood();
            SetEyes();
        }
    }

    protected override void DrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mainObjectTransform.position, SearchRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(mainObjectTransform.position, maxJumpDist);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(mainObjectTransform.position, mainObjectTransform.position + Vector3.up * jumpHeight);
    }
}
