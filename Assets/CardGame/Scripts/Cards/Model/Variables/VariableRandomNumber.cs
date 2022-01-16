using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

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
    
    // Return the regex used to find the identifier in card
    public Regex GetRegex()
    {
        return new Regex(@"%Random\[(?<Param>\d{1,2}|100)\]%", RegexOptions.IgnoreCase);
    }
    
    
    // Return a value between 1 and the given max
    public string GetValue(int max)
    {
        return "<b>" + Random.Range(1, max).ToString() + "</b>";
    }
}
