using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseInventorySystem : MonoBehaviour
{
    private GameObject player;
    public Image[] sprites;
    public List<Text> amountTexts = new List<Text>();
    public Sprite StandardSprite;
    private InventorySlot[] slots;
   // private Vector3 slotSize;

    private void Start()
    {
        List<Image> tempSprites = new List<Image>();
        player = GameObject.FindObjectOfType<MovementComponent>().gameObject;
        slots = GetComponentsInChildren<InventorySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            tempSprites.Add(slots[i].GetComponent<Image>());
        }
        sprites = tempSprites.ToArray();//GetComponentsInChildren<Image>();
       // slotSize = slots[0].GetComponent<RectTransform>().sizeDelta;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].image.sprite = StandardSprite;
    //        slots[i].GetComponent<RectTransform>().sizeDelta = slotSize;
        }
    }
    public void UpdateInventory(CollectibleComponent br)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].image.sprite == br.baseCollectible.GuiSprite)
            {
                slots[i].amount += br.Quantity;
                slots[i].amountTXT.text = slots[i].amount.ToString();
                
         //       slots[i].GetComponent<RectTransform>().sizeDelta = slotSize;
                return;
            }

        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].image.sprite == StandardSprite)
            {
                slots[i].image.sprite = br.baseCollectible.GuiSprite;
                slots[i].amount += br.Quantity;
                slots[i].amountTXT.text = slots[i].amount.ToString();
       //         slots[i].GetComponent<RectTransform>().sizeDelta = slotSize;

                return;

            }
            //else if (slots[i].image.sprite == br.baseCollectible.GuiSprite)
            //{
            //    slots[i].amount += br.Quantity;
            //    slots[i].amountTXT.text = slots[i].amount.ToString();
            //    break;
            //}

        }
    }
    public void RemoveFromInventoryUI(CollectibleComponent br)
    {
        int slotIndex = 10;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].image.sprite == br.baseCollectible.GuiSprite)
            {
                slotIndex = i;
            }
        }
        if (slotIndex > 6)
        {

        }
        else
        {
            if (slots[slotIndex].image.sprite != StandardSprite && slotIndex < slots.Length)
            {
                slots[slotIndex].amount -= 1;
                if (slots[slotIndex].amount < 1)
                {

                    sprites[slotIndex].sprite = StandardSprite;
         //           slots[slotIndex].GetComponent<RectTransform>().sizeDelta = slotSize;
                }

                slots[slotIndex].amountTXT.text = slots[slotIndex].amount.ToString();

                if (slots[slotIndex].amount < 1)
                {
                    slots[slotIndex].amountTXT.text = "";
       //             slots[slotIndex].GetComponent<RectTransform>().sizeDelta = slotSize;

                }

            }
        }

    }
/*
    public void ResetInventory()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].sprite = StandardSprite;
            slots[i].amountTXT.text = "";
            slots[i].amount = 0;
        }
    }*/


}