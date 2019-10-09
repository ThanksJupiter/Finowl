using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TorchIndicator : MonoBehaviour
{
    public Canvas canvas;
    public Image[] img;
    public Image[] particles;
    public void SetImageFillAmount(int index, float value)
    {
        img[index - 1].fillAmount = value;
    }

    public void ResetImageFillAmount(int index)
    {
        img[index - 1].fillAmount = 0;
    }

    public void FillImage(int index, float amount)
    {
        img[index - 1].fillAmount += amount;
    }
    public void PlayVfx(int index)
    {
        particles[index - 1].gameObject.SetActive(false);
        particles[index - 1].gameObject.SetActive(true);
    }
    private void Update()
    {
        if (particles[0].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !particles[0].GetComponent<Animator>().IsInTransition(0))
        {
            particles[0].gameObject.SetActive(false);
        }
        if (particles[1].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !particles[1].GetComponent<Animator>().IsInTransition(0))
        {
            particles[1].gameObject.SetActive(false);
        }
        if (particles[2].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !particles[2].GetComponent<Animator>().IsInTransition(0))
        {
            particles[2].gameObject.SetActive(false);
        }
    }
}
