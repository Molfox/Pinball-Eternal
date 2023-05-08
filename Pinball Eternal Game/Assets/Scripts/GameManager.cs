using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool cursorLocked = true;
    [SerializeField] bool timerOn = true;
    [SerializeField] float timer = 100f;
    [SerializeField] int score = 0;
    [SerializeField] int combo;
    [SerializeField] float comboTimer = 5f;

    [SerializeField] GameObject scoreText;
    [SerializeField] GameObject comboText;
    [SerializeField] GameObject timerText;

    // Start is called before the first frame update
    void Start()
    {
        if (cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        comboText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TimerManager();
    }

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
