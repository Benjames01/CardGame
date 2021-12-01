using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
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
        
        string dataPath = GetDataPath(cardPack);
        string jsonData = JsonUtility.ToJson(cardPack, true);
        
        File.WriteAllText(dataPath + ".json", jsonData);
        
        
        Debug.Log("Saved: " + cardPack.ID + ".json");
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

    public static bool RemoveCardPack(CardPack cardPack)
    {
        if (!Directory.Exists(DataPath))
            return false;

        string dataPath = GetDataPath(cardPack);
        
        string cardPath = dataPath + ".json";
        string metaPath = dataPath + ".json.meta";
        if (File.Exists(cardPath))
        {
            File.Delete(cardPath);
            
            if (File.Exists(metaPath))
            {
                File.Delete(metaPath);
            }
            
            return true;
        }

        return false;
    }


    private static string GetDataPath(CardPack cardPack)
    {
        return DataPath + cardPack.ID;
    }

}
