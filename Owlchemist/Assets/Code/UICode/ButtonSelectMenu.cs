using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelectMenu : MonoBehaviour
{
    [SerializeField] Button selected;
    public GameObject lastpage;
    public PauseMenu pausesScrenManager;
    private PlayerFilter player;

    private void OnEnable()
    {
        this.player = pausesScrenManager.player;
        if (selected)
        {
             EventSystem.current.SetSelectedGameObject(selected.gameObject);
             selected.OnSelect(null);
        }
        player.inputComponent.OnBButtonDown += ReturnToMenu;
    }
    private void OnDisable()
    {
        player.inputComponent.OnBButtonDown -= ReturnToMenu;
    }
    public void ReturnToMenu()
    {
        if (!lastpage)
        {
            player.animationComponent.animator.speed = 1f;
            player.gameManagerComponent.OnStartGameTick();
            pausesScrenManager.gameObject.SetActive(false);
            /*Debug.Log("Hercules");
            lastpage.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Melvin, Melvin");
            player.animationComponent.animator.speed = 1f;
            player.gameManagerComponent.OnStartGameTick();
            pausesScrenManager.gameObject.SetActive(false);*/
        }
    }
    public void ReDoPlayer()
    {
        this.player = pausesScrenManager.player;
        player.inputComponent.OnBButtonDown += ReturnToMenu;
    }

}