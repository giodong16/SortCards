using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pref
{
 #region Datas
    public static int Money
    {
        set
        {
            PlayerPrefs.SetInt(Const.MONEY, value);
        }
        get
        {
            return PlayerPrefs.GetInt(Const.MONEY, 0);
        }
    }

    public static int Volume
    {
        set
        {
            PlayerPrefs.SetInt(Const.VOLUME, value);
        }
        get
        {
            return PlayerPrefs.GetInt(Const.VOLUME, 1);
        }
    }

    public static int UnlockLevel
    {
        set
        {
            int old = PlayerPrefs.GetInt(Const.UNLOCKLEVEL);
            if(old < value)
            {
                PlayerPrefs.SetInt(Const.UNLOCKLEVEL, value);
            }
        }
        get
        {
            return PlayerPrefs.GetInt(Const.UNLOCKLEVEL,1);
        }
    }

    public static int CurrentLevel
    {
        set => PlayerPrefs.SetInt(Const.CURRENTLEVEL, value);
        get => PlayerPrefs.GetInt(Const.CURRENTLEVEL, 1);
    }
#endregion Datas


}
public class Const
{

    public static string MONEY = "Money";
    public static string VOLUME = "Volume";
    public static string UNLOCKLEVEL = "UnlockLevel";
    public static string CURRENTLEVEL = "CurrentLevel";

}