using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;

    public float gravity = -9.81f;

    public float jumpHeight = 1f;

    public Transform groundCheck;

    public float groundDistance = 0.4f;

    public LayerMask groundMask;
    int coins;

    Vector3 velocity;
    bool isGrounded;

        // Update is called once per frame
        void Update()
        {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {

             velocity.y = -2f;
            

        }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
           
        }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

        
        }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("dupa");
        if (other.transform.gameObject.tag == "gold")
        {
            int addcoins = Random.Range(1, 5);
            coins = coins + addcoins;
            Destroy(other.gameObject);
            Debug.Log(coins);
        }
    }
}
