using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emotionTest : MonoBehaviour
{
    [SerializeField]
    private PuffBallMovement movement;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            var dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            movement.ApplyForce(dir * 5, ForceMode2D.Impulse);
        }
    }
}
