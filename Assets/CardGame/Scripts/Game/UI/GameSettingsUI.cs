using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsUI : MonoBehaviour
{

    public static event Action<Game> OnGameStarted; 


    [SerializeField] private GameObject PlayerList;
    [SerializeField] private GameObject GameSettingsView;
    [SerializeField] private GameObject PlayerTemplate;

    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private HorizontalSelector teamsSelector;



    [SerializeField] private GameObject SettingsPanel;
    
    
    private List<GameObject> PlayerItemClones = new List<GameObject>();
    
    private List<CardPack> cardPacks;

    private int teams;
    
    private void Awake()
    {
        teamsSelector.selectorEvent.AddListener(delegate(int index)
        {
            teams = index + 1;
            
            for (var i = 0; i <= PlayerItemClones.Count - 1; i++)
            {
                if (PlayerItemClones[i] == null) continue;
                
                var selector = PlayerItemClones[i].transform.Find("Team Selector").GetComponent<HorizontalSelector>();

                if (selector.index <= index) continue;
                selector.index = index;
                selector.UpdateUI();
            }
        });
    }

    private void OnEnable()
    {
        CardPackUI.OnCardPacksSelected += OnCardPacksSelected;
    }

    private void OnDisable()
    {
        CardPackUI.OnCardPacksSelected -= OnCardPacksSelected;
    }

    private void OnCardPacksSelected(List<CardPack> packs)
    {
        this.cardPacks = packs;

        if (GameSettingsView != null)
        {
            GameSettingsView.SetActive(true);
        }
    }


    public void OnGameStartButtonPressed()
    {

        Game game = new Game();
        game.SetCardPacks(cardPacks);
        
        int numberTeams = SettingsPanel.transform.Find("Teams/Horizontal Selector").GetComponent<HorizontalSelector>().index + 1;
        List<Team> teams = new List<Team>();
        
        for (int i = 0; i < numberTeams; i++)
        {
            teams.Add(new Team());
        }
        
        foreach (var playerItem in PlayerItemClones.Where(playerItem => playerItem != null))
        {
            var selector = playerItem.transform.Find("Team Selector").GetComponent<HorizontalSelector>().index;
            var name =  playerItem.transform.Find("Name Field").GetComponent<TMP_InputField>().text;

            Player player = new Player(name);
            
            teams[selector].AddPlayer(player);
        }

        foreach (var team in teams)
        {
            if(team != null)
                game.AddTeam(team);
        }


        if (game.GetPlayers().Count > 0)
        {
            OnGameStarted?.Invoke(game);
        }
        
    }
    
    
    public void OnReturnButtonPressed()
    {
        if (GameSettingsView != null)
        {
            GameSettingsView.SetActive(false);
        }
    }

    public void OnAddPlayerButtonPressed()
    {
        if (!string.IsNullOrEmpty(playerName.text))
        {
            
            var clone = Instantiate(PlayerTemplate, PlayerList.transform, true);
            clone.transform.localScale = Vector3.one;

            var button = clone.transform.Find("Button").GetComponent<Button>();

            var name = clone.transform.Find("Name Field").GetComponent<TMP_InputField>();

            var selector = clone.transform.Find("Team Selector").GetComponent<HorizontalSelector>();

            name.text = playerName.text;
            playerName.text = "";

            PlayerItemClones.Add(clone);
            
            int index = 0;
            if (PlayerItemClones.Count  > 0)
            {
                index = PlayerItemClones.Count - 1;
            }
            
            button.onClick.AddListener(() =>
            {
                RemovePlayer(index);
            });
            
            selector.selectorEvent.AddListener(delegate(int selectorIndex)
            {
                if (selectorIndex <= teams - 1) return;
                selector.index = teams - 1;
                selector.UpdateUI();
            });
            
            
        }
    }
    private void RemovePlayer(int index)
    {
        Destroy(PlayerItemClones[index]);
    }
    
}
