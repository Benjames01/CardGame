using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    
    public static  GameManager Instance
    {
        get { return _instance; }
    }
    
    public static event Action<Card> OnCardSelected;
    public static event Action OnGameEnded;

    private List<Card> cards;

    private Game game;

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
        this.game = game;
        OnCardSelected?.Invoke(game.GetRandomCard());
        
    }

    private void OnNextCard()
    {
        OnCardSelected?.Invoke(game.GetRandomCard());
    }

    private void OnGameEnd()
    {
        OnGameEnded?.Invoke();
    }
}

