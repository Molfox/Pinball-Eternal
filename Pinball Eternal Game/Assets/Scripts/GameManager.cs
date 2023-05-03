using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool cursorLocked = true;
    [SerializeField] float score;

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

    public void changeScore(float addScore)
    {

    }
}
