
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
    [SerializeField] private Material grassMaterial;
    [Header("Decorations")]
    [SerializeField] private GameObject[] flowerPrefabs; // Change this to an array
    [UnityEngine.Range(0, 1)]
    [SerializeField] private float flowerChance = 0.2f;

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

                // Assign the grass material to the renderer
                MeshRenderer renderer = tile.AddComponent<MeshRenderer>();
                renderer.material = grassMaterial;

                float x = i * tileWidth;
                float y = n * tileWidth;

                Vector3[] vertices = new Vector3[4];
                vertices[0] = new Vector3(x, 0f, y);
                vertices[1] = new Vector3(x, 0f, y + tileWidth);
                vertices[2] = new Vector3(x + tileWidth, 0f, y);
                vertices[3] = new Vector3(x + tileWidth, 0f, y + tileWidth);

                // NEW: UV Mapping (tells the texture how to wrap)
                Vector2[] uv = new Vector2[4];
                uv[0] = new Vector2(0, 0);
                uv[1] = new Vector2(0, 1);
                uv[2] = new Vector2(1, 0);
                uv[3] = new Vector2(1, 1);

                int[] triangles = { 0, 1, 2, 1, 3, 2 };

                mesh.vertices = vertices;
                mesh.uv = uv; // Don't forget this!
                mesh.triangles = triangles;

                mesh.RecalculateNormals();
                mesh.RecalculateBounds();

                tiles[i, n] = tile;
                if (Random.value < flowerChance)
                {
                    SpawnFlowerOnTile(i, n, tileWidth);
                }
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

    private void SpawnFlowerOnTile(int i, int n, float tileWidth)
    {
        float xPos = i * tileWidth + (tileWidth / 2f);
        float zPos = n * tileWidth + (tileWidth / 2f);

        // Choose a random flower from your list of 8
        int randomIndex = UnityEngine.Random.Range(0, flowerPrefabs.Length);
        GameObject selectedPrefab = flowerPrefabs[randomIndex];

        Vector3 flowerPos = new Vector3(xPos, 0f, zPos);

        GameObject flower = Instantiate(selectedPrefab, flowerPos, Quaternion.identity);
        flower.transform.parent = transform;

        // Random rotation makes the repeated 8 flowers look like a unique meadow
        flower.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
    }


}
