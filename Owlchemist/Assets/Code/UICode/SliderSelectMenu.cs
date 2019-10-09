using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderSelectMenu : MonoBehaviour
{
    [SerializeField] Slider selected;
    public Image myImage;

    private void OnEnable()
    {
        myImage.gameObject.SetActive(true);
        selected.OnSelect(null);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected.gameObject);
    }
}