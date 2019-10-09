using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutPopUps : MonoBehaviour
{
    private bool tutDone;
    public CanvasGroup uiElement;

    private void Start()
    {
        tutDone = false;
        uiElement.enabled = false;
        Debug.Log("FALSE!");
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 1));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0));
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
    {

        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while(true)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && uiElement.enabled)
        {
            Closetut();
            FadeOut();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Capsule" && tutDone == false)
        {
            FadeIn();
        }
    }


    private void Closetut()
    {
        tutDone = true;
        Debug.Log("Closed!");
    }



}
