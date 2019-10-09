using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SisterLetter : MonoBehaviour
{
    private PlayerFilter player;

    public void OpenLetter(PlayerFilter player)
    {
        this.player = player;
        gameObject.SetActive(true);
        player.gameManagerComponent.OnStopGameTick();
        player.inputComponent.OnBButtonDown += ResumeGame;
    }

    private void ResumeGame()
    {
        player.gameManagerComponent.OnStartGameTick();
        gameObject.SetActive(false);
    }
}
