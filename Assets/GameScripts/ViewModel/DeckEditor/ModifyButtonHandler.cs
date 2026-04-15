using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GameScripts.Model.Game.GameController;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.DeckEditor
{
    public class ModifyButtonHandler : MonoBehaviour
    {
        [SerializeField] private Button ModifyButton;

        private void Start()
        {


            ModifyButton.onClick.RemoveAllListeners();
            ModifyButton.onClick.AddListener(()
                => {
                    if (DeckManagerController.Instance.SelectedDeck != null)
                    {
                        Debug.Log("Modification started");
                        DeckManagerController.Instance.AddSelectedDeckToDeckBuilder();
                    }

                }
            );
        }

    }
}
