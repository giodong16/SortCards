using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Column : MonoBehaviour
{
    [Header("Lock")]
    public bool isUnlocked = false;
  //  public int unLockCost;
    public GameObject lockColumn;
    public TMP_Text costText;

    [Header("ProgressBar")]
    public ProgessBar progressBar;

    public bool isScoreColumn = false;
    public Stack<Card> cards = new Stack<Card>();
    public int requireCards = 12;
    public float verticalSpace = 0.5f;
    public float spacingZ = 0.1f;
    public float firstSpacingZ = 0.2f;

    private void Start()
    {
        UpdateCostUI();
        DeactiveLock();
    }
    public bool CanPush(Card newCard)
    {
        return cards.Count == 0 || cards.Peek().cardColor == newCard.cardColor;
    }


    public void Push(Card newCard) {
        cards.Push(newCard);
        newCard.transform.SetParent(transform,false);
        UpdateCardPostion();
        if (isScoreColumn)
        {
            CheckScoreColumn();
            if (progressBar != null)
            {
                progressBar.UpdateProgress(cards.Count, requireCards, true);

            }
        }
    }

    public Card Pop()
    {
        var card = cards.Pop();
        UpdateCardPostion();
        if (isScoreColumn)
        {
            if (progressBar != null)
            {
                progressBar.UpdateProgress(cards.Count, requireCards, false);

            }
        }
        return card;
    }
    public List<Card> PeekTopGroup()
    {
        List<Card> group = new List<Card>();
        if (cards.Count == 0) return group;
 
        Color topColor = cards.Peek().cardColor;
        foreach (var card in cards)
        {
            if (card.cardColor == topColor) group.Add(card);
            else break;
        }
      //  group.Reverse();
        return group;
    }

    public void PopGroup(List<Card> group)
    {
        if(group ==  null|| group.Count == 0) return;  
        for (int i = 0; i < group.Count; i++)
        {
            cards.Pop();
        }
        UpdateCardPostion();
        if (isScoreColumn)
        {
            if (progressBar != null)
            {
                progressBar.UpdateProgress(cards.Count, requireCards, false);

            }
        }
    }


    public void PushGroup(List<Card> group)
    {
        if (group == null || group.Count == 0) return;
        foreach (Card card in group)
        {
            cards.Push(card);
            card.transform.SetParent(transform,false);
        }
        UpdateCardPostion();

        if (isScoreColumn)
        {
            CheckScoreColumn();
            if (progressBar != null)
            {
                progressBar.UpdateProgress(cards.Count, requireCards, true);

            }
        }
    }

    public void UpdateCardPostion()
    {
        if(cards ==null || cards.Count == 0) { return; }
        int i = cards.Count;
        foreach (Card card in cards) {
            card.transform.localPosition = new Vector3(0f, 0f, (i+1) * spacingZ);
            i--;
        } 
    }
    void OnMouseDown()
    {
        if (GameManager.Instance.currentState != GameState.Playing) return;
        if (GameManager.Instance.isInteracting) return;
        if (!isUnlocked)
        {
            TryUnlock();
            return;
        }
        FindObjectOfType<GameManager>().OnColumnClicked(this);
    }
    private void CheckScoreColumn()
    {
        if (cards.Count >= requireCards)
        {
            GameManager.Instance.AddMoney(cards.Count);
            AudioManager.Instance.PlaySound(SoundEffectName.GetScore);

            foreach (Card card in cards)
            {
                Destroy(card.gameObject);
            }

            cards.Clear();
            UpdateCardPostion();

            if (progressBar != null)
            {
                progressBar.UpdateProgress(requireCards, requireCards,true); 

            }
        }
    }



    private void TryUnlock()
    {
        if (Pref.Money >= GameManager.Instance.currentUnlockCost)
        {
            Pref.Money -= GameManager.Instance.currentUnlockCost;
            UIManager.Instance.UpdateMoneyUI(Pref.Money);
            isUnlocked = true;
            //Audio
            AudioManager.Instance.PlaySound(SoundEffectName.Unlock);
            // Deactive UI Lock;
            DeactiveLock();
            GameManager.Instance.CreateUnlockCost();
            // UpdateCost UI
            GameManager.Instance.UpdateAllColumnsCost();
        }
        else
        {
            AudioManager.Instance.PlaySound(SoundEffectName.FailedUnlock);
            GetComponent<PlayAnimation>().Play();
            Debug.Log("Không đủ tiền");
            // Audio
        }
        
    }
    public void DeactiveLock()
    {
        lockColumn.SetActive(!isUnlocked);
    }
    public void UpdateCostUI()
    {
        costText.text = GameManager.Instance.currentUnlockCost.ToString();
    }

}
