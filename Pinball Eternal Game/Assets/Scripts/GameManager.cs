/***
 * GameManger.cs
 * By Nathan Boles
 * 
 * This script manages all the basic functionalities of the game. 
 * Including things like the score and timer. 
 * 
 */
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

    [Tooltip("The gameobject that houses the TMP for the score on the game over screen")]
    [SerializeField] GameObject goScoreText;

    [Tooltip("The Status Screen")]
    [SerializeField] GameObject statusScreen;
    [Tooltip("The Game Over Screen")]
    [SerializeField] GameObject gameOverScreen;
    

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
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        TimerManager();
    }

    /// <summary>
    /// This method keeps track of the timer in the game.
    /// If the timer is active, then it will appear on the canvas screen. Each frame, it will subtract that amount
    /// of time from variable timer, then display that as an int on the timer on screen.
    /// When the timer runs out, the Game End screen will show to give the player their highscore this playthrough.
    /// </summary>
    private void TimerManager()
    {
        if (timerOn)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                timerText.GetComponent<TextMeshProUGUI>().text = Mathf.FloorToInt(timer).ToString();
            } 
            else
            {
                timer = 0;
                GameEnd();
                //Build a proper Game End Screen
                Debug.Log("Game End");
                Debug.Log("Score: " + score);
            }
        }
    }


    /// <summary>
    /// This method adds to score when it is called upon.
    /// This script first adds a bonus to a combo, and if that combo is greater then 2, it'll appear on the screen.
    /// Then it uses that combo, multiplying it by addscore, to add that number to the players current score and
    /// making it appear on screen. Afterwards it starts a coroutine which tracks the combo timer.
    /// </summary>
    /// <param name="addScore">The initial score to be added when this is called</param>
    public void ChangeScore(int addScore)
    {
        combo++;
        if (combo > 1)
        {
            StopAllCoroutines(); //It'll be reset later in the script
            comboText.SetActive(true);
            comboText.GetComponent<TextMeshProUGUI>().text = "X" + combo.ToString();
        }
        score += addScore * combo;
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
        StartCoroutine(ComboTimer());
    }

    /// <summary>
    /// This IEnumerator will wait however long the comboTimer is and then resets combo.
    /// </summary>
    /// <returns></returns>
    IEnumerator ComboTimer()
    {
        yield return new WaitForSeconds(comboTimer);
        comboText.SetActive(false);
        combo = 0;
    }

    public void GameEnd()
    {
        statusScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        goScoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }
}
