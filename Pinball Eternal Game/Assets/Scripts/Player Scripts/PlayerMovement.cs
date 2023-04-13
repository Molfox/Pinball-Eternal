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
        if (cc.isGrounded)
        {
            if (Input.GetButtonDown("Sprint"))
            {
                direction = new Vector3(Input.GetAxisRaw("Horizontal") + Time.deltaTime + playerRun, 0, Input.GetAxisRaw("Vertical") + Time.deltaTime + playerRun);
            }
            else //walk speed
            {
                direction = new Vector3(Input.GetAxisRaw("Horizontal") + Time.deltaTime + playerWalk, 0, Input.GetAxisRaw("Vertical") + Time.deltaTime + playerWalk);
            }
            if (Input.GetButtonDown("Jump"))
            {
                direction.y += Mathf.Sqrt(gravityValue * -3f * jumpHeight);
            }
        } else
            direction.y += gravityValue * Time.deltaTime;

        cc.Move(direction);

    }

    void Movement()
    {

    }
}
