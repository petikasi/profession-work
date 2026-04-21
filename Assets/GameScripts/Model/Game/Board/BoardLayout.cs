
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardLayout :MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private  int widthOfTable = 60;
    [SerializeField] private  int heightOfTable = 40;
    [SerializeField] private  int sizeOfTile = 10;

    [Header("Materials")]
    [SerializeField] private Material grassMaterial;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] unitPrefabs;
    [SerializeField] private GameObject[] flowerPrefabs;

    [Header("Generation Settings")]
    [UnityEngine.Range(0, 1)]
    [SerializeField] private float flowerChance = 0.2f;

    private GameObject[,] tiles;
    private bool isGenerated = false;
    private Deck currentDeck;

    public void GenerateBoardLayout()
    {
        if (isGenerated) return;
        isGenerated = true;

        GenerateMapTiles(widthOfTable, heightOfTable, sizeOfTile);

        // Example unit setup
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

        Deck deck = new(myUnits, Factions.Human);
        //GenerateDecks(deck);
    }

    private void GenerateMapTiles(int tileCountX, int tileCountY, float tileWidth)
    {
        tiles = new GameObject[tileCountX, tileCountY];

        for (int i = 0; i < tileCountX; i++)
        {
            for (int n = 0; n < tileCountY; n++)
            {
                // 1. Create Tile Object
                GameObject tile = new($"Tile_{i}_{n}");
                tile.transform.parent = transform;

                // 2. Set up Mesh
                Mesh mesh = new();
                tile.AddComponent<MeshFilter>().mesh = mesh;
                MeshRenderer renderer = tile.AddComponent<MeshRenderer>();
                renderer.material = grassMaterial;

                // 3. Calculate Positions
                float x = i * tileWidth;
                float z = n * tileWidth;

                Vector3[] vertices = new Vector3[]
                {
                    new (x, 0f, z),
                    new (x, 0f, z + tileWidth),
                    new (x + tileWidth, 0f, z),
                    new (x + tileWidth, 0f, z + tileWidth)
                };

                Vector2[] uv = new Vector2[]
                {
                    new (0, 0),
                    new (0, 1),
                    new (1, 0),
                    new (1, 1)
                };

                int[] triangles = { 0, 1, 2, 1, 3, 2 };

                mesh.vertices = vertices;
                mesh.uv = uv;
                mesh.triangles = triangles;

                mesh.RecalculateNormals();
                mesh.RecalculateBounds();

                tiles[i, n] = tile;

                // 4. Decoration Spawning
                if (Random.value < flowerChance && flowerPrefabs.Length > 0)
                {
                    SpawnFlowerOnTile(i, n, tileWidth);
                }
            }
        }
    }

    private void SpawnFlowerOnTile(int i, int n, float tileWidth)
    {
        // Center the flower on the tile
        float xPos = i * tileWidth + (tileWidth / 2f);
        float zPos = n * tileWidth + (tileWidth / 2f);

        // Raise it slightly (0.01) to prevent "Z-Fighting" with the floor
        Vector3 flowerPos = new(xPos, 0.01f, zPos);

        // Pick random flower
        GameObject randomFlower = flowerPrefabs[0];

        GameObject flower = Instantiate(randomFlower, flowerPos, Quaternion.identity, transform);

        // Randomize rotation and slightly randomize scale for a natural look
        flower.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        float randomScale = Random.Range(0.8f, 1.2f);
        flower.transform.localScale *= randomScale;
    }

    private List<BaseUnit> GenerateDecks(Deck deck)
    {
        List<BaseUnit> spawnedUnits = new();
        List<UnitTypes> list = deck.GetHoleListUnit();

        for (int i = 0; i < list.Count; i++)
        {
            UnitTypes type = list[i];
            BaseUnit bs = Instantiate(unitPrefabs[(int)type - 1], transform).GetComponent<BaseUnit>();

            bs.X = i + 1; // Start at 1 per your original logic
            bs.Y = 0;

            // Positioning logic
            float xOffset = bs.X * sizeOfTile - (sizeOfTile / 2f);
            float zOffset = bs.Y * sizeOfTile + (sizeOfTile / 2f);
            float yOffset = (type == UnitTypes.BasicMelee) ? 1f : 0f;

            bs.transform.position = new Vector3(xOffset, yOffset, zOffset);
            spawnedUnits.Add(bs);
        }

        return spawnedUnits;
    }


}
