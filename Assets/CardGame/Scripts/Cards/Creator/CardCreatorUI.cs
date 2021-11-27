using System;
using System.Collections;
using System.Collections.Generic;
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

    private List<ICardVariable> cardVariables;

    private void Awake()
    {
        Card card = CardPersistence.LoadCardFromJson();

        cardNameField.text = card.Name;
        cardTextField.text = card.Text;

        PrepareVariablesList();
    }

    public void OnSaveButtonClicked()
    {
        try
        {
            Card card = new Card(cardNameField.text, cardTextField.text);
            CardPersistence.SaveCardToJson(card);
        }
        catch (ArgumentNullException e)
        {
            notificationManager.title = "Error Saving Card!";
            notificationManager.description = "Make sure all fields are filled and try again.";
            notificationManager.UpdateUI();
            notificationManager.OpenNotification();
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
