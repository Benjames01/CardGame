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
        
        string dataPath = GetDataPath(cardPack);
        string jsonData = JsonUtility.ToJson(cardPack, true);
        
        File.WriteAllText(dataPath + ".json", jsonData);
        
        
        Debug.Log("Saved: " + cardPack.ID + ".json");
    }
    
    public static List<CardPack> LoadCardPacks()
    {
        List<CardPack> cardPacks = new List<CardPack>();

        // Create the Card Pack directory if it doesn't already exist
        if (!Directory.Exists(DataPath))
        {
            Debug.Log("Creating CardPack Directory at: " + DataPath);
            Directory.CreateDirectory(DataPath);
        }

        // Get all files in the directory with the .json format
        DirectoryInfo cardPacksFolder = new DirectoryInfo(DataPath);
        FileInfo[] cardPackFiles = cardPacksFolder.GetFiles("*.json");
        
        // For all of the card pack files, convert from json into a CardPack object and add to list of card packs
        foreach (var cardPack in cardPackFiles)
        {
            string jsonData = File.ReadAllText(DataPath + cardPack.Name);
            CardPack pack = JsonUtility.FromJson<CardPack>(jsonData);
            cardPacks.Add(pack);
        }

        // return the list of card packs loaded
        return cardPacks;
    }

    public static bool RemoveCardPack(CardPack cardPack)
    {
        if (!Directory.Exists(DataPath))
            return false;

        string dataPath = GetDataPath(cardPack);
        
        string cardPath = dataPath + ".json";
        string metaPath = dataPath + ".json.meta";
        
        // Check the file and its meta data file exist and delete
        if (File.Exists(cardPath))
        {
            File.Delete(cardPath);
            
            if (File.Exists(metaPath))
            {
                File.Delete(metaPath);
            }
            
            // Successfully deleted
            return true;
        }

        // Didn't delete
        return false;
    }

    // Return the file path for the given card pack
    private static string GetDataPath(CardPack cardPack)
    {
        return DataPath + cardPack.ID;
    }

}
