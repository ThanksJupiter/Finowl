using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
    [System.Serializable]
    public class Heart
    {
        public bool filled;
        public Image outlineImage;
        public Image fillImage;
    }

    public Color heartRemovedColor;
    public float fillAmount = 1f;
    public Heart[] hearts = new Heart[3];

    private Color initialColor;
    public int currentHeart = 2;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        initialColor = hearts[0].fillImage.color;
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].filled = true;
        }
    }

    public void SetImageFillAmount(int index, float value)
    {
        hearts[index - 1].fillImage.fillAmount = value;
    }

    public void ResetImageFillAmount(int index)
    {
        hearts[index - 1].fillImage.fillAmount = 0;
    }

    public void FillImage(int index, float amount)
    {
        hearts[index - 1].fillImage.fillAmount += amount;
    }

    public void TriggerHeartPulse(int index)
    {
        if (index != 0)
        {
            animator.SetTrigger(string.Format("Pulse{0}", index - 1));
        }
    }

    public void TriggerHeartDecay(int index)
    {
        animator.SetTrigger(string.Format("Decay{0}", index));
    }

    public void RemoveHeart()
    {
        /*switch (currentHeart)
        {
            case 2:
                UnfillHeart(currentHeart);
                currentHeart--;
                break;
            case 1:
                UnfillHeart(currentHeart);
                currentHeart--;
                break;
            case 0:
                UnfillHeart(currentHeart);
                break;
            default:
                break;
        }*/
    }

    public void RestoreHeart()
    {
        /*switch (currentHeart)
        {
            case 2:
                FillHeart(currentHeart);
                break;
            case 1:
                if (CurrentHeartFilled())
                {
                    currentHeart++;
                    FillHeart(currentHeart);
                    break;
                }
                else
                {
                    FillHeart(currentHeart);
                    currentHeart++;
                }
                break;
            case 0:
                if (CurrentHeartFilled())
                {
                    currentHeart++;
                    FillHeart(currentHeart);
                    break;
                }
                else
                {
                    FillHeart(currentHeart);
                    currentHeart++;
                }
                break;
            default:
                break;
        }*/
    }

    private void FillHeart(int index)
    {
        StopCoroutine("SlowUnfillHeart");
        StartCoroutine("SlowFillHeart", index);
        //hearts[index].fillImage.color = initialColor;
        //hearts[index].filled = true;
    }

    private void UnfillHeart(int index)
    {
        hearts[index].filled = false;
        StopCoroutine("SlowFillHeart");
        StartCoroutine("SlowUnfillHeart", index);
        //hearts[index].fillImage.color = heartRemovedColor;
        //hearts[index].filled = false;
    }

    private IEnumerator SlowFillHeart(int index)
    {
        while (hearts[index].fillImage.fillAmount <= 1)
        {
            hearts[index].fillImage.fillAmount += fillAmount * Time.deltaTime;

            if (hearts[index].fillImage.fillAmount >= 1)
            {
                hearts[index].filled = true;
            }

            yield return null;
        }
    }

    private IEnumerator SlowUnfillHeart(int index)
    {
        while (hearts[index].fillImage.fillAmount >= 0)
        {
            hearts[index].fillImage.fillAmount -= fillAmount * Time.deltaTime;

            if (hearts[index].fillImage.fillAmount <= 0)
            {
                hearts[index].filled = false;
            }

            yield return null;
        }
    }

    private bool CurrentHeartFilled()
    {
        return hearts[currentHeart].filled;
    }

    public void Reset()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].fillImage.color = initialColor;
        }
        currentHeart = 2;
    }
}
