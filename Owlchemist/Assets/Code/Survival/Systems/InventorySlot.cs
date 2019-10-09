using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int amount = 0;
    public Text amountTXT;
    public Image image;

    private void Awake()
    {
        amountTXT = GetComponentInChildren<Text>();
        image = GetComponent<Image>();
    }
}
