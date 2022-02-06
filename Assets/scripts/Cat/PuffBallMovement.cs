using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffBallMovement : MonoBehaviour
{
    [SerializeField]
    private PuffBallCreator creator;
    [SerializeField]
    private Rigidbody2D rigidbody;

    [SerializeField]
    [Range(0, 1)]
    private float forceTransitionCoefficient;
    [SerializeField]
    private LayerMask GroundLayer;
    [SerializeField]
    private float jumpDelay;
    private float lastJumpTime;

    [SerializeField]
    private float DownAngle;

    private bool isOnGround;
    public bool IsOnGround => isOnGround;

    private Dictionary<Collider2D, int> groundColliders = new Dictionary<Collider2D, int>();
    private HashSet<Collider2D> bodyColliders = new HashSet<Collider2D>();

    public event System.Action GroundTouchEvent;


    // Start is called before the first frame update
    private void Start()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.TryGetComponent<Collider2D>(out var c))
            {
                bodyColliders.Add(c);
            }
        }
    }

    public void ApplyForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
    {
        if (!isOnGround)
            return;
        if (Time.time < lastJumpTime + jumpDelay)
            return;
        rigidbody.AddForce(force, mode);
        foreach (var segment in creator.segments)
        {
            segment.rb.AddForce(force * forceTransitionCoefficient, mode);
        }
        lastJumpTime = Time.time;
    }

    public void SetVelocity(Vector2 velocity)
    {
        if (!isOnGround)
            return;
        if (Time.time < lastJumpTime + jumpDelay)
            return;
        rigidbody.velocity = velocity;
        foreach (var segment in creator.segments)
        {
            segment.rb.velocity = velocity;
        }
        lastJumpTime = Time.time;
    }

    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((GroundLayer.value & (1 << collision.gameObject.layer)) == 0)
            return;
        var point = collision.ClosestPoint(transform.position);
        var angle = Vector2.Angle(Vector2.down,
            point - (Vector2)transform.position);
        if (angle > DownAngle)
            return;
        if (bodyColliders.Contains(collision))
            return;
        if (!isOnGround)
            GroundTouchEvent?.Invoke();
        isOnGround = true;
        if (!groundColliders.ContainsKey(collision))
            groundColliders.Add(collision, 0);
        groundColliders[collision]++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!groundColliders.ContainsKey(other))
            return;
        groundColliders[other]--;
        if (groundColliders[other] <= 0)
            groundColliders.Remove(other);
        if (groundColliders.Count == 0)
        {
            isOnGround = false;
        }
    }

    private void OnDrawGizmos()
    {
        var a1 = (-90 - DownAngle) * Mathf.Deg2Rad;
        var a2 = (-90 + DownAngle) * Mathf.Deg2Rad;
        var length = creator.Radius + creator.InnerRadius + 
            creator.ColliderOffset + 0.01f;
        var dir1 = new Vector2(Mathf.Cos(a1), Mathf.Sin(a1)) * length;
        var dir2 = new Vector2(Mathf.Cos(a2), Mathf.Sin(a2)) * length;

        Gizmos.DrawLine(transform.position, (Vector2)transform.position + dir1);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + dir2);

        if (isOnGround)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }


}
