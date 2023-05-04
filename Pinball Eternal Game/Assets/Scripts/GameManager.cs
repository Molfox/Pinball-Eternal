using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool cursorLocked = true;
    [SerializeField] int score = 0;
    [SerializeField] int combo;
    [SerializeField] float comboTimer = 5f;

    // Start is called before the first frame update
    void Start()
    {
        if (cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScore(int addScore)
    {
        if (combo > 1)
        {
            StopAllCoroutines();
        }
        combo++;
        score += addScore * combo;
        StartCoroutine(ComboTimer());
    }

    IEnumerator ComboTimer()
    {
        yield return new WaitForSeconds(comboTimer);
        combo = 0;
    }
}
