using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    bool isFireCD = false;
    [SerializeField] float fireCooldown = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isFireCD)
        {
            Fire();
        }
    }

    void Fire()
    {
        //shoots a raycast, checking to see if it's an enemy and within range, then activates the appropriate
        //knockback script
    }
}
