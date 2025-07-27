using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] int level;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text;
    public int Level { get => level; set => level = value; }

    private void Start()
    {
        button.onClick.AddListener(OpenLevel);
        text.text = level.ToString();
    }
    public void OpenLevel()
    {
        GameManager.Instance.LoadScene(level);
    }
}
