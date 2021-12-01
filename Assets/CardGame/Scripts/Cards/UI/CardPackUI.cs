using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPackUI : MonoBehaviour
{
    public static event Action<CardPack> OnViewCardPackPressed;
    public static event Action<List<CardPack>> OnCardPacksSelected;
    
    [SerializeField] private GameObject cardPackView;
    [SerializeField] private GameObject cardPackList;
    [SerializeField] private GameObject selectedCardPackList;
    [SerializeField] private GameObject cardPackTemplate;

    [SerializeField] private ModalWindowManager createModalWindow;
    [SerializeField] private TMP_InputField cardPackTitleField;

    private List<CardPack> cardPacks;
    
    private List<GameObject> clonedItemsList = new List<GameObject>();
    private List<GameObject> clonedSelectedItemsList = new List<GameObject>();
    
    private List<int> selectedCardPacks = new List<int>();
        
    private int selectedPack = -1;

    [SerializeField] private Button removeButton;
    
    private void OnEnable()
    {
        CardCreatorUI.OnCardSaved += UpdateCard;
        CardCreatorUI.OnCardSavedAndExit += UpdateCard;
        
        CardCreatorUI.OnCreatorDisplay += DisableView;
        CardViewerUI.OnViewerDisplay += DisableView;

        CardViewerUI.OnReturnPressed += EnableView;
        CardViewerUI.OnCardRemoved += RemoveCard;
        CardViewerUI.OnCardAdded += UpdateCard;
    }

    private void OnDisable()
    {
        CardCreatorUI.OnCardSaved -= UpdateCard;
        CardCreatorUI.OnCardSavedAndExit -= OnCardSavedAndExit;
        
        CardCreatorUI.OnCreatorDisplay -= DisableView;
        CardViewerUI.OnViewerDisplay -= DisableView;
        
        CardViewerUI.OnReturnPressed -= EnableView;
        CardViewerUI.OnCardRemoved -= RemoveCard;
        CardViewerUI.OnCardAdded -= UpdateCard;
    }
    
    private void Awake()
    {
      LoadCardPacks();
      
      if (createModalWindow != null)
      {
          createModalWindow.cancelButton.onClick.AddListener(() =>
          {
              createModalWindow.CloseWindow();
          });
      }

      if (createModalWindow != null)
      {
          createModalWindow.confirmButton.onClick.AddListener(() =>
          {
              if (string.IsNullOrEmpty(cardPackTitleField.text)) return;
          
              var cardPack = new CardPack(cardPackTitleField.text, new List<Card>());
              CardPersistence.SaveCardPack(cardPack);
              LoadCardPacks();

              cardPackTitleField.text = "";
          });
      }
      
      if (removeButton != null)
      {
          removeButton.onClick.AddListener(DeleteSelectedCardPack);
      }
    }

    private void DeleteSelectedCardPack()
    {
        CardPersistence.RemoveCardPack(cardPacks[selectedPack]);
        LoadCardPacks();
    }
    
    private void LoadCardPacks()
    {
        if (clonedItemsList.Count > 0)
        {
            foreach (var item in clonedItemsList)
            {
                Destroy(item);
            }
        }
        
        cardPacks = CardPersistence.LoadCardPacks();

        if (cardPacks.Count > 0)
        {
            // load cards
            Debug.Log("We have " + cardPacks.Count + " cardPacks!");
            int index = 0;
            foreach (var cardPack in cardPacks)
            {
                var clone = Instantiate(cardPackTemplate, cardPackList.transform, true);
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
                    selectedPack = tempIndex;

                    if (selectedCardPackList == null) return;
                    Debug.Log("Selected: " + cardPack.Name);
                    SelectCardPack(tempIndex);

                });
                
                clonedItemsList.Add(clone);
                index++;
            }
        } else
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
    
    private void DisableView()
    {
        cardPackView.SetActive(false);
    }
    
    private void EnableView()
    {
        cardPackView.SetActive(true);
    }

    private void ShowCards(CardPack cardPack)
    {
        OnViewCardPackPressed?.Invoke(cardPack);
    }
    
    private void OnCardSavedAndExit(Card card)
    {
        UpdateCard(card);
        OnViewCardPackPressed?.Invoke(cardPacks[selectedPack]);
    }

    private void UpdateCard(Card card)
    {
        if (selectedPack == -1 || selectedPack > cardPacks.Count - 1) return;
        
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
            
        CardPersistence.SaveCardPack(cardPacks[selectedPack]);
    }

    private void RemoveCard(Card card)
    {
        if (selectedPack == -1 || selectedPack > cardPacks.Count - 1) return;

        var removeCard = cardPacks[selectedPack].Cards.FirstOrDefault(c => c.ID == card.ID);
        if (removeCard == null) return;
        cardPacks[selectedPack].Cards.Remove(removeCard);
        CardPersistence.SaveCardPack(cardPacks[selectedPack]);
    }
    
    public void DisplayCreate()
    {
        createModalWindow.OpenWindow();
    }

    public void ShowCardView()
    {
        if (selectedPack <= -1 || selectedPack >= cardPacks.Count) return;
        OnViewCardPackPressed?.Invoke(cardPacks[selectedPack]);
        ShowCards(cardPacks[selectedPack]);
    }

    public void ReturnToScene()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.MenuScene);
    }

    private void SelectCardPack(int cardPack)
    {
        selectedCardPacks.Add(cardPack);

        var clone = Instantiate(clonedItemsList[cardPack], selectedCardPackList.transform, true);
        clone.transform.localScale = Vector3.one;

        var button = clone.GetComponent<Button>();

        int index = selectedCardPacks.Count - 1;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            RemoveSelectedCard(index);
        });
        
        clonedSelectedItemsList.Add(clone);
    }

    private void RemoveSelectedCard(int index)
    {
        Destroy(clonedSelectedItemsList[index]);
        selectedCardPacks[index] = -1;
    }

    public void OnCardPacksButton()
    {
        var packs = selectedCardPacks.Where(index => index > -1 && index <= this.cardPacks.Count - 1).Select(index => this.cardPacks[index]).ToList();

        if (packs.Count > 0)
        {
            OnCardPacksSelected?.Invoke(packs);
        }
    }
}
