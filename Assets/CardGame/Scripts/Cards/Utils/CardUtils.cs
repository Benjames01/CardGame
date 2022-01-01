using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using UnityEngine;

public class CardUtils
{

    public static List<ICardVariable> CardVariables = new List<ICardVariable>()
    {
        new VariableRandomNumber(),
       new VariableRandomPlayer()
    };



    public static string ParseCard(String text)
    {
        foreach (var cardVariable in CardVariables)
        {
            Regex regex = cardVariable.GetRegex();
            var results = regex.Matches(text);
            foreach (Match match in results)
            {
                

                int param = -1;
                int.TryParse(match.Groups["Param"].Value, out param );


                string value;
                if (param != -1)
                {
                   // Debug.Log("Param: " + match.Groups["Param"].Value);
                    value = cardVariable.GetValue(param);
                }
                else
                {
                    value = cardVariable.GetValue();
                }
                
                
                int index = text.IndexOf(match.Value);
                //Debug.Log("replacing " + match.Value.ToString() + " at: " + index);
                if (index != -1)
                   text = text.Remove(index, match.Value.Length).Insert(index, value);
            }
        }
        return text;
    }
    
    
}
