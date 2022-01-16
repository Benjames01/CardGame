using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class CardCreatorUI : MonoBehaviour
{

    [SerializeField] private Button btnSaveAndContinue;
    [SerializeField] private Button btnSaveAndExit;
    [SerializeField] private Button btnCancelAndExit;


    [SerializeField]
    private TMP_InputField cardNameField;
    [SerializeField]
    private TMP_InputField cardTextField;

    [SerializeField]
    private NotificationManager notificationManager;

    [SerializeField] private GameObject variableList;
    [SerializeField] private GameObject variableListItemTemplate;
    [SerializeField] private GameObject creatorCanvas;

    [SerializeField] private CardPackUI cardPack;

    private Card card;


    public static event Action OnCreatorDisplay;
    public static event Action<Card> OnCardSaved;
    public static event Action<Card> OnCardSavedAndExit;
    public static event Action OnCardExit; 
    

    private void OnEnable()
    {
        PrepareVariablesList();

        CardViewerUI.OnCardSelected += EditCard;
    }

    private void OnDisable()
    {
        CardViewerUI.OnCardSelected -= EditCard;
    }

    
    private void Awake()
    {
        
        // Add event listeners
        btnCancelAndExit.onClick.AddListener(() =>
        {
            OnSaveButtonClicked(SaveType.CancelAndExit);
        });
        
        btnSaveAndContinue.onClick.AddListener(() =>
        {
            OnSaveButtonClicked(SaveType.SaveAndContinue);
        });
        
        btnSaveAndExit.onClick.AddListener(() =>
            {
                OnSaveButtonClicked(SaveType.SaveAndExit);
            }
        );
    }
    
    
    // Enable the card creator view and update the ui to reflect the selected card
    public void EditCard(Card card)
    {
        OnCreatorDisplay?.Invoke();
        creatorCanvas.SetActive(true);
        this.card = card;
        
        cardNameField.text = card.Name;
        cardTextField.text = card.Text;
    }
    
    public enum SaveType
    {
        SaveAndContinue,
        SaveAndExit,
        CancelAndExit
        
    }
    
    public void OnSaveButtonClicked(SaveType saveType)
    {
        // Dont save if card text or name is empty
        if (string.IsNullOrWhiteSpace(cardNameField.text) || string.IsNullOrWhiteSpace(cardTextField.text))
        {
            notificationManager.title = "Error Saving Card!";
            notificationManager.description = "Make sure all fields are filled and try again.";
            notificationManager.UpdateUI();
            notificationManager.OpenNotification();
        }
        else
        {
            
            if (saveType != SaveType.CancelAndExit)
            {
                card.Name = cardNameField.text;
                card.Text = cardTextField.text;
            }
            
            // Follow selected save procedure
            switch (saveType)
            {
                case SaveType.CancelAndExit:
                    OnCardExit?.Invoke();
                    creatorCanvas.SetActive(false);
                    break;
                case SaveType.SaveAndContinue:
                    OnCardSaved?.Invoke(card);
                    notificationManager.title = "Saved Card!";
                    notificationManager.description = "Card saved successfully!";
                    notificationManager.UpdateUI();
                    notificationManager.OpenNotification();
                    notificationManager.CloseNotification();
                    break;
                case SaveType.SaveAndExit:
                    OnCardSavedAndExit?.Invoke(card);
                    creatorCanvas.SetActive(false);
                    break;
            }
        }
    }
    
    
    // Create the ui buttons for their respective card variables
    public void PrepareVariablesList()
    {
        int index = 0;
        foreach (var variable in CardUtils.CardVariables)
        {
            var clone = Instantiate(variableListItemTemplate, variableList.transform, true);
            clone.transform.localScale = Vector3.one;
            
            TMP_Text name = clone.transform.Find("Name Panel/Name Value").GetComponent<TMP_Text>();
            TMP_Text id = clone.transform.Find("ID Panel/ID Value").GetComponent<TMP_Text>();
            TMP_Text description = clone.transform.Find("Desc Panel/Description Value").GetComponent<TMP_Text>();
            
            name.text = variable.Name;
            id.text = variable.Identifier;
            description.text = variable.Description;

            int tempIndex = index;
            clone.GetComponent<Button>().onClick.AddListener(
                (() =>
                    {
                        AddIdentifierToText(CardUtils.CardVariables[tempIndex]);
                    }));
            index++;
        }
    }

    void AddIdentifierToText(ICardVariable variable)
    {
        cardTextField.text += " " + variable.Identifier + " ";
    }
    
    
    
}
