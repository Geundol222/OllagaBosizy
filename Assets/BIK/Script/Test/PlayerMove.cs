using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveForce;
    [SerializeField] float moveMaxForce;

    [SerializeField] float jumpForce;

    Vector3 moveDir;
    Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        rigidbody.AddForce(moveDir.x * moveForce * transform.right, ForceMode2D.Force);
        if(rigidbody.velocity.magnitude > moveMaxForce)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * moveMaxForce;
        }
    }

    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }
}
