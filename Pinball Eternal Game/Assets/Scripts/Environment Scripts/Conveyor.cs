using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [Tooltip("The direction that objects are pushed towards")]
    [SerializeField] Transform dir;
    [Tooltip("The speed they are moved")]
    [SerializeField] float slantSpeed;

    void OnCollisionStay(Collision collision)
    {
        collision.transform.position = Vector3.MoveTowards(collision.transform.position, dir.position,
                      slantSpeed * Time.deltaTime);
    }
}
