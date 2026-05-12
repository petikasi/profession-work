
using System.Collections.Generic;
using System.Linq;
using Assets.GameScripts.Model.Game.GameController;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using Assets.GameScripts.Persistence.Database;
using UnityEngine.TestTools;

public class BoardLayout : MonoBehaviour
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

    [Header("DataBaseForUnits")]
    [SerializeField] private UnitDatabase unitDatabase;

    private bool isGenerated = false;

    public void GenerateBoardLayout()
    {
        if (isGenerated) return;
        isGenerated = true;

        // 1. Padló legenerálása
        GenerateOneBigFloor(widthOfTable, heightOfTable, sizeOfTile);

        // 2. Növényzet szórása
        PopulateEntireMeadow(widthOfTable, heightOfTable, sizeOfTile);

        // 3. Egységek lehelyezése
        GenerateDecks();

        // 4. Kamera pozicionálása
        FitCameraToMap();

        // 5. Háló kirajzolása 
        GenerateGridLines();

        // 6. Optimalizálás (Batching)
        StaticBatchingUtility.Combine(gameObject);
    }

    //Ez a methódus feleõs a pálya legenerálásáért
    private void GenerateOneBigFloor(int tileCountX, int tileCountY, float tileWidth)
    {
        GameObject floor = new("GrandFloor");
        floor.transform.parent = transform;
        floor.isStatic = true;

        MeshFilter meshFilter = floor.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = floor.AddComponent<MeshRenderer>();
        meshRenderer.material = grassMaterial;

        Mesh mesh = new();
        int vCount = (tileCountX + 1) * (tileCountY + 1);
        Vector3[] vertices = new Vector3[vCount];
        Vector2[] uvs = new Vector2[vCount];

        for (int n = 0, z = 0; z <= tileCountY; z++)
        {
            for (int x = 0; x <= tileCountX; x++)
            {
                vertices[n] = new Vector3(x * tileWidth, 0, z * tileWidth);
                uvs[n] = new Vector2(x, z);
                n++;
            }
        }

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
        floor.AddComponent<MeshCollider>();
    }

    private void PopulateEntireMeadow(int tileCountX, int tileCountY, float tileWidth)
    {
        GameObject flowerContainer = new("FlowerContainer");
        flowerContainer.transform.parent = transform;

        for (int i = 0; i < tileCountX; i++)
        {
            for (int n = 0; n < tileCountY; n++)
            {
                if (Random.value < decorationChance)
                {
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
            Vector3 pos = new(
                xBase + Random.Range(-jitter, jitter),
                0.05f + (pivotOffset * 0.1f),
                zBase + Random.Range(-jitter, jitter)
            );

            GameObject go = Instantiate(prefab, pos, Quaternion.identity, parent);
            go.isStatic = true;
            go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            go.transform.localScale = Vector3.one * Random.Range(scaleMin, scaleMax);

            foreach (Transform child in go.transform)
            {
                child.localPosition = new Vector3(0, pivotOffset, 0);
            }
        }
    }

    private void GenerateDecks()
    {
        var unitsToSpawn = DeckManagerController.Instance.SelectedDeck.GetHoleListUnit();

        for (int i = 0; i < unitsToSpawn.Count; i++)
        {
            UnitTypes type = unitsToSpawn[i];
            UnitData data = unitDatabase.GetUnitData(type);

            if (data != null)
            {
                // Típus lekérése név alapján (Reflection)
                System.Type scriptType = System.Type.GetType(data.scriptClassName);

                // Ha a script egy mappában (namespace-ben) van, használd a teljes nevet:
                // System.Type.GetType("Assets.Scripts.Elven." + data.scriptClassName);

                PlaceUnitAtTile(i + 2, 0, data.unitPrefab, scriptType ?? typeof(BaseUnit));
            }
        }
    }

    public BaseUnit PlaceUnitAtTile(int x, int y, GameObject unitPrefab, System.Type unitType)
    {
        float worldX = (x * sizeOfTile) + (sizeOfTile / 2f);
        float worldZ = (y * sizeOfTile) + (sizeOfTile / 2f);
        Vector3 spawnPos = new(worldX, 0.5f, worldZ);

        GameObject unitObj = Instantiate(unitPrefab, spawnPos, Quaternion.identity);
        unitObj.transform.rotation = Quaternion.Euler(-90, 0, 0);

        // Dependency Injection: Itt adjuk hozzá vagy kérjük le a komponenst típus alapján
        BaseUnit unitScript = unitObj.GetComponent(unitType) as BaseUnit;

        if (unitScript != null)
        {
            unitScript.TileX = x;
            unitScript.TileY = y;
        }

        return unitScript;
    }
    private void FitCameraToMap()
    {
        float centerX = (widthOfTable * sizeOfTile) / 2f;
        float centerZ = (heightOfTable * sizeOfTile) / 2f;
        Vector3 cameraPos = new(centerX, 150f, centerZ - 100f);
        Camera.main.transform.position = cameraPos;
        Camera.main.transform.LookAt(new Vector3(centerX, 0, centerZ));
    }

    // ELTÁVOLÍTOTTUK A PARAMÉTEREKET, mert az osztály változóit használjuk
    private void GenerateGridLines()
    {
        GameObject gridContainer = new("GridContainer");
        gridContainer.transform.parent = transform;

        Material lineMat = new(Shader.Find("Sprites/Default"));

        // Új szín beállítása: Szürke (0.5f) és 30%-os átlátszóság (0.3f)
        // Color(R, G, B, A) -> 0 az átlátszó, 1 a telített
        lineMat.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);

        // Vízszintes vonalak
        for (int z = 0; z <= heightOfTable; z++)
        {
            CreateLine(
                new Vector3(0, 0.1f, z * sizeOfTile),
                new Vector3(widthOfTable * sizeOfTile, 0.1f, z * sizeOfTile),
                gridContainer.transform, lineMat
            );
        }

        // Függõleges vonalak
        for (int x = 0; x <= widthOfTable; x++)
        {
            CreateLine(
                new Vector3(x * sizeOfTile, 0.1f, 0),
                new Vector3(x * sizeOfTile, 0.1f, heightOfTable * sizeOfTile),
                gridContainer.transform, lineMat
            );
        }
    }

    private void CreateLine(Vector3 start, Vector3 end, Transform parent, Material mat)
    {
        GameObject lineObj = new("GridLine");
        lineObj.transform.parent = parent;
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.material = mat;
        lr.startWidth = 0.5f;
        lr.endWidth = 0.5f;
        lr.useWorldSpace = true;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lineObj.isStatic = true;
    }
}
