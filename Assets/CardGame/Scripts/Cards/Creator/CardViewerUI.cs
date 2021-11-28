using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardViewerUI : MonoBehaviour
{
    private CardPack cardPack;

    
    [SerializeField] private GameObject cardViewCanvas;

    private List<GameObject> cardTemplateClones = new List<GameObject>();

    [SerializeField] private GameObject cardList;
    [SerializeField] private GameObject cardTemplate;
    [SerializeField] private CardCreatorUI cardCreator;

    public void ViewCardPack(CardPack cardPack)
    {

        if (cardTemplateClones.Count > 0)
        {
            foreach (var clone in cardTemplateClones)
            {
                Destroy(clone);
            }
        }
        
        int index = 0;
        foreach (var card in cardPack.Cards)
        {
            Debug.Log("INDEX: " + index);
            var clone = Instantiate(cardTemplate);
            
            TMP_Text name = clone.transform.Find("Name/Value").GetComponent<TMP_Text>();
            TMP_Text text = clone.transform.Find("Text/Value").GetComponent<TMP_Text>();
            
            clone.transform.parent = cardList.transform;
            clone.transform.localScale = Vector3.one;

            name.text = card.Name;
            text.text = card.Text;

            int tempIndex = index;
            clone.GetComponent<Button>().onClick.AddListener(() =>
            {
                cardCreator.EditCard(cardPack.Cards[tempIndex]);
            });
            
            index++;
        }
        
        cardViewCanvas.gameObject.SetActive(true);
    }
    
    
    
    
}
