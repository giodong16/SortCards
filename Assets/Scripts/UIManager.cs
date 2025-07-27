using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TMP_Text moneyText;
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
            DontDestroyOnLoad(gameObject);
        }
        else 
        { 
            Destroy(gameObject);
            
        }
        InitializeDictionary();
    }
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
   /*      HideAllUI(); // Ẩn toàn bộ UI khi Scene mới được load
         ShowCanvas(CanvasName.Canvas_GamePlay); 
         cooldownUIs.ResetAllCooldownUI();*/
    }

    private void ShowCanvas(CanvasName canvasName)
    {
/*        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Home"))
        {
            ShowUI(CanvasName.Canvas_Home);
        }
        else
        {
            ShowUI(CanvasName.Canvas_GamePlay);
            LevelTimer levelTimer = FindObjectOfType<LevelTimer>();
            if (levelTimer != null) { 
                levelTimer.StartTimer();
            }
        }*/
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
}
public enum CanvasName
{
    Canvas_Home, Canvas_GamePlay, Canvas_GamePause, Canvas_GameOver, Canvas_Win, Canvas_SelectLevel
}