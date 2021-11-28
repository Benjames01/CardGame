using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPackUI : MonoBehaviour
{

    [SerializeField] private CardViewerUI cardViewer;
    
    [SerializeField] private GameObject cardPackList;
    [SerializeField] private GameObject cardPackTemplate;

    private List<CardPack> cardPacks;

    private int selectedPack = -1;
    
    public void Awake()
    {
        cardPacks = CardPersistence.LoadCardPacks();

        if (cardPacks.Count > 0)
        {
            // load cards
            Debug.Log("We have " + cardPacks.Count + " cardPacks!");
            int index = 0;
            foreach (var cardPack in cardPacks)
            {
                var clone = Instantiate(cardPackTemplate);
                clone.transform.parent = cardPackList.transform;
                clone.transform.localScale = Vector3.one;

                TMP_Text name = clone.transform.Find("Name/Value").GetComponent<TMP_Text>();
                TMP_Text lastPlayed = clone.transform.Find("Last Played/Value").GetComponent<TMP_Text>();
                TMP_Text timesPlayed = clone.transform.Find("Times Played/Value").GetComponent<TMP_Text>();
                TMP_Text dateCreated = clone.transform.Find("Date Created/Value").GetComponent<TMP_Text>();

                name.text = cardPack.Name;
                lastPlayed.text = cardPack.LastPlayed;
                timesPlayed.text = cardPack.TimesPlayed.ToString();
                dateCreated.text = cardPack.DateCreated;

                int tempIndex = index;
                clone.GetComponent<Button>().onClick.AddListener(() =>
                {
                    ShowCards(cardPacks[tempIndex]);
                });
                index++;
            }

        }
        else
        {
            Debug.Log("Creating a test card pack!");
            
            // Create a test pack
            Card testCard = new Card("Test card 1", "Test card 1 text");
            Card testCard2 = new Card("Test card 2", "Test card 2 text");

            List<Card> cards = new List<Card>();
            cards.Add(testCard);
            cards.Add(testCard2);

            CardPack cardPack = new CardPack("Test Pack", cards);
            
            CardPersistence.SaveCardPack(cardPack);
        }
    }

    private void ShowCards(CardPack cardPack)
    {
        cardViewer.ViewCardPack(cardPack);
        foreach (var card in cardPack.Cards)
        {
            Debug.Log(card.Name);
        }
    }

    public void UpdateCard(Card card)
    {
        if (selectedPack != -1 && selectedPack <= cardPacks.Count - 1)
        {
            var updateCard = cardPacks[selectedPack].Cards.FirstOrDefault(c => c.ID == card.ID);
            if (updateCard == null)
            {
                cardPacks[selectedPack].Cards.Add(card);
            }
            else
            {
                updateCard.Name = card.Name;
                updateCard.Text = card.Text;
            }
        }
        
    }
}
