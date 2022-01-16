using System.Collections.Generic;
using System.Text.RegularExpressions;



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
    
    // Select and return the name of a random player in the game
    public string GetValue(int args=-1)
    {
        List<Player> players = GameManager.Instance.GetGame().GetPlayers();
        System.Random random = new System.Random();

        int index = random.Next(players.Count);
        Player player = players[index];

        
        // if there's only one player, return that player's name
        if (players.Count == 1)
        {
            return "<b>" + player.Name + "</b>";
        }
        
        // While the player chosen is the same as the previously chosen player, pick another
        while (player == lastSelectedPlayer)
        {
            index = random.Next(players.Count);
            player = players[index];
        }
        
        // Update the last selected player
        lastSelectedPlayer = player;
        
        // Return the player's name with some formatting
        return "<b>" + player.Name + "</b>";
    }

    // Return the regex used to find the identifier in card
    public Regex GetRegex()
    {
        return new Regex(Identifier, RegexOptions.IgnoreCase);
    }
}
