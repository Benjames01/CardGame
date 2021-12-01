using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.InteropServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardViewerUI : MonoBehaviour
{
    private CardPack cardPack;

    [SerializeField] private Button btnReturn;
    [SerializeField] private GameObject cardViewCanvas;

    private List<GameObject> cardTemplateClones = new List<GameObject>();

    [SerializeField] private GameObject cardList;
    [SerializeField] private GameObject cardTemplate;

    private int selectedIndex;
    public static event Action<Card> OnCardSelected;
    public static event Action<Card> OnCardRemoved;

    public static event Action<Card> OnCardAdded;
    public static event Action OnViewerDisplay;

    public static event Action OnReturnPressed;
    
    private void Awake()
    {
        btnReturn.onClick.AddListener(() =>
        {
            cardViewCanvas.SetActive(false);
            OnReturnPressed?.Invoke();
        });
    }

    private void OnEnable()
    {
        CardPackUI.OnViewCardPackPressed += OnViewCardPack;
        CardCreatorUI.OnCardExit += OnCardExit;
        CardCreatorUI.OnCardSavedAndExit += OnSavedAndExit;
        CardCreatorUI.OnCardSaved += OnSavedAndContinue;
        CardCreatorUI.OnCreatorDisplay += OnCreatorDisplay;
    }

    private void OnDisable()
    {
        CardPackUI.OnViewCardPackPressed -= OnViewCardPack;

        CardCreatorUI.OnCardExit -= OnCardExit;
        CardCreatorUI.OnCardSavedAndExit -= OnSavedAndExit;
        CardCreatorUI.OnCardSaved -= OnSavedAndContinue;
        CardCreatorUI.OnCreatorDisplay -= OnCreatorDisplay;
    }

    private void OnCreatorDisplay()
    {
        if (cardViewCanvas.activeSelf)
        {
            Debug.Log("On Creator Display - disabling");
            cardViewCanvas.SetActive(false);
        }
        else
        {
            Debug.Log("On Creator Display - already disabled");
        }
    }

    private void SetVisible()
    {
        OnViewerDisplay?.Invoke();
        cardViewCanvas.SetActive(true);
    }

    private void OnViewCardPack(CardPack cardPack)
    {
        ViewCardPack(cardPack);
    }

    private void OnCardExit()
    {
        cardViewCanvas.gameObject.SetActive(true);
    }

    private void OnSavedAndContinue(Card card)
    {
        ViewCardPack(cardPack, false);
    }

    private void OnSavedAndExit(Card card)
    {
        ViewCardPack(cardPack);
    }

    private void ViewCardPack(CardPack pack, bool showScreen = true)
    {
        SetVisible();
        this.cardPack = pack;

        if (cardTemplateClones.Count > 0)
        {
            foreach (var clone in cardTemplateClones)
            {
                Destroy(clone);
            }
        }

        int index = 0;
        foreach (var card in pack.Cards)
        {
            var clone = Instantiate(cardTemplate, cardList.transform, true);

            TMP_Text name = clone.transform.Find("Name/Value").GetComponent<TMP_Text>();
            TMP_Text text = clone.transform.Find("Text/Value").GetComponent<TMP_Text>();

            clone.transform.localScale = Vector3.one;

            cardTemplateClones.Add(clone);

            name.text = card.Name;
            text.text = card.Text;

            int tempIndex = index;
            clone.GetComponent<Button>().onClick.AddListener(() => { selectedIndex = tempIndex; });

            index++;
            cardViewCanvas.gameObject.SetActive(showScreen);
        }
    }


    public void OnCardEdit()
    {
        if (selectedIndex <= -1 || selectedIndex > cardPack.Cards.Count - 1) return;
        OnCardSelected?.Invoke(cardPack.Cards[selectedIndex]);
    }

    public void OnCardRemove()
    {
        if (selectedIndex <= -1 || selectedIndex > cardPack.Cards.Count - 1) return;
        OnCardRemoved?.Invoke(cardPack.Cards[selectedIndex]);
        ViewCardPack(cardPack);
    }

    public void OnCardAdd()
    {
        OnCardAdded?.Invoke(new Card("", ""));
        ViewCardPack(cardPack);
    }
    
}
