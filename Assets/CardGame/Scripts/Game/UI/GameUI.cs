using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{

    public static event Action OnReturnGame;
    public static event Action OnNextCard;
    
    
    [SerializeField] private GameObject GameWindow;

    [SerializeField] TMP_Text cardTitle;
    [SerializeField] TMP_Text cardText;
    
    
    private void Awake()
    {
        GameManager.OnCardSelected += OnCardSelected;
    }
    private void OnCardSelected(Card card)
    {
        if (GameWindow != null)
        {
            GameWindow.SetActive(true);
        }
        
        cardTitle.SetText(card.Name);

        string parsedText = CardUtils.ParseCard(card.Text);
        
        
        cardText.SetText(parsedText);
    }

    public void OnReturnGameButtonPressed()
    {
        if (GameWindow != null)
        {
            GameWindow.SetActive(false);
        }
        OnReturnGame?.Invoke();
    }

    public void OnNextCardButtonPressed()
    {
        OnNextCard?.Invoke();
    }
    
}
