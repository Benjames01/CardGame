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

    private List<ICardVariable> cardVariables;

    private Card card;

    private void OnEnable()
    {
        PrepareVariablesList();
    }

    public void EditCard(Card card)
    {
        creatorCanvas.SetActive(true);
        this.card = card;
        
        cardNameField.text = card.Name;
        cardTextField.text = card.Text;
    }

    public void OnSaveButtonClicked()
    {
        if (string.IsNullOrWhiteSpace(cardNameField.text) || string.IsNullOrWhiteSpace(cardTextField.text))
        {
            notificationManager.title = "Error Saving Card!";
            notificationManager.description = "Make sure all fields are filled and try again.";
            notificationManager.UpdateUI();
            notificationManager.OpenNotification();
        }
        else
        {
            card.Name = cardNameField.text;
            card.Text = cardTextField.text;
            cardPack.UpdateCard(card);
            
            notificationManager.title = "Saved Card!";
            notificationManager.description = "Card saved successfully!";
            notificationManager.UpdateUI();
            notificationManager.OpenNotification();
            creatorCanvas.SetActive(false);
        }
    }
    
    public void PrepareVariablesList()
    {
        cardVariables = new List<ICardVariable>();
        cardVariables.Add(new VariableRandomNumber());
        cardVariables.Add(new VariableRandomPlayer());


        int index = 0;
        foreach (var variable in cardVariables)
        {
            var clone = Instantiate(variableListItemTemplate);
            clone.transform.parent = variableList.transform;
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
                        Debug.Log("Index of pressed button is: " + tempIndex);
                        AddIdentifierToText(cardVariables[tempIndex]);
                    }));
            index++;
        }
    }

    void AddIdentifierToText(ICardVariable variable)
    {
        cardTextField.text += " " + variable.Identifier + " ";
    }
    
    
    
}
