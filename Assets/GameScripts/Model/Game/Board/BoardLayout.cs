
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardLayout :MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private int widthOfTable = 15;
    [SerializeField] private int heightOfTable = 10;
    [SerializeField] private int sizeOfTile = 10;

    [Header("Materials")]
    [SerializeField] private Material grassMaterial;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] unitPrefabs;
    [SerializeField] private GameObject[] baseGrassPrefabs;
    [SerializeField] private GameObject[] detailFlowerPrefabs;

    [Header("Meadow Density Settings")]
    [UnityEngine.Range(0, 1)]
    [SerializeField] private float decorationChance = 1.0f;
    [SerializeField] private int densityPerTile = 20;
    [SerializeField] private float scaleMin = 2.5f;
    [SerializeField] private float scaleMax = 4.5f;
    [SerializeField] private float pivotOffset = 0.5f;

    private GameObject[,] tiles;
    private bool isGenerated = false;

    public void GenerateBoardLayout()
    {
        if (isGenerated) return;
        isGenerated = true;

        GenerateMapTiles();

        // Final step: Static Batching for the whole board
        // This combines all meshes to reduce Draw Calls significantly
        StaticBatchingUtility.Combine(gameObject);
    }

    private void GenerateMapTiles()
    {
        tiles = new GameObject[widthOfTable, heightOfTable];

        for (int i = 0; i < widthOfTable; i++)
        {
            for (int n = 0; n < heightOfTable; n++)
            {
                // Create the Tile container
                GameObject tile = new($"Tile_{i}_{n}");
                tile.transform.parent = transform;
                // Performance: Mark as static so Unity can batch it
                tile.isStatic = true;

                // Setup the floor mesh
                Mesh mesh = new();
                tile.AddComponent<MeshFilter>().mesh = mesh;
                MeshRenderer renderer = tile.AddComponent<MeshRenderer>();
                renderer.material = grassMaterial;
                // Performance: Ensure floor can be instanced/batched
                renderer.realtimeLightmapIndex = -1;

                float x = i * sizeOfTile;
                float z = n * sizeOfTile;

                mesh.vertices = new Vector3[]
                {
                    new (x, 0f, z),
                    new (x, 0f, z + sizeOfTile),
                    new (x + sizeOfTile, 0f, z),
                    new (x + sizeOfTile, 0f, z + sizeOfTile)
                };

                mesh.uv = new Vector2[] { new(0, 0), new(0, 1), new(1, 0), new(1, 1) };
                mesh.triangles = new int[] { 0, 1, 2, 1, 3, 2 };

                mesh.RecalculateNormals();
                mesh.RecalculateBounds();

                tiles[i, n] = tile;

                if (Random.value < decorationChance)
                {
                    // Pass the 'tile' as parent to help with culling and grouping
                    PopulateLushMeadow(i, n, sizeOfTile, tile.transform);
                }
            }
        }
    }

    private void PopulateLushMeadow(int i, int n, float tileWidth, Transform parentTile)
    {
        float xBase = i * tileWidth + (tileWidth / 2f);
        float zBase = n * tileWidth + (tileWidth / 2f);

        for (int s = 0; s < densityPerTile; s++)
        {
            GameObject prefabToSpawn;
            if (Random.value < 0.7f && baseGrassPrefabs.Length > 0)
                prefabToSpawn = baseGrassPrefabs[Random.Range(0, baseGrassPrefabs.Length)];
            else if (detailFlowerPrefabs.Length > 0)
                prefabToSpawn = detailFlowerPrefabs[Random.Range(0, detailFlowerPrefabs.Length)];
            else
                continue;

            float jitter = tileWidth * 0.6f;
            float xPos = xBase + Random.Range(-jitter, jitter);
            float zPos = zBase + Random.Range(-jitter, jitter);

            Vector3 finalPos = new(xPos, 0.02f + (pivotOffset * 0.1f), zPos);

            // Use parentTile instead of 'transform' to group objects by tile
            GameObject go = Instantiate(prefabToSpawn, finalPos, Quaternion.identity, parentTile);

            // Performance: Tell Unity this flower will never move
            go.isStatic = true;

            go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            float sFactor = Random.Range(scaleMin, scaleMax);
            go.transform.localScale = Vector3.one * sFactor;

            foreach (Transform child in go.transform)
            {
                child.localPosition = new(0, pivotOffset, 0);
            }
        }
    }
}
