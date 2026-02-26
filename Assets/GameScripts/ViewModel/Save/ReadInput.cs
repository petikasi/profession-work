using Unity.VisualScripting;
using UnityEngine;

public class ReadInput : MonoBehaviour
{

    public void ReadStringInput( string s) 
    {
        Debug.Log($"Deck got named as:{s}");
        DeckBuilderController.Instance.CurrentDeck.NAME = s;
    }
}
