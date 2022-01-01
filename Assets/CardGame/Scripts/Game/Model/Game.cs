using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Game
{
    public List<Team> Teams { get; private set; }

    private List<Card> cards;
    private List<Player> players = new List<Player>();
    private List<CardPack> cardPacks;

    private Player lastSelectedPlayer;
    private Team lastSelectedTeam;
    private Card lastSelectedCard;

    private int rounds = 1;
    public void SetCardPacks(List<CardPack> cardPacks)
    {
        this.cardPacks = cardPacks;
        CollateCards();
    }
    
    public void AddTeam(Team team)
    {
        if (Teams == null)
        {
            Teams = new List<Team>();
        }
        Teams.Add(team);
        players.AddRange(team.Players);
    }

    public void SetRounds(int rounds)
    {
        this.rounds = rounds;
    }
    
    public void RemoveTeam(Team team)
    {
        Teams.Remove(team);
        CollatePlayers();
    }


    public List<Player> GetPlayers()
    {
        if (players.Count == 0)
        {
            CollatePlayers();
        }
        return players;
    }

    private void CollateCards()
    {
        List<Card> cards = new List<Card>();
        foreach (var cardPack in cardPacks)
        {
            cards.AddRange(cardPack.Cards);
        }

        this.cards = cards;
    }
    
    private void CollatePlayers()
    {
        List<Player> players = new List<Player>();
        foreach (var team in Teams)
        {
            players.AddRange(team.Players);
        }

        this.players = players;
    }


    public Card GetRandomCard()
    {
        int index = new Random().Next(cards.Count);
        Card card = cards[index];

        if (cards.Count == 1)
        {
            return cards[0];
        } else if (cards.Count == 0)
        {
            return new Card("oopsy!", "This card was left empty :(");
        }
        while (card == lastSelectedCard)
        {
            index = new Random().Next(cards.Count);
            card = cards[index];
        }

        lastSelectedCard = card;
        
        return card;
    }

    public Player GetRandomPlayer()
    {
        List<Player> allPlayers = new List<Player>();

        foreach (var team in Teams)
        {
            allPlayers.AddRange(team.Players);
        }
        
        int index = new Random().Next(allPlayers.Count);
        Player player = allPlayers[index];

        while (player == lastSelectedPlayer)
        {
            index = new Random().Next(allPlayers.Count);
            player = allPlayers[index];
        }

        lastSelectedPlayer = player;
        
        return player;
    }

    public string GetRandomTeamName()
    {
        int index = new Random().Next(Teams.Count);
        Team team = Teams[index];

        while (team == lastSelectedTeam)
        {
            index = new Random().Next(Teams.Count);
            team = Teams[index];
        }

        lastSelectedTeam = team;
        
        return team.Name;
    }

    public int GetRounds()
    {
        return rounds;
    }
    
}
