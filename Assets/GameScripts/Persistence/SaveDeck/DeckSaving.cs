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
        private static string DeckFolder =>
        Path.Combine(Application.persistentDataPath, "Decks");

        public static void SaveDeckList(DeckList deckList)
        {
            if (!Directory.Exists(DeckFolder))
                Directory.CreateDirectory(DeckFolder);

            foreach (Deck deck in deckList.decks)
            {
                SaveDeck(deck);
            }
        }

        public static void SaveDeck(Deck deck)
        {
            if (!Directory.Exists(DeckFolder))
                Directory.CreateDirectory(DeckFolder);

            string path = Path.Combine(DeckFolder, deck.ID + ".json");
            string json = JsonUtility.ToJson(deck);
            Debug.Log(json);
            File.WriteAllText(path, json);

            Debug.Log($"Saved deck {deck.NAME} to {path}");
        }



        public static DeckList LoadDeckList()
        {
            DeckList deckList = new();

            string folder = Path.Combine(Application.persistentDataPath, "Decks");

            if (!Directory.Exists(folder))
                return deckList;

            string[] files = Directory.GetFiles(folder, "*.json");

            foreach (string file in files)
            {
                string json = File.ReadAllText(file);
                Deck deck = JsonUtility.FromJson<Deck>(json);
                deckList.decks.Add(deck);
            }

            return deckList;
        }



    }
}
