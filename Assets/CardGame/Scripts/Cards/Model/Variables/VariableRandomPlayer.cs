using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;


public class VariableRandomPlayer : ICardVariable
{
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
    
    public string GetValue()
    {
        return "Random Player";
    }
}
