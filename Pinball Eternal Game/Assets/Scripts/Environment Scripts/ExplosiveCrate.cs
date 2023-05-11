/***
 * ExplosiveCrates.cs
 * By Nathan Boles
 * 
 * This object doesn't do much on it's own. But when hit by something that calls it's Knockback trait, it'll be 
 * sent in a direction away from that object til it either loses all it's velocity or hits an enemy or player.
 * When this happens, create an explosion using an animation that creates a killbox collider.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCrate : MonoBehaviour
{
    bool knockback = false;
    bool collision = false;
    Rigidbody rb;

    [Tooltip("The game object that contains this objects explosion collision (tagged with killBox)")]
    [SerializeField] GameObject explosionCollision;
    [Tooltip("The animator of this object")]
    [SerializeField] Animator animatorLink;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (knockback)
        {
            if (rb.velocity == new Vector3(0,0,0) || collision)
            {
                Explosion();
            }
        }
    }

    /// <summary>
    /// When called, this activates the crates explosion animation along with activating it's hitbox.
    /// </summary>
    private void Explosion()
    {
        animatorLink.Play("Crate Explosion");
        explosionCollision.SetActive(true);
    }

    /// <summary>
    /// The method that handles the intial effects of knockback effects. It applies the impulse force for the object
    /// and pushes this object away from what is causing the knockback.
    /// </summary>
    /// <param name="force">The amount of force applied to this object</param>
    /// <param name="tf">The position of the object applying knockback to this</param>
    public void Knockback(float force, Vector3 tf)
    {
        StopAllCoroutines();
        knockback = true;
        Vector3 direction = transform.position - tf;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        Debug.Log("Hit");
    }

    /// <summary>
    /// When this touches a player or enemy while suffering from knockback, explode!
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (knockback && (other.CompareTag("enemy") || other.CompareTag("Player")))
        {
            collision = true;
        }
    }

}
