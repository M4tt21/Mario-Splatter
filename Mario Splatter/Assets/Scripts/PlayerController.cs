using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public CharacterController controller;
    private float speed = 20f;
    private Vector3 velocity;
    private float gravity = -9.81f;
    public float mouseSens = 100f;
    public Transform cameraTransform;
    float rotation = 0;
    public float jumpspeed = 5;


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

        Vector3 move = transform.right * hzMove + transform.forward * vtMove;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        controller.Move(move);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpspeed * -3.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        //Movimento Telecamera
        rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}