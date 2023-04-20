using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] int health;

    //Create a code that keeps track of health and watches to see if
    //the player collides with an Enemy attack or a killbox.
    //Reset if health hits 0 or falls into the kill plain

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemyAttack") || other.CompareTag("killBox"))
        {
            health--;
        }
        else if(other.CompareTag("deathFloor"))
        {
            //respawn or restart?
        }
    }


}
