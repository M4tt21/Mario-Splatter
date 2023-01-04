using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float vspeed = 0;
    private float speed = 20f;
    private Vector3 velocity;
    private float gravity = -9.81f;
    public float mouseSens = 100f;
    public float jumpSpeed = 10;
    public Transform cameraTransform;
    float rotation = 0;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

        float hzMove = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vtMove = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        Vector3 move = cameraTransform.right * hzMove + cameraTransform.forward * vtMove;

        if (!controller.isGrounded)
        {
            float jump = Input.GetAxis("Jump") * jumpSpeed;
            vspeed = jump;
        }
        vspeed -= gravity * Time.deltaTime;
        
        //Vector3 move = new Vector3(hzMove, 0f, vtMove);
        
        move.y = vspeed;
        controller.Move(move * Time.deltaTime);

        //if (controller.isGrounded && velocity.y < 0)
        //{
        //    velocity.y = -2f;
       // }


        controller.Move(move);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}