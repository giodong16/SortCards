using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TMP_Text moneyText;
    public TMP_Text currentLevelText;
    public ProgessBar levelProgessBar;
    private Coroutine levelCoroutineProgess;
    [System.Serializable]
    public class UIObject
    {
        public CanvasName canvasName;
        public UIEffect uiEffect;
    }

    public List<UIObject> uis = new List<UIObject>();
    private Dictionary<CanvasName, UIEffect> uiDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           
        }
        InitializeDictionary();
    }
    private void Start()
    {
        currentLevelText.text ="Level "+ Pref.CurrentLevel.ToString();
        levelProgessBar.Show(true);
        UpdateMoneyUI(Pref.Money);
    }
    private void InitializeDictionary()
    {
        uiDictionary = new Dictionary<CanvasName, UIEffect>();
        foreach(var ui in uis)
        {
            ui.uiEffect.SetCanvasGroup(false);
            uiDictionary.Add(ui.canvasName,ui.uiEffect); 
        }
    }

    public void ShowUI(CanvasName canvasName)
    {
        if (uiDictionary.ContainsKey(canvasName))
        {
            uiDictionary[canvasName].Show();
        }
    }

    public void HideUI(CanvasName canvasName) {
        if (uiDictionary.ContainsKey(canvasName))
        {
            uiDictionary[canvasName].Hide();
        }
    }

    public void HideAllUI()
    {
        foreach(var ui in uiDictionary.Values)
        {
            ui.gameObject.SetActive(false);
        }
    }

    public void UpdateMoneyUI(int money)
    {
        moneyText.text = money.ToString();
    }
    public void UpdateProgessBar(int amount, int total)
    {
        if (levelCoroutineProgess != null) {
            StopCoroutine(levelCoroutineProgess);
        }
        levelCoroutineProgess = StartCoroutine(levelProgessBar.FillProgressBar(amount,total));
    }
}
public enum CanvasName
{
    Canvas_Home, Canvas_GamePlay, Canvas_GamePause, Canvas_GameOver, Canvas_Win, Canvas_SelectLevel
}