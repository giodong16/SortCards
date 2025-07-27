using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Color cardColor;

    public void SetUp()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material = new Material(meshRenderer.material);
            meshRenderer.material.color = cardColor;
        }
    }
    private void OnMouseDown()
    {
        if (GameManager.Instance.currentState != GameState.Playing) return;
        if (GameManager.Instance.isInteracting) return;

        GameManager.Instance.OnColumnClicked(GetComponentInParent<Column>());
    }

}

