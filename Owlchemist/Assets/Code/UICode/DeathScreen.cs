using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public CartInteraction cartInteraction;

    private PlayerFilter player;

    public void SubscribeToButtonEvents(PlayerFilter player)
    {
        this.player = player;
        player.movementComponent.alive = false;
        player.inputComponent.OnAButtonDown += Respawn;
        player.inputComponent.OnBButtonDown += QuitGame;
    }

    private void Respawn()
    {
        cartInteraction.PlayerDeath();
        player.inputComponent.OnAButtonDown -= Respawn;
        player.inputComponent.OnBButtonDown -= QuitGame;
        player.gameManagerComponent.isInsideCart = true;
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
       
        SceneManager.LoadScene(0);
        /*player.inputComponent.OnBButtonDown -= QuitGame;
        player.inputComponent.OnAButtonDown -= Respawn;
        player.animationComponent.animator.speed = 1f;
        */player.gameManagerComponent.OnStartGameTick();
    }

}
