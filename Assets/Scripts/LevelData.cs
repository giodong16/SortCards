using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewLevel",menuName = "Game/New Level Data")]
public class LevelData : ScriptableObject
{
    public int level;
    public int maxColor;
    public int sameColorGroupMin = 2;
    public int sameColorGroupMax = 6;
    public int initialDealCount = 16;
    public int dealBonusPerRound = 1;
    public int totalCardsRequire;

}
