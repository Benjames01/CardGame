using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


[Serializable]
public class CardPack
{
  public List<Card> Cards;
  public string Name;
  public string LastPlayed;
  public string DateCreated;
  public int TimesPlayed;
  public string ID;
  
  
  
  
  public CardPack(string name, List<Card> cards)
  {
    this.Name = name;
    this.Cards = cards;
    this.DateCreated = System.DateTime.Now.ToString();
    this.TimesPlayed = 0;
    this.ID = Guid.NewGuid().ToString();
  }

 
  
}
