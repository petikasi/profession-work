using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardLayout :MonoBehaviour
{
    private readonly int WIDTHOFTHETABLE = 60;
    private readonly int HEIGHTOFTHETABLE = 40;
    private readonly int SIZEOFATILE = 10;
    private bool ISGENERATED=false;
    private Deck currentDeck;

    private GameObject[,] tiles;


    [Header("Prefabs")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Material[] teammaterials;

    public void GenerateBoardLayout()
    {

        if (ISGENERATED) return;
        ISGENERATED = true;

        GenerateMapTiles(WIDTHOFTHETABLE, HEIGHTOFTHETABLE, SIZEOFATILE);

        List<UnitTypes> myUnits = new()
        {
        UnitTypes.BasicMelee,
        UnitTypes.Ranged,
        UnitTypes.AdvancedMelee,
        UnitTypes.Wizard,
        UnitTypes.Artillery,
        UnitTypes.Special,
        UnitTypes.BasicMelee
        };

        Deck deck = new(myUnits,Factions.Human);
        List<BaseUnit> baseUnits = GenerateDecks(deck);

    }
    public void CreateDeck(Factions fact, List<UnitTypes> list)
    {

        Deck deck = new(list, fact);
        currentDeck = deck;

    }

    private void GenerateMapTiles(int tileCountX, int tileCountY, float tileWidth) 
    {
        tiles = new GameObject[tileCountX, tileCountY];
        for (int i = 0; i < tileCountX; i++)
        {
            for (int n = 0; n < tileCountY; n++)
            {
                GameObject tile = new($"X:{i}, Y:{n}");
                tile.transform.parent = transform;

                Mesh mesh = new();

                tile.AddComponent<MeshFilter>().mesh = mesh;
                tile.AddComponent<MeshRenderer>();

                float x = i * tileWidth;
                float y = n * tileWidth;

                Vector3[] vertices = new Vector3[4];
                vertices[0] = new Vector3(x, 0f, y);
                vertices[1] = new Vector3(x, 0f, y + tileWidth);
                vertices[2] = new Vector3(x + tileWidth, 0f, y);
                vertices[3] = new Vector3(x + tileWidth, 0f, y + tileWidth);

                int[] triangles = { 0, 1, 2, 1, 3, 2 };

                mesh.vertices = vertices;
                mesh.triangles = triangles;

                mesh.RecalculateNormals();
                mesh.RecalculateBounds();

                tiles[i, n] = tile;
            }
        }




    }


    private BaseUnit GeneratePiece(Factions faction, UnitTypes type) 
    {

        BaseUnit bs =  Instantiate(prefabs[(int)type-1], transform).GetComponent<BaseUnit>();
        return bs;
    }

    private List<BaseUnit> GenerateDecks(Deck deck) 
    {
        List<BaseUnit> baseunits = new();
        int i = 1;
        foreach (UnitTypes a in deck.GetHoleListUnit())
        {
            BaseUnit bs = Instantiate(prefabs[(int) a-1], transform).GetComponent<BaseUnit>();
            baseunits.Add(bs);
            bs.X = i;
            bs.Y = 0;
            if (a== UnitTypes.BasicMelee)// ez azért van mert van egy hiba a prefabba ami azt csinálja, h az y tengelyen egyel lelyebb megy a 3D model
            {
                bs.transform.position = new Vector3(bs.X * SIZEOFATILE - (SIZEOFATILE / 2), 1, bs.Y * SIZEOFATILE + (SIZEOFATILE / 2));

            }
            else
            {
                bs.transform.position = new Vector3(bs.X * SIZEOFATILE - (SIZEOFATILE / 2), 0, bs.Y * SIZEOFATILE + (SIZEOFATILE / 2));
            }
                
          
            i++;

        }
        return baseunits;
        
    }


}
