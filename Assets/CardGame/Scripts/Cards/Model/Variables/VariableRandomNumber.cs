using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableRandomNumber : ICardVariable
{
    public string Name
    {
        get => "Random Number";
    }
    public string Description
    {
        get => "Selects a random number between 1 and your chosen max";
    }
    public string Identifier
    {
        get => "%Random[max]%";
    }
    
    public string GetValue()
    {
        return Random.Range(0, 10).ToString();
    }
}
