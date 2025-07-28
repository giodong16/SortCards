using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinDialog : MonoBehaviour
{
    [SerializeField] private float delayBeforeNextLevel = 1f;


    private void Start()
    {
        StartCoroutine(NextLevel());
    }
    IEnumerator DelayHide(float duration)
    {
        yield return new WaitForSeconds(duration);
        UIManager.Instance.HideUI(CanvasName.Canvas_Win);     
    }
    IEnumerator NextLevel()
    {
        yield return StartCoroutine(DelayHide(delayBeforeNextLevel));
        GameManager.Instance.Next();
    }
    
}
