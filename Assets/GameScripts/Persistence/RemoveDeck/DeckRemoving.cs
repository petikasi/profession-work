using System;
using System.Collections.Generic;
using System.IO;
using Assets.GameScripts.Model.Deckmaker.Deks;
using UnityEditor.Rendering;
using UnityEngine;

namespace Assets.GameScripts.Persistence.RemoveDeck
{
    public class DeckRemoving
    {
        private static string DeckFolder =>
        Path.Combine(Application.persistentDataPath, "Decks");

        public static void DeleteDeck(string deckID)
        {
            string path = Path.Combine(DeckFolder, deckID + ".json");

            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Deck deleted: " + deckID);
            }
            else
            {
                Debug.LogWarning("Deck file not found: " + path);
            }
        }

    }
}
