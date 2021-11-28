using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CardPersistence
{
    private static string DataPath = Application.dataPath + "/CardPacks/";
    
    public static void SaveCardPack(CardPack cardPack)
    {
        if (!Directory.Exists(DataPath))
        {
            Debug.Log("Creating CardPack Directory at: " + DataPath);
            Directory.CreateDirectory(DataPath);
        }
        
        string jsonData = JsonUtility.ToJson(cardPack, true);
        File.WriteAllText(DataPath + cardPack.Name + ".json", jsonData);
        
        
        Debug.Log("Saved: " + cardPack.Name + ".json");
    }
    
    public static List<CardPack> LoadCardPacks()
    {
        List<CardPack> cardPacks = new List<CardPack>();

        if (!Directory.Exists(DataPath))
        {
            Debug.Log("Creating CardPack Directory at: " + DataPath);
            Directory.CreateDirectory(DataPath);
        }

        DirectoryInfo cardPacksFolder = new DirectoryInfo(DataPath);
        FileInfo[] cardPackFiles = cardPacksFolder.GetFiles("*.json");

        foreach (var cardPack in cardPackFiles)
        {
            string jsonData = File.ReadAllText(DataPath + cardPack.Name);
            CardPack pack = JsonUtility.FromJson<CardPack>(jsonData);
            cardPacks.Add(pack);
        }

        return cardPacks;
    }

}
