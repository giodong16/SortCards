using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingVolumeButton : MonoBehaviour
{
    public Sprite turnOn;
    public Sprite turnOff;
    bool isTurnOn;
    public Image image;
    private void Start()
    {
        if(Pref.Volume == 0)
        {
            isTurnOn = false;
        }
        else
        {
            isTurnOn=true;
        }
        SettingVolume();
    }
    public void Switch()
    {
        isTurnOn = !isTurnOn;
        SettingVolume();
    }
    public void SettingVolume()
    {
     
        if(isTurnOn)
        {
            image.sprite = turnOn;
            Pref.Volume = 1;
        }
        else
        {
            image.sprite = turnOff;
            Pref.Volume = 0;
        }
        AudioManager.Instance.SettingVolume();
    }
}
