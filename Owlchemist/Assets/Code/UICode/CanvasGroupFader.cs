using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFader : MonoBehaviour
{
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;
    public bool visible;

    private float alpha = 0f;
    private CanvasGroup cg;

    public FadeState currentState;
    public enum FadeState
    {
        None,
        FadeIn,
        FadeOut
    }

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = alpha;
        //gameObject.SetActive(false);
        currentState = FadeState.None;
    }

    private void Update()
    {
        switch (currentState)
        {
            case FadeState.None:
                break;
            case FadeState.FadeIn:
                IncreaseCanvasAlpha(fadeInTime);
                break;
            case FadeState.FadeOut:
                DecreaseCanvasAlpha(fadeOutTime);
                break;
            default:
                break;
        }
    }

    public void Display(bool instantDisplay)
    {
        gameObject.SetActive(true);
        if (instantDisplay)
        {
            cg.alpha = 1f;
        }
        else
        {
            currentState = FadeState.FadeIn;
            //StartCoroutine(FadeIn());
        }
    }

    public void Hide(bool instantHide)
    {
        if (instantHide == true)
        {
            gameObject.SetActive(false);
            cg.alpha = 0f;
        }
        else
        {
            currentState = FadeState.FadeOut;
            //StartCoroutine(FadeOut());
        }
    }

    public IEnumerator FadeIn()
    {
        while (alpha <= 1)
        {
            IncreaseCanvasAlpha(fadeInTime);
            yield return null;
        }

        visible = true;
    }

    public IEnumerator FadeOut()
    {
        while (alpha >= 0)
        {
            DecreaseCanvasAlpha(fadeOutTime);
            yield return null;
        }

        gameObject.SetActive(false);
        visible = false;
    }

    private void IncreaseCanvasAlpha(float amount)
    {
        if (alpha <= 1)
        {
            alpha += amount * Time.deltaTime;
            cg.alpha = alpha;
        }
        else
        {
            visible = true;
            currentState = FadeState.None;
        }
    }

    private void DecreaseCanvasAlpha(float amount)
    {
        if (alpha >= 0)
        {
            alpha -= amount * Time.deltaTime;
            cg.alpha = alpha;
        }
        else
        {
            visible = false;
            currentState = FadeState.None;
        }
    }
}
