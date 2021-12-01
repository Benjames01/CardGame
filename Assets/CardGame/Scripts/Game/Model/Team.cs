using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Team
{
    public string Name { get; }
    public List<Player> Players { get; }

    private Player lastSelectedPlayer = new Player("Last Selected Player");


    public Team()
    {
        Players = new List<Player>();
    }    
    public Team(List<Player> players)
    {
        this.Players = players;
    }
    
    public Player GetRandomPlayer()
    {
        int index = new Random().Next(Players.Count);
        Player player = Players[index];

        while (player == lastSelectedPlayer)
        {
            index = new Random().Next(Players.Count);
            player = Players[index];
        }

        lastSelectedPlayer = player;
        
        return player;
    }

    public void AddPlayer(Player player)
    {
        Players.Add(player);
    }
    
    public void RemovePlayer(Player player)
    {
        Players.Remove(player);
    }
    
}
