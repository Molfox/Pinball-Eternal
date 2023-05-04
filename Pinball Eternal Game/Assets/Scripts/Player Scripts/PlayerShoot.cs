using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    bool isFireCD = false;
    [SerializeField] float fireCooldown = 1f;
    [SerializeField] float range = 10f;
    [SerializeField] float force = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && !isFireCD)
        {
            Fire();
        }
    }

    void Fire()
    {
        RaycastHit hit;
        if(Physics.Raycast(this.transform.position,this.transform.forward, out hit, range))
        {
            if(hit.collider.CompareTag("enemy") && hit.transform.GetComponent<EnemyBehavior>() != null)
            {
                hit.transform.GetComponent<EnemyBehavior>().Knockback(force, transform.position);
            }
        }
    }
}
