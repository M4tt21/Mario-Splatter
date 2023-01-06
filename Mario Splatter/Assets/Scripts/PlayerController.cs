using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    private static float wSpeed = 5f;
    private static float rSpeed = 10f;
    private static int rScale = 5;
    private float currentSpeed = wSpeed;
    private Vector3 velocity;
    private float gravity = -9.81f;
    public float mouseSens = 100f;
    public Transform cameraTransform;
    float rotation = 0;
    public float jumpspeed = 2;



    private GameObject guns;




    // Start is called before the first frame update
    void Start()
    {
        guns = GameObject.FindGameObjectWithTag("Guns");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float hzMove = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        float vtMove = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        Vector3 move = transform.right * hzMove + transform.forward * vtMove;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = gravity/3;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpspeed * -3.0f * gravity);
            animator.SetBool("isJumping", true);
        }
        else animator.SetBool("isJumping", false);

        velocity.y += gravity*2 * Time.deltaTime;
        controller.Move((velocity * Time.deltaTime) + move);

        //sprint
        if(Input.GetButton("Fire3") && (Input.GetAxis("Vertical")>0) && !Input.GetButton("Horizontal"))
        {
            if(currentSpeed<rSpeed)currentSpeed += rSpeed * Time.deltaTime;
            //Se corre disabilito l'arma che ha in mano
            guns.gameObject.SetActive(false);
        }
        else if(!Input.GetButton("Fire3"))
        {
            if (currentSpeed >wSpeed) currentSpeed -= wSpeed * Time.deltaTime;
            guns.gameObject.SetActive(true);
        }

        //if ((Input.GetButtonUp("Fire3") || Input.GetButton("Horizontal") || (Input.GetAxis("Vertical") < 0)) && currentSpeed>wSpeed){currentSpeed -= wSpeed * Time.deltaTime;}

        float dotPF = Vector3.Dot(transform.forward, controller.velocity);
        float dotPH = Vector3.Dot(transform.right, controller.velocity);
        float dotPV = Vector3.Dot(transform.up, controller.velocity);

        animator.SetFloat("fVelocity", (float)Math.Round(dotPF, 2));
        animator.SetFloat("hVelocity", (float)Math.Round(dotPH,2));
        animator.SetFloat("vVelocity", (float)Math.Round(dotPV, 2));
        
        animator.SetFloat("turn", mouseX*10);

        if (dotPF == 0 && dotPH == 0 && dotPV == 0) animator.SetBool("isStill", true);



        //Movimento Telecamera
        rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }


    private int turnDirection(float Axis){  //1=Right -1=Left
        if (Axis < 0) return -1;
        else if (Axis > 0) return 1;
        return 0;
    }
}