using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 direction;
    CharacterController cc;

    [Tooltip("What is being moved?")]
    [SerializeField] Transform playerBodyLink;

    [Tooltip("How fast the player should move")]
    [SerializeField] float playerWalk = 5f;

    [Tooltip("How fast the player should run")]
    [SerializeField] float playerRun = 10f;

    [SerializeField] float jumpHeight = 3f;

    [SerializeField] float gravityValue = -9.8f;

    [SerializeField] bool sprint = false;
    

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Movement();
    }

    void GetInput()
    {
        float speed = playerWalk;
        if (Input.GetButton("Sprint"))
        {
            speed = playerRun;
            sprint = true;
        }
        else
            sprint = false;

        float x = Input.GetAxisRaw("Horizontal") * speed;
        float z = Input.GetAxisRaw("Vertical") * speed;

        if (cc.isGrounded)
        {
            if (direction.y < 0)
            {
                direction.y = -1f;
            }
            if (Input.GetButton("Jump"))
            {
                direction.y += Mathf.Sqrt(gravityValue * -3f * jumpHeight);
            }
        } else
        {
            direction.y += gravityValue * Time.deltaTime;
        }

        Vector3 move = transform.right * x + transform.forward * z + direction;

        cc.Move(move * Time.deltaTime);
        

    }

    void Movement()
    {

    }
}
