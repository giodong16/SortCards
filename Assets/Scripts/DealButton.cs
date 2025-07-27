using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealButton : MonoBehaviour
{
    public Transform spawnTransform;

    private void OnMouseDown()
    {
        if (GameManager.Instance.currentState != GameState.Playing) return;
        if (GameManager.Instance.isDealing || GameManager.Instance.isInteracting) return;

        StartCoroutine(GameManager.Instance.DealCards(spawnTransform));
    }
}
