using System;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets.GameScripts.ViewModel.DeckEditor
{
    public  class DeckViewController :MonoBehaviour
    {
        #region SerializableFields
        [SerializeField] private Button modifyButton;
        [SerializeField] private Button renameButton;
        #endregion
        

        #region
        List<DeckPanel> deckPanels;
        private int nums = 9;
        private int site = 1;
        #endregion


        #region

        #endregion


    }
}
