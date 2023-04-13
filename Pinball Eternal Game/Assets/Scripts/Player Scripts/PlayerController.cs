using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCameraLink;
    [SerializeField] Transform playerBodyLink;
    [SerializeField] float xSensitivity;
    [SerializeField] float ySensitivity;
    float xRotate;
    float yRotate;

    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        MouseRotationUpdate();
        PlayerMoveUpdate();

    }

    void GetInput()
    {
        
    }

    void MouseRotationUpdate()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X") * xSensitivity * Time.deltaTime, 
                                        Input.GetAxisRaw("Mouse Y") * ySensitivity * Time.deltaTime);

        yRotate += mouseDelta.x;
        xRotate -= mouseDelta.y;
        xRotate = Mathf.Clamp(xRotate, -90.0f, 90.0f);

        transform.rotation = Quaternion.Euler(xRotate, yRotate, 0);
        playerBodyLink.rotation = Quaternion.Euler(0, yRotate, 0);
        playerCameraLink.rotation = transform.rotation;
    }

    void PlayerMoveUpdate()
    {

    }
}
