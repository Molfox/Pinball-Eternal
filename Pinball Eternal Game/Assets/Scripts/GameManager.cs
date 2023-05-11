using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Tooltip("Do you want the cursor to be locked and invisible while the game is playing?")]
    [SerializeField] bool cursorLocked = true;
    [Tooltip("Is the timer for the game on?")]
    [SerializeField] bool timerOn = true;
    [Tooltip("How much time the players have")]
    [SerializeField] float timer = 100f;
    [Tooltip("What is the player's current score")]
    [SerializeField] int score = 0;
    [Tooltip("What is the player's current combo")]
    [SerializeField] int combo;
    [Tooltip("How long should the combo meter last")]
    [SerializeField] float comboTimer = 5f;

    [Tooltip("The gameobject that houses the TMP for the score")]
    [SerializeField] GameObject scoreText;
    [Tooltip("The gameobject that houses the TMP for the combo")]
    [SerializeField] GameObject comboText;
    [Tooltip("The gameobject that houses the TMP for the timer")]
    [SerializeField] GameObject timerText;

    // Start is called before the first frame update
    void Start()
    {
        //If cursorLocked is true, then lock the cursor to center of screen and make it invisible.
        if (cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        comboText.SetActive(false); //Make sure that comboText isn't visible when the game starts
    }

    // Update is called once per frame
    void Update()
    {
        TimerManager();
    }

    /// <summary>
    /// This method keeps track of the timer in the game.
    /// </summary>
    private void TimerManager()
    {
        if (timerOn)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                timerText.GetComponent<TextMeshPro>().text = Mathf.FloorToInt(timer).ToString();
            } 
            else
            {
                timer = 0;
                //Build a proper Game End Screen
                Debug.Log("Game End");
                Debug.Log("Score: " + score);
            }
        }
    }

    public void ChangeScore(int addScore)
    {
        combo++;
        if (combo > 1)
        {
            StopAllCoroutines();
            comboText.SetActive(true);
            comboText.GetComponent<TextMeshPro>().text = "X" + combo.ToString();
        }
        score += addScore * combo;
        scoreText.GetComponent<TextMeshPro>().text = score.ToString();
        StartCoroutine(ComboTimer());
    }

    IEnumerator ComboTimer()
    {
        yield return new WaitForSeconds(comboTimer);
        comboText.SetActive(false);
        combo = 0;
    }
}
