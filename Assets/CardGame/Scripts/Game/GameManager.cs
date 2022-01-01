using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    
    public static  GameManager Instance
    {
        get { return _instance; }
    }
    
    public static event Action<Card> OnCardSelected;
    public static event Action OnGameEnded;
    public static event Action OnNowLastRound;

    private List<Card> cards;

    private Game game;

    private int currentRound = 1;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    public Game GetGame()
    {
        return game;
    }
    
    private void OnEnable()
    {
        GameSettingsUI.OnGameStarted += OnGameStarted;
        GameUI.OnNextCard += OnNextCard;
    }
    
    private void OnDisable()
    {
        GameSettingsUI.OnGameStarted -= OnGameStarted;
        GameUI.OnNextCard -= OnNextCard;
    }
    
    private void OnGameStarted(Game game)
    {
        currentRound = 1;
        Debug.Log("Current round: " + currentRound);
        this.game = game;
        OnCardSelected?.Invoke(game.GetRandomCard());
        
    }

    private void OnNextCard()
    {
        if (currentRound + 1 > game.GetRounds())
        {
            Debug.Log("Game Ended - Reached round: " + currentRound);
            OnGameEnd();
            return;
        }
        
        currentRound += 1;

        Debug.Log("Current round: " + currentRound);
        if (currentRound == game.GetRounds())
        {
            Debug.Log("This is the last round.");
            OnLastRound();
        }
        
        OnCardSelected?.Invoke(game.GetRandomCard());
    }

    private void OnGameEnd()
    {
        OnGameEnded?.Invoke();
    }
    
    private void OnLastRound()
    {
        OnNowLastRound?.Invoke();
    }
}

