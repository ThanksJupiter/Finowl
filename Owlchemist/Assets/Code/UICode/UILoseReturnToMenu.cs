using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoseReturnToMenu : MonoBehaviour
{
    //public CartInteraction cartRef;
    private GameObject playerRef;
    void QuitGame()
    {
        playerRef.GetComponent<InputComponent>().OnAButtonDown -= QuitGame;
        playerRef.GetComponent<AnimationComponent>().animator.speed = 1f;
        playerRef.GetComponent<GameManagerComponent>().OnStartGameTick();
        SceneManager.LoadScene(0);
    }
    private void Awake()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerRef.GetComponent<MovementComponent>().alive = false;
        playerRef.GetComponent<InputComponent>().OnAButtonDown += QuitGame;
    }
}
