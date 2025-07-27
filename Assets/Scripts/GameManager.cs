using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    Start,
    Playing,
    Pause,
    Gameover,
    Win,

}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Stats")]
    public GameState currentState;

    [Header("Gameplay")]
    public List<Column> columns;
    public List<Color> colors;
    public int totalMoney = 0;
    public bool isDealing =false;
    public bool isInteracting =false;

    public GameObject cardPrefab;
    private List<Card> selectedGroup;
    private Column selectedColumn;

    [Header("Level Datas")]
    public List<LevelData> levelDatas;

    [Header("Data Per Level")]
    public int currentLevel;
    public LevelData currentData;
    public int cardGetPerLevel = 0;
    public int maxColor;
    public int currentDealCount;
    public int currentUnlockCost = 5;
    public int preUnlockCost = 3;


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
        
    }
    
    public void AddMoney(int m)
    {
        Pref.Money += m;

        cardGetPerLevel += m;

        UIManager.Instance.UpdateMoneyUI(Pref.Money);
        if (IsWin())
        {
            Win();
        }

    }
    public void CreateUnlockCost()
    {
        int temp = currentUnlockCost;
        currentUnlockCost += preUnlockCost;
        preUnlockCost = temp;
    }
    public void UpdateAllColumnsCost()
    {
        foreach(Column col in columns)
        {
            col.UpdateCostUI();
        }
    }

    public void OnColumnClicked(Column column)
    {
        if (selectedGroup == null)
        {
            if (column.cards.Count > 0)
            {
                selectedGroup = column.PeekTopGroup();
                column.PopGroup(selectedGroup);
                selectedColumn = column;
                LiftCards(selectedGroup);
            }
        }
        else
        {
            if (column.CanPush(selectedGroup[0]))
            {
                // Audio Move
                AudioManager.Instance.PlaySound(SoundEffectName.ShuffleCard);
                //    column.PushGroup(selectedGroup);
                if(column!= selectedColumn)
                {
                    StartCoroutine(CardMover.Instance.MoveCardGroupParabola(selectedGroup, column));
                }
                else
                {
                    column.PushGroup(selectedGroup);
                }
                selectedGroup = null;
                selectedColumn = null;
            }
            else
            {
                // Audio Move Failed
                AudioManager.Instance.PlaySound(SoundEffectName.FailedMove);
                ResetCurrentSelected();
            }
        }

    }

    public void ResetCurrentSelected()
    {
        if (selectedGroup != null && selectedColumn != null)
        {
            selectedColumn.PushGroup(selectedGroup);
            selectedGroup = null;
            selectedColumn = null;
        }
        
    }
   
    public IEnumerator DealCards(Transform spawnTransform)
    {
        ResetCurrentSelected();
        isDealing = true;
        isInteracting = true;
        List<Column> availableColumns = columns
            .Where(c => !c.isScoreColumn && c.isActiveAndEnabled && c.isUnlocked)
            .ToList();

        int cardsRemaining = currentDealCount;
        AudioManager.Instance.PlaySound(SoundEffectName.DealButton);

        while (cardsRemaining > 0)
        {
            int groupSize = Random.Range(currentData.sameColorGroupMin, currentData.sameColorGroupMax + 1);
            groupSize = Mathf.Min(groupSize, cardsRemaining);
            Color color = RandomColor();

            // IMPORTANT: refresh column order after animation
            Column targetCol = availableColumns.OrderBy(col => col.cards.Count).First();

            List<Card> group = new List<Card>();
            for (int i = 0; i < groupSize; i++)
            {
                GameObject newCard = Instantiate(cardPrefab, spawnTransform);
                Card card = newCard.GetComponent<Card>();
                card.cardColor = color;
                card.SetUp();
                group.Add(card);
            }

            // Wait until card animation finishes before choosing next column
            yield return StartCoroutine(CardMover.Instance.MoveCardGroupParabola(group, targetCol, 0.1f));
            cardsRemaining -= groupSize;
        }
        isDealing = false;
        isInteracting = false;
        currentDealCount += currentData.dealBonusPerRound;
    }


    private Color RandomColor()
    {
        if (colors == null) return Color.white;
        int randomIndex = Random.Range(0, maxColor);
        return colors[randomIndex];
    }
    public void LiftCards(List<Card> group)
    {
        if (group == null || group.Count == 0) return;
        for (int i = 0; i < group.Count; i++)
        {
            group[i].gameObject.transform.position += new Vector3(0, 0, 0.5f);
        }

    }

    #region Events Button

    public void OnSelectLevelShow()
    {
        UIManager.Instance.ShowUI(CanvasName.Canvas_SelectLevel); 
    }

    public void OnSelectLevelHide()
    {
        UIManager.Instance.HideUI(CanvasName.Canvas_SelectLevel);
    }

    #endregion Events Button


    #region Status Game
    public void SetUp()
    {
        currentState = GameState.Playing;

        //cập nhật dữ liệu mỗi level
        cardGetPerLevel = 0;

        currentData = levelDatas[currentLevel - 1];

        if(currentData.maxColor <= colors.Count)
        {
            maxColor = currentData.maxColor;
        }
        else
        {
            maxColor = colors.Count;
        }

        currentDealCount = currentData.initialDealCount;

        currentUnlockCost = 5;
        preUnlockCost = 3;
        // cập nhật hiển thị ui trên gameplay
        UIManager.Instance.UpdateMoneyUI(Pref.Money);
    }

    bool IsWin()
    {
        return cardGetPerLevel>= currentData.totalCardsRequire;
    }
    public void Win()
    {
        currentState = GameState.Win;
        AudioManager.Instance.PlaySound(SoundEffectName.Win);
        Pref.UnlockLevel = currentLevel + 1;
        UIManager.Instance.ShowUI(CanvasName.Canvas_Win);
        FindObjectOfType<WinDialog>().SetUp(cardGetPerLevel);
    }
    public void Pause()
    {
        currentState= GameState.Pause;
        Time.timeScale = 0;
        UIManager.Instance.ShowUI(CanvasName.Canvas_GamePause);
    }
    public void Resume()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1;
        UIManager.Instance.HideUI(CanvasName.Canvas_GamePause); // pause
        UIManager.Instance.HideUI(CanvasName.Canvas_Win);
    }
    public void Restart()
    {
        Time.timeScale = 1;
        LoadScene(currentLevel);
    }
    public void Next()
    {
        LoadScene(currentLevel+1);
    }
    public void Exit()
    {
        Time.timeScale = 1;
        LoadScene();

    }
    public void LoadScene(int level )
    {
        SceneManager.LoadScene("Gameplay");
        currentLevel = level;
        SetUp();

    }
    public void LoadScene()
    {
        SceneManager.LoadScene("Home");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UIManager.Instance.HideAllUI();
        if (scene.name == "Gameplay")
        {
            columns = FindObjectsOfType<Column>().OrderBy(col => col.transform.position.x).ToList();
            UIManager.Instance.ShowUI(CanvasName.Canvas_GamePlay);
        }
        else
        {
            UIManager.Instance.ShowUI(CanvasName.Canvas_Home);
        }

    }

    #endregion Status Game
}

