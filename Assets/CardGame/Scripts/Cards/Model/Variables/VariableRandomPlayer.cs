using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using UnityEngine;


public class VariableRandomPlayer : ICardVariable
{
    private Player lastSelectedPlayer;
    
    public string Name
    {
        get => "Random Player";
    }
    public string Description
    {
        get => "Selects a random player from the player list.";
    }
    public string Identifier
    {
        get => "%RandomPlayer%";
    }
    
    public string GetValue(int args=-1)
    {

        List<Player> players = GameManager.Instance.GetGame().GetPlayers();

        System.Random random = new System.Random();

        int index = random.Next(players.Count);
        Player player = players[index];

        while (player == lastSelectedPlayer)
        {
            index = random.Next(players.Count);
            player = players[index];
        }

        lastSelectedPlayer = player;
        
        return "<b>" + player.Name + "</b>";
    }

    public Regex GetRegex()
    {
        return new Regex("%RandomPlayer%", RegexOptions.IgnoreCase);
    }
}
