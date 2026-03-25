using System;
using Assets.GameScripts.Model.Game.GameController;
using Assets.GameScripts.ViewModel.DeckEditor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiDeckPanel : MonoBehaviour, IPointerClickHandler
{

    [Header("UI")]
    [SerializeField] private TMP_Text deckNameText;
    [SerializeField] private Image factionImage;
    private Deck currentDeck;


    public void Initialize(
        Deck deck,
        Sprite factionSprite)
    {
        deckNameText.text = deck.NAME;
        currentDeck = deck;
        factionImage.sprite = factionSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Selected Deck: " + currentDeck.NAME);
        DeckManagerController.Instance.Choosendeck(currentDeck);
    }
}
