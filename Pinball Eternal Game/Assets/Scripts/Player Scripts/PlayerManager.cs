/***
 * PlayerManager.cs
 * By Nathan Boles
 * 
 * This script keeps track fo the basic functionalitiy of the player character.
 * This includes keeping track of player health, iFrames, and what happens if the player falls off
 * somewhere. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerManager : MonoBehaviour
{
    [Tooltip("How much health does the player have")]
    [SerializeField] int health;
    [Tooltip("How long is the player untouchable after being hit")]
    [SerializeField] float iFramesTime = 2f;

    bool iFrames = false;

    PlayerMovement pmReference;
    CharacterController cc;
    GameManager gm;

    [Tooltip("Images used for player health")]
    [SerializeField] Image[] healthUI; //Might change this from a serialize field into something that is set via script
    

    //Create a code that keeps track of health and watches to see if
    //the player collides with an Enemy attack or a killbox.
    //Reset if health hits 0 or falls into the kill plain

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        gm = GetComponent<GameManager>();
        pmReference = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// When this object enters another trigger, it'll check to see if it's an enemy attack or killBox
    /// If it is, TakeDamage is called, if not, check to see if it is a death floor. If it is, teleport the player
    /// to the last safe spot, and TakeDamage afterwards. 
    /// </summary>
    /// <param name="other">Trigger Collider this object entered</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!iFrames && (other.CompareTag("enemyAttack") || other.CompareTag("killBox")))
        {
            TakeDamage(1);
        }
        else if(other.CompareTag("deathFloor"))
        {
            cc.enabled = false;
            transform.position = pmReference.getLastSolidGround();
            cc.enabled = true;
            TakeDamage(1);
        }
    }

    /// <summary>
    /// When called, this method will subtract from the player's health. To do this, it'll turn on iFrames
    /// so that multiple hits won't happen at once, then it'll use a for loop to reduce health and the image 
    /// via how much damage happens.
    /// </summary>
    /// <param name="damage">The amount of damage happening to the character</param>
    private void TakeDamage(int damage)
    {
        iFrames = true;
        StartCoroutine(iFrameWait());
        for (int i = 0; i < damage;  i++)
        {
            health--;
            healthUI[health].enabled = false;
            if (health == 0)
            {
                i = damage;
                gm.GameEnd();
            }
        }
        
    }

    /// <summary>
    /// This IEnumerator basically waits for however long iFrames last after being called, then turns it off. 
    /// </summary>
    /// <returns></returns>
    IEnumerator iFrameWait()
    {
        yield return new WaitForSeconds(iFramesTime);
        iFrames = false;
    }

}
