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
                Debug.Log(DeckBuilderController.Instance.DeckInBuilding.NAME);
                if (DeckBuilderController.Instance.DeckInBuilding.NAME != null) 
                {
                    Debug.Log("Saving started");
                    DeckBuilderController.Instance.InitializeSave();
                }
                
            }
        );
    }


}
