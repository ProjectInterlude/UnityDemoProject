using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerHuntExample : MonoBehaviour
{
    bool firedPrompt = false;
    public bool fireDisplayKey = false;

    // Update is called once per frame
    void Update()
    {
        if(!firedPrompt && Time.time > 5)
        {

            PauseGame();
            Interlude.ScavengerHunt.DisplayPrompt();
            firedPrompt = true;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        Debug.Log("resuming");
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
