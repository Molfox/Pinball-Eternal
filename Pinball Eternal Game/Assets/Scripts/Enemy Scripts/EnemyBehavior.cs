/***
 * EnemyBehavior.cs
 * By Nathan Boles
 * Based on and expanded from code and concepts taught by Professor Cory Newman
 * 
 * This script controls all things dealing with the basic enemy of this game. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Tooltip("The points that this enemy will travel to")]
    [SerializeField] Transform[] patrolPoints;

    [Tooltip("The point that this enemy will respawn at")]
    [SerializeField] Transform respawnPoint;

    [SerializeField] int currentPointIndex = 1;

    [Tooltip("How long between each time this enemy moves")]
    [SerializeField] float waitTimeMove;
    bool mWait = false;

    [Tooltip("How fast this enemy is")]
    [SerializeField] float enemySpeed = 5;
    [Tooltip("How much damage does this enemy do")]
    [SerializeField] int enemyDamage = 1;
    [Tooltip("How long it takes before this enemy can attack again")]
    [SerializeField] float attackCooldown = 3;
    [Tooltip("How far can this enemy see")]
    [SerializeField] float sightRange = 10f;
    [Tooltip("How far will it try to attack")]
    [SerializeField] float attackRange = 1f;
    [Tooltip("How far the player needs to be to escape aggro")]
    [SerializeField] float escapeRange = 15f;
    [Tooltip("How many points is this enemy worth?")]
    [SerializeField] int pointsWorth = 200;
    [Tooltip("How long does it take for this enemy to respawn")]
    [SerializeField] float respawnTime = 20f;

    GameObject playerBeingChased;

    GameManager gm;

    bool alive = true;
    bool knockback = false;
    bool attackCD = false;
    Rigidbody rb;

    [Tooltip("This is the game object with the collider used when this enemy attacks")]
    [SerializeField] GameObject attackCollider;

    [Tooltip("This enemies parent object")]
    [SerializeField] GameObject parent;

    [Tooltip("Meant for testing purposes. 1: Patrol, 2: Wait, 3: Knockback, 4: Player Chase, 5: Rotating, 6: Dead")]
    [SerializeField] int stage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = FindObjectOfType<GameManager>();
        stage = 5; //Start on Rotate
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
            case 5: // Rotate
                RotateStage();
                break;
            case 6: // Purgatory
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// This stage has the character look for the current next patrol point, then move towards it. If this character
    /// has reached that patrol point, activate MoveWait. Always look for the player while doing this. 
    /// </summary>
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

    /// <summary>
    /// This method, when called, creates a Raycast starting from the front of this object.
    /// If that raycast hits a player object, move to Chase behavior
    /// </summary>
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

    /// <summary>
    /// This stage looks at the next patrol point and sees if this object is looking at it. If not, turn
    /// til you are facing it then go back to patrol. Check for the player while doing this.
    /// </summary>
    private void RotateStage()
    {
        if (Vector3.Angle(transform.forward, patrolPoints[currentPointIndex].position - transform.position) > .3) // Some leniency is needed so that the object doesn't get stuck.
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, patrolPoints[currentPointIndex].position - transform.position,
                                                            enemySpeed * Time.deltaTime, 0.0f));
        }
        else
            stage = 1;
        CheckForPlayer();
    }

    /// <summary>
    /// This script is a basic enemy chase script that checks the distance between the player and 
    /// this object. If this object is close enough then attack. If it's to far, then give up the
    /// chase and head back to your patrol route. Otherwise move towards and rotate towards the player. 
    /// </summary>
    private void PlayerChase()
    {
        float distance = Vector3.Distance(transform.position, playerBeingChased.transform.position);
        if (distance < attackRange)
        {
            Attack();
        } 
        else if (distance > escapeRange)
        {
            playerBeingChased = null;
            stage = 5;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, playerBeingChased.transform.position,
                enemySpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, playerBeingChased.transform.position - transform.position,
                                                            enemySpeed * Time.deltaTime, 0.0f));
        }
    }

    /// <summary>
    /// An IEnumerator that waits for howeverlong waitTimeMove is, then changes the point index to the next one in line
    /// and starts rotating this object owards it. 
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveWait()
    {
        yield return new WaitForSeconds(waitTimeMove);
        if (currentPointIndex + 1 < patrolPoints.Length)
            currentPointIndex++;
        else
            currentPointIndex = 0;
        stage = 5;
    }

    /// <summary>
    /// The method that handles the intial effects of knockback effects. It applies the impulse force for the object
    /// and pushes this object away from what is causing the knockback.
    /// </summary>
    /// <param name="force">The amount of force applied to this object</param>
    /// <param name="tf">The position of the object applying knockback to this</param>
    public void Knockback(float force, Vector3 tf)
    {
        StopAllCoroutines(); //Needs to have these reset so that things don't break. 
        attackCollider.SetActive(false);
        knockback = true;
        stage = 3;
        Vector3 direction = transform.position - tf;
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        Debug.Log("Hit");
    }

    /// <summary>
    /// This method checks to see if this object is no longer effected by any force. If it isn't, reset this object
    /// and have it start rotating back towards it's patrol point. 
    /// </summary>
    private void KBCheck()
    {
        if (rb.velocity == new Vector3(0, 0, 0))
        {
            knockback = false;
            mWait = false;
            attackCD = false;
            stage = 5;
        }
    }

    /// <summary>
    /// If attack is not on cooldown, this script allows the enemy to attack
    /// </summary>
    private void Attack()
    {
        if (!attackCD)
        {
            attackCollider.SetActive(true); 
            //An Animator.play script will likely fit here
            attackCD = true;
            StartCoroutine(AttackWait());
            StartCoroutine(ColliderFrames());
        }
    }

    /// <summary>
    /// Goes about setting up what happens when this object dies. Turns everything off, adds the relevant score, and
    /// set up Respawn point. Enjoy purgatory
    /// </summary>
    private void Death()
    {
        if (alive)
        {
            alive = false;
            gameObject.GetComponent<BoxCollider>().enabled = false; //Double check that it's a box collider!!!!!!
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gm.ChangeScore(pointsWorth);
            StartCoroutine(Respawning());
            stage = 6;
        }

    }

    /// <summary>
    /// An IEnumerator for when this dies. Once it waits for the appropriate amount of time
    /// place this in the appropriate position with everything back alive. Begin rotating towards
    /// your next point once again.
    /// </summary>
    /// <returns></returns>
    IEnumerator Respawning()
    {
        yield return new WaitForSeconds(respawnTime);
        transform.position = respawnPoint.position;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        alive = true;
        stage = 5;
    }

    /// <summary>
    /// An IEnumerator that handles the cooldown on the enemy attack. Attacking every frame gets a little to hectic.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackWait()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackCD = false;
        
    }

    /// <summary>
    /// An IEnumerator that handles how long the attack lasts for. Perfect for a test object, though likely will
    /// be removed when an animator is used as it'll be easier to control the collider through that. 
    /// </summary>
    /// <returns></returns>
    IEnumerator ColliderFrames()
    {
        yield return new WaitForSeconds(1);
        attackCollider.SetActive(false);
    }

    /// <summary>
    /// When this hits one of many different types of dangerous object, including another enemy, kill this. 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("killBox") || other.CompareTag("deathFloor"))
        {
            Death();
        } 
        else if (other.CompareTag("enemy") && other.GetComponent<EnemyBehavior>() != null)
        {
            if (knockback || other.GetComponent<EnemyBehavior>().getKnockback())
            {
                Death();
            }
        }
        //Can add a bounce function here to get a ping pong effect going
    }

    /// <summary>
    /// Returns the objects knockback variable when called
    /// </summary>
    /// <returns>knockback</returns>
    public bool getKnockback()
    {
        return knockback;
    }
}
