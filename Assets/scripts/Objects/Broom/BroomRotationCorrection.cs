using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomRotationCorrection : MonoBehaviour
{
    [SerializeField]
    private Broom broom;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Vector3 anglePidSettings;
    [SerializeField]
    private Vector3 velocityPidSettings;
    [SerializeField]
    private PhysicsMaterial2D correctingMaterial;
    private PhysicsMaterial2D restMaterial;
    private bool isCorrecting;

    private FloatPid angleCorrectionController;
    private FloatPid velocityCorrectionController;
    // Start is called before the first frame update
    void Start()
    {
        broom.PickUpEvent += OnPickUp;
        broom.DropEvent += OnDrop;
        angleCorrectionController = new FloatPid
            (anglePidSettings.x, anglePidSettings.y, anglePidSettings.z);
        velocityCorrectionController = new FloatPid
            (velocityPidSettings.x, velocityPidSettings.y, velocityPidSettings.z);
        restMaterial = rb.sharedMaterial;
    }
    private void OnPickUp()
    {
        isCorrecting = true;
        rb.sharedMaterial = correctingMaterial;
    }

    private void OnDrop()
    {
        isCorrecting = false;
        rb.sharedMaterial = restMaterial;
    }

    private void FixedUpdate()
    {
        if (!isCorrecting)
            return;
        var angleError = Vector2.SignedAngle(transform.up, Vector2.up);
        var angleCorrection = angleCorrectionController
            .Update(angleError, Time.fixedDeltaTime);
        rb.AddTorque(angleCorrection);
        var velocityError = -rb.angularVelocity;
        var velocityCorrection = velocityCorrectionController
            .Update(velocityError, Time.fixedDeltaTime);
        rb.AddTorque(velocityCorrection);
    }

    private void OnValidate()
    {
        angleCorrectionController = new FloatPid
            (anglePidSettings.x, anglePidSettings.y, anglePidSettings.z);
        velocityCorrectionController = new FloatPid
            (velocityPidSettings.x, velocityPidSettings.y, velocityPidSettings.z);
    }

    private void OnDestroy()
    {
        broom.PickUpEvent -= OnPickUp;
        broom.DropEvent -= OnDrop;
    }
}
