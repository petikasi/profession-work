
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

        // 1. Create the big floor (One object instead of 2400!)
        GenerateOneBigFloor(widthOfTable, heightOfTable, sizeOfTile);

        // 2. Spawn the flowers on top using the same grid logic
        PopulateEntireMeadow(widthOfTable, heightOfTable, sizeOfTile);

        // 3. Optional: Combine all flowers for extra performance
        StaticBatchingUtility.Combine(gameObject);
    }

    private void GenerateOneBigFloor(int tileCountX, int tileCountY, float tileWidth)
    {
        GameObject floor = new GameObject("GrandFloor");
        floor.transform.parent = transform;
        floor.isStatic = true;

        MeshFilter meshFilter = floor.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = floor.AddComponent<MeshRenderer>();
        meshRenderer.material = grassMaterial;

        Mesh mesh = new Mesh();

        // We create vertices for the WHOLE table at once
        int vCount = (tileCountX + 1) * (tileCountY + 1);
        Vector3[] vertices = new Vector3[vCount];
        Vector2[] uvs = new Vector2[vCount];

        for (int n = 0, z = 0; z <= tileCountY; z++)
        {
            for (int x = 0; x <= tileCountX; x++)
            {
                vertices[n] = new Vector3(x * tileWidth, 0, z * tileWidth);
                // This makes the texture repeat on every tile
                uvs[n] = new Vector2(x, z);
                n++;
            }
        }

        // Define the triangles for all 2,400 tiles
        int[] triangles = new int[tileCountX * tileCountY * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < tileCountY; z++)
        {
            for (int x = 0; x < tileCountX; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + tileCountX + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + tileCountX + 1;
                triangles[tris + 5] = vert + tileCountX + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        // IMPORTANT: Add a MeshCollider so you can still "Click" the floor
        floor.AddComponent<MeshCollider>();
    }

    private void PopulateEntireMeadow(int tileCountX, int tileCountY, float tileWidth)
    {
        // Create one container for all flowers to keep the Hierarchy clean
        GameObject flowerContainer = new GameObject("FlowerContainer");
        flowerContainer.transform.parent = transform;

        for (int i = 0; i < tileCountX; i++)
        {
            for (int n = 0; n < tileCountY; n++)
            {
                // Only spawn if our chance is hit
                if (Random.value < decorationChance)
                {
                    // We reuse your Lush Meadow logic here!
                    SpawnFlowerCluster(i, n, tileWidth, flowerContainer.transform);
                }
            }
        }
    }

    private void SpawnFlowerCluster(int i, int n, float tileWidth, Transform parent)
    {
        float xBase = i * tileWidth + (tileWidth / 2f);
        float zBase = n * tileWidth + (tileWidth / 2f);

        for (int s = 0; s < densityPerTile; s++)
        {
            GameObject prefab = (Random.value < 0.7f) ? baseGrassPrefabs[0] : detailFlowerPrefabs[0];

            float jitter = tileWidth * 0.6f;
            Vector3 pos = new Vector3(
                xBase + Random.Range(-jitter, jitter),
                0.05f + (pivotOffset * 0.1f), // Lifted slightly
                zBase + Random.Range(-jitter, jitter)
            );

            GameObject go = Instantiate(prefab, pos, Quaternion.identity, parent);
            go.isStatic = true;
            go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            go.transform.localScale = Vector3.one * Random.Range(scaleMin, scaleMax);

            // Adjust pivot manually via code
            foreach (Transform child in go.transform)
            {
                child.localPosition = new Vector3(0, pivotOffset, 0);
            }
        }
    }
}
