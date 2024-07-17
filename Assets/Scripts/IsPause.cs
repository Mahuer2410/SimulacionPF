using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPause : MonoBehaviour
{
    private bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }
}
