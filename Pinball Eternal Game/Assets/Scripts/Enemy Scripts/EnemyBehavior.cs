using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Tooltip("The points that this enemy will travel to")]
    [SerializeField] Transform[] patrolPoints;

    int currentPointIndex;

    [Tooltip("How long between each time this enemy moves")]
    [SerializeField] float waitTimeMove;
    bool mWait = false;

    [Tooltip("How fast this enemy is")]
    [SerializeField] float enemySpeed = 5;
    [Tooltip("How much damage does this enemy do")]
    [SerializeField] int enemyDamage = 1;

    bool alive = true;
    bool knockback = false;
    Rigidbody rb;

    [Tooltip("This enemies parent object")]
    [SerializeField] GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != patrolPoints[currentPointIndex].position && alive && !knockback)
        {
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPointIndex].position,
                enemySpeed * Time.deltaTime);
        }
        else
        {
            if (knockback && alive)
            {
                if (rb.velocity == new Vector3 (0,0,0))
                {
                    knockback = false;
                    mWait = false;
                }
            }
            if (!mWait && alive && !knockback)
            {
                mWait = true;
                StartCoroutine(MoveWait());
            }
        }
        //Patrol is started
        //Need to make a simple rotation to make sure the enemy is facing the patrol point it wants to head towards
        //Maybe can do this in MoveWait?
        //Need to create a detection system along with an attack system.
    }

    IEnumerator MoveWait()
    {
        yield return new WaitForSeconds(waitTimeMove);
        if (currentPointIndex + 1 < patrolPoints.Length)
            currentPointIndex++;
        else
            currentPointIndex = 0;
        mWait = false;
    }

    public void Knockback(float force, Vector3 tf)
    {
        StopAllCoroutines();
        knockback = true;
        Vector3 direction = tf - transform.position;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    private void attack()
    {
        //This will activate an attack trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("killBox") || other.CompareTag("deathFloor"))
        {
            alive = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            Destroy(parent);
        }
    }
}
