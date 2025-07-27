using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    Animation anim;
    private void Start()
    {
        anim = GetComponent<Animation>();
    }
    public void Play()
    {
        if (anim != null) {
            anim.Play();
        }
    }
}
