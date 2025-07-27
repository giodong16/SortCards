using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public RectTransform rootLevelMenu;
    public GameObject levelButtonPrefabs;

    private void Start()
    {
        RefreshLevelMenu();
    }
    private void OnEnable()
    {
        RefreshLevelMenu();
    }
    public void CreateLevelMenu()
    {
        List<LevelData> datas = GameManager.Instance?.levelDatas;
        if (datas == null || datas.Count == 0) return;

        for (int i = 0;i<datas.Count;i++) { 
            GameObject levelButton = Instantiate(levelButtonPrefabs, rootLevelMenu);
            LevelButton button = levelButton.GetComponent<LevelButton>();
            button.Level = i+1;
            if (i + 1 <= Pref.UnlockLevel) {
                button.GetComponent<Button>().interactable = true;
            }
            else
            {
                button.GetComponent<Button>().interactable = false;
            }
        }
    }
    private void RefreshLevelMenu()
    {
        foreach (RectTransform child in rootLevelMenu)
        {
            Destroy(child.gameObject);
        }

        CreateLevelMenu();
    }

}
