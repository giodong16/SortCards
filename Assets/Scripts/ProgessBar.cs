using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgessBar : MonoBehaviour
{
    public Image fillImage;
    public float durationFill = 0.3f;
    public bool isFilling  = false;

    public Coroutine delayHideCoroutine;
    public Coroutine fillCoroutine;
    private void Awake()
    {
        Show(false);
    }
    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
        if (!isShow)
        {
            fillImage.fillAmount = 0;
        }
    }

    public void UpdateProgress(int currentCard, int totalCard, bool isPush)
    {
        if (currentCard <= 0)
        {
            if (isPush) {
                if (delayHideCoroutine != null) { StopCoroutine(delayHideCoroutine); }
                delayHideCoroutine = StartCoroutine(DelayHideProgressBar(durationFill));
            }
            else
            {
                Show(false);
            }
          
        }
        else
        {
            Show(true);
            if(fillCoroutine != null) StopCoroutine(fillCoroutine);
            fillCoroutine = StartCoroutine(FillProgressBar(currentCard, totalCard));
        }
    }

    public IEnumerator FillProgressBar(int currentCard, int totalCard)
    {
        isFilling = true;
        float startFill = fillImage.fillAmount;
        float endFill = Mathf.Clamp01((float)currentCard / totalCard);
        float timeCount = 0f;

        while (timeCount < durationFill)
        {
            timeCount += Time.deltaTime;
            float t = timeCount / durationFill;
            fillImage.fillAmount = Mathf.Lerp(startFill, endFill, t);
            yield return null;
        }

        fillImage.fillAmount = endFill;
        isFilling = true;
    }

    private IEnumerator DelayHideProgressBar(float delay)
    {
        yield return new WaitForSeconds(delay);
        Show(false);
    }
}
