using UnityEngine;
using UnityEngine.UI;

public class SaveDeckButton : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private Button saveButtonObj;

    private void Start() { 


        saveButtonObj.onClick.RemoveAllListeners();
        saveButtonObj.onClick.AddListener(()
            =>{
                if (DeckBuilderController.Instance.CurrentDeck.NAME != null) 
                {

                    DeckBuilderController.Instance.InitializeSave();
                }
                
            }
        );
    }


}
