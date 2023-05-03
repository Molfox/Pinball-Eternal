using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] Transform playerCameraLink;
    [SerializeField] Transform playerBodyLink;
    [SerializeField] float xSensitivity = 100f;
    [SerializeField] float ySensitivity = 100f;
    float xRotate;
    float yRotate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseRotationUpdate();
    }

    void MouseRotationUpdate()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X") * xSensitivity,
                                        Input.GetAxisRaw("Mouse Y") * ySensitivity);

        xRotate -= mouseDelta.y;
        xRotate = Mathf.Clamp(xRotate, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(xRotate, 0, 0);
        playerBodyLink.Rotate(Vector3.up * mouseDelta.x);
    }
}
