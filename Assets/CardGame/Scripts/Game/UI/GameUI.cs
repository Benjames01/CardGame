using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{

    public static event Action OnReturnGame;
    public static event Action OnNextCard;
    
    
    [SerializeField] private GameObject GameWindow;

    [SerializeField] private TMP_Text cardTitle;
    [SerializeField] private TMP_Text cardText;
    [SerializeField] private TMP_Text gameTitle;
    
    [SerializeField] private ButtonManager continueButtonText;

    private int currentRound = 1;
    private int lastRound = 1;
    
    private bool started = false;
    private float elapsedTime;
    private float timeStarted;
    
    private void OnEnable()
    {
        GameManager.OnCardSelected += OnCardSelected;
        GameManager.OnNowLastRound += OnLastCard;
        GameManager.OnGameEnded += OnGameEnded;
        GameSettingsUI.OnGameStarted += OnGameStarted;
    }
    
    private void OnDisable()
    {
        GameManager.OnCardSelected -= OnCardSelected;
        GameManager.OnNowLastRound -= OnLastCard;
        GameManager.OnGameEnded += OnGameEnded;
    }

    private void Update()
    {
        if (started)
        {
            elapsedTime += Time.deltaTime;

            gameTitle.text = GetGameTitle();
        }
    }


    private void OnGameStarted(Game game)
    {
        currentRound = 1;
        lastRound = game.GetRounds();
        started = true;

        timeStarted = Time.time;
        elapsedTime = 0f;
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
        currentRound += 1;
    }

    private void OnGameEnded()
    {
        SceneManager.LoadScene("CardGame/Scenes/MenuScene");
    }

    private void OnLastCard()
    {
        continueButtonText.buttonText = "END GAME";
        continueButtonText.UpdateUI();
    }

    private string GetGameTitle()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        string formatTimeSpan = timeSpan.ToString("mm':'ss'.'ff");
        
        return "Round (" + currentRound + ") " + formatTimeSpan;
    }
    
}
