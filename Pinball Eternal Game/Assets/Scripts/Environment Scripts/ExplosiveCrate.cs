using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCrate : MonoBehaviour
{
    bool knockback = false;
    bool collision = false;
    Rigidbody rb;

    [SerializeField] GameObject explosionCollision;
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

    private void Explosion()
    {
        animatorLink.Play("Crate Explosion");
        explosionCollision.SetActive(true);
    }    

    public void Knockback(float force, Vector3 tf)
    {
        StopAllCoroutines();
        knockback = true;
        Vector3 direction = transform.position - tf;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        Debug.Log("Hit");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (knockback && (other.CompareTag("enemy") || other.CompareTag("Player")))
        {
            collision = true;
        }
    }

}
