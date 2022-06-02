using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController Cc;

    // Start is called before the first frame update
    void Start()
    {
        Cc = GetComponent<CharacterController>();
    }

    void ApplyMove(float speed = 6f)
    {
        if (Cc.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
        }
    }

    void ApplyGravity(float gravity = 20f)
    {
        moveDirection.y -= gravity * Time.deltaTime;
    }

    void ApplyJump(float speedJump = 8f)
    {
        if (Input.GetButtonDown("Jump") || Input.GetButton("Jump"))
        {
            moveDirection.y = speedJump;
        }
    }

    void ApplyCamera(float speedCamera = 10f)
    {
        if (Input.GetMouseButton(0))
        {
            float mov = Input.GetAxis("Mouse X");
            if (mov != 0)
            {
                transform.Rotate(Vector3.down * Input.GetAxis("Mouse X") * speedCamera);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMove();
        ApplyGravity();
        ApplyJump();
        ApplyCamera();
        Cc.Move(moveDirection * Time.deltaTime);
    }
}
