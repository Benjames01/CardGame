using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CardPersistence
{
    private static string DataPath = Application.dataPath + "/CardPacks/";
    
    public static void SaveCardToJson(Card card)
    {
        string jsonData = JsonUtility.ToJson(card, true);
        File.WriteAllText(Application.dataPath+"/CardPacks/card.json", jsonData);
    }

    public static Card LoadCardFromJson()
    {
        
        Card card = new Card("Enter Card Name","Enter Card Text");

        if (!Directory.Exists(DataPath))
        {
            Debug.Log("Creating Directory at: " + DataPath);
            Directory.CreateDirectory(DataPath);
        }
        
        if (File.Exists(DataPath + "card.json"))
        {
            Debug.Log("loading file from json.");
            string jsonData = File.ReadAllText(DataPath + "card.json");
            card = JsonUtility.FromJson<Card>(jsonData);
        }
        
        return card;
    }
    
}
