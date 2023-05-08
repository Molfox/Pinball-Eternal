using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] float iFramesTime = 2f;

    bool iFrames = false;

    PlayerMovement pmReference;

    [SerializeField] Image[] healthUI;
    

    //Create a code that keeps track of health and watches to see if
    //the player collides with an Enemy attack or a killbox.
    //Reset if health hits 0 or falls into the kill plain

    // Start is called before the first frame update
    void Start()
    {
        pmReference = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!iFrames && (other.CompareTag("enemyAttack") || other.CompareTag("killBox")))
        {
            TakeDamage(1);
        }
        else if(other.CompareTag("deathFloor"))
        {
            transform.position = pmReference.getLastSolidGround();
            TakeDamage(1);
        }
    }

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
                //GameOver;
            }
        }
        
    }

    IEnumerator iFrameWait()
    {
        yield return new WaitForSeconds(iFramesTime);
        iFrames = false;
    }

}
