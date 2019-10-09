using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartSystem : MonoBehaviour
{
   
    public static void WinState()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // win - load cool endscreen
    }

    public static void LoseState()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); //lose - reset stats and go back to cart
    }
    public static void ReloadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); //Restart everything
    }

}
