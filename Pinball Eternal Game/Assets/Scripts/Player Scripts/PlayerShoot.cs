/***
 * PlayerShoot.cs
 * By Nathan Boles
 * 
 * When the player presses the fire button, then this shoots. If it hits a shootable object, it pushes that object 
 * back with a set amount of force. 
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    bool isFireCD = false;

    [Tooltip("How long between each time this can fire")]
    [SerializeField] float fireCooldown = 1f;
    [Tooltip("How far is the range")]
    [SerializeField] float range = 5f;
    [Tooltip("How much force does this apply")]
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
            isFireCD = true; //You can't shoot one shot per frame
            StartCoroutine(FireWait());
        }
    }

    /// <summary>
    /// Use a raycast to check to see if an object within range is an enemy or crate object. 
    /// This then pulls from that objects respective script to call upon it's Knockback method.
    /// </summary>
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
                hit.transform.GetComponent<ExplosiveCrate>().Knockback(force, transform.position);
            }
        }
    }

    /// <summary>
    /// Wait for so many seconds before being able to use Fire again.
    /// </summary>
    /// <returns></returns>
    IEnumerator FireWait()
    {
        yield return new WaitForSeconds(fireCooldown);
        isFireCD = false;
    }    

}
