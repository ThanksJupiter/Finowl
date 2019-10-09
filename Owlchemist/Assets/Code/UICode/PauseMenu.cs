using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Button defaultButton;
    public GameObject PauseScreen;
    public PlayerFilter player;

    public void SubscribeToPause(PlayerFilter player)
    {
        this.player = player;
        PauseScreen.GetComponent<ButtonSelectMenu>().ReDoPlayer();
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
