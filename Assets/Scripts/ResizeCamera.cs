using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCamera : MonoBehaviour
{
    void Start()
    {
        Camera cam = Camera.main;

        float targetAspect = 9f / 16f; 
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            cam.orthographicSize = cam.orthographicSize / scaleHeight;
        }
    }

}
