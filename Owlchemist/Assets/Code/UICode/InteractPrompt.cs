using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractPrompt : MonoBehaviour
{
    public Text interactTXT;
    public Image buttonIMG;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
