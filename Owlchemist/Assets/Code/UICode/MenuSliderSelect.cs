using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuSliderSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    public Image myImage;

    public void OnSelect(BaseEventData eventData)
    {
        myImage.gameObject.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        myImage.gameObject.SetActive(false);
    }
}