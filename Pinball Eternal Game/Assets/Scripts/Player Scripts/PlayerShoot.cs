using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    bool isFireCD = false;
    [SerializeField] float fireCooldown = 1f;
    [SerializeField] float range = 5f;
    [SerializeField] float force = 1000f;

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
            isFireCD = true;
            StartCoroutine(FireWait());
        }
    }

    void Fire()
    {
        RaycastHit hit;
        if(Physics.Raycast(this.transform.position,this.transform.forward, out hit, range))
        {
            Debug.Log("Ray Out");
            if(hit.collider.CompareTag("enemy") && hit.transform.GetComponent<EnemyBehavior>() != null)
            {
                hit.transform.GetComponent<EnemyBehavior>().Knockback(force, transform.position);
                Debug.Log(hit.collider.tag);
            } 
            else if (hit.collider.CompareTag("crate") && hit.transform.GetComponent<ExplosiveCrate>() != null)
            {

            }
        }
    }

    IEnumerator FireWait()
    {
        yield return new WaitForSeconds(fireCooldown);
        isFireCD = false;
    }    

}
