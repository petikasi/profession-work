using System;
using System.Collections.Generic;
using System.IO;
using Assets.GameScripts.Model.Deckmaker.Deks;
using UnityEditor.Rendering;
using UnityEngine;

namespace  GameScripts.Persistence
{

    public class DeckSaving
    {
        private static readonly string path = Application.persistentDataPath + "/decks.json";

        public static void SavingDecks(Deck deck, DeckList deckList)
        {

            if (File.Exists(path))
            {
                string existingJson = File.ReadAllText(path);
                deckList = JsonUtility.FromJson<DeckList>(existingJson);
            }

            deckList ??= new DeckList();

            deckList.decks.Add(deck);
            string json = JsonUtility.ToJson(deckList, true);
            File.WriteAllText(path, json);
        }



        public static DeckList LoadDeck()
        {
            if (!File.Exists(path))
            {
                Debug.Log("No save file found.");
                return null;
            }

            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<DeckList>(json);
        }



    }
}
