using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Tooltip("The points that this enemy will travel to")]
    [SerializeField] Transform[] patrolPoints;

    [SerializeField] int currentPointIndex;

    [Tooltip("How long between each time this enemy moves")]
    [SerializeField] float waitTimeMove;
    bool mWait = false;

    [Tooltip("How fast this enemy is")]
    [SerializeField] float enemySpeed = 5;
    [Tooltip("How much damage does this enemy do")]
    [SerializeField] int enemyDamage = 1;
    [Tooltip("How far can this enemy see")]
    [SerializeField] float sightRange = 10f;
    [Tooltip("How far will it try to attack")]
    [SerializeField] float attackRange = 1f;
    [Tooltip("How far the player needs to be to escape aggro")]
    [SerializeField] float escapeRange = 15f;

    GameObject playerBeingChased;

    GameManager gm;

    bool alive = true;
    bool knockback = false;
    Rigidbody rb;

    [Tooltip("This is the game object with the collider used when this enemy attacks")]
    [SerializeField] GameObject attackCollider;

    [Tooltip("This enemies parent object")]
    [SerializeField] GameObject parent;

    [Tooltip("Meant for testing purposes. 1: Patrol, 2: Wait, 3: Knockback, 4: Player Chase, 5: Death")]
    [SerializeField] int stage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = FindObjectOfType<GameManager>();
        stage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (stage)
        {
            case 1: // Patrol
                PatrolStage();
                break;
            case 2: // Patrol Wait
                CheckForPlayer();
                break;
            case 3: // Knockback
                KBCheck();
                break;
            case 4: // Player Chase
                PlayerChase();
                break;
            case 5: // Death
                break;
            default:
                break;
        }
        //Patrol is started
        //Need to make a simple rotation to make sure the enemy is facing the patrol point it wants to head towards
        //Maybe can do this in MoveWait?
        //Need to create a detection system along with an attack system.
    }

    private void PatrolStage()
    {
        if (transform.position != patrolPoints[currentPointIndex].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPointIndex].position,
                enemySpeed * Time.deltaTime);
        }
        else
        {
            mWait = true;
            stage = 2;
            StartCoroutine(MoveWait());
        }
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        RaycastHit hit;
        if(Physics.Raycast(this.transform.position, this.transform.forward, out hit, sightRange))
        {
            if(hit.collider.CompareTag("Player"))
            {
                playerBeingChased = hit.transform.gameObject;
                stage = 4;
            }
        }
    }

    private void PlayerChase()
    {
        float distance = Vector3.Distance(transform.position, playerBeingChased.transform.position);
        if (distance < attackRange)
        {
            Attack();
        } else if (distance > escapeRange)
        {
            stage = 1;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, playerBeingChased.transform.position,
                enemySpeed * Time.deltaTime);
        }
    }

    IEnumerator MoveWait()
    {
        yield return new WaitForSeconds(waitTimeMove);
        if (currentPointIndex + 1 < patrolPoints.Length)
            currentPointIndex++;
        else
            currentPointIndex = 0;
        stage = 1;
    }

    public void Knockback(float force, Vector3 tf)
    {
        StopAllCoroutines();
        knockback = true;
        stage = 3;
        Vector3 direction = transform.position - tf;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        Debug.Log("Hit");
    }

    private void KBCheck()
    {
        if (rb.velocity == new Vector3(0, 0, 0))
        {
            knockback = false;
            mWait = false;
            stage = 1;
        }
    }

    private void Attack()
    {
        Debug.Log("Attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("killBox") || other.CompareTag("deathFloor") || (other.CompareTag("enemy") && knockback))
        {
            alive = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            Destroy(parent);
        }
    }
}
