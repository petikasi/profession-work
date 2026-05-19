
using System.Collections.Generic;
using System.Linq;
using Assets.GameScripts.Model.Game.Board;
using Assets.GameScripts.Model.Game.GameController;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
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
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject[] baseGrassPrefabs;
    [SerializeField] private GameObject[] detailFlowerPrefabs;

    [Header("Meadow Density Settings")]
    [UnityEngine.Range(0, 1)]
    [SerializeField] private float decorationChance = 1.0f;
    [SerializeField] private int densityPerTile = 20;
    [SerializeField] private float scaleMin = 2.5f;
    [SerializeField] private float scaleMax = 4.5f;
    [SerializeField] private float pivotOffset = 0.5f;

    private bool isGenerated = false;
    private UnitRegistry unitRegistry;

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
            GameObject prefab = (Random.value < 0.7f) ? baseGrassPrefabs[Random.Range(0, baseGrassPrefabs.Length-1)] : detailFlowerPrefabs[Random.Range(0, detailFlowerPrefabs.Length-1)];
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
        // 1. Példányosítás
        UnitRegistry registry = new();

        registry.InitializeRegistry(this.prefabs);

        var unitsToSpawn = DeckManagerController.Instance.SelectedDeck.GetHoleListUnit();
        Factions faction = DeckManagerController.Instance.SelectedDeck.FactionsGet; 

        for (int i = 0; i < unitsToSpawn.Count; i++)
        {
            UnitTypes type = unitsToSpawn[i];
            UnitRegistryEntry entry = registry.GetEntry(type, faction);

            if (entry != null)
            {

                PlaceUnitAtTile(i % widthOfTable, i / widthOfTable, entry.Prefab, entry.ScriptType);
            }
            else
            {
                Debug.LogError($"Unit is not registrated: {type} to the current faction : {faction}");
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

    /// <summary>
    /// Létrehoz egy egyenest a 3D térben két pont között egy LineRenderer komponens segítségével.
    /// </summary>
    /// <param name="start">A vonal kezdõpontjának világkoordinátája (Vector3)</param>
    /// <param name="end">A vonal végpontjának világkoordinátája (Vector3)</param>
    /// <param name="parent">A szülõ objektum (Transform), ami alá a létrejött vonal strukturálisan tartozni fog</param>
    /// <param name="mat">A vonal megjelenítéséhez használt anyag (Material/Shader)</param>
    private void CreateLine(Vector3 start, Vector3 end, Transform parent, Material mat)
    {
        // 1. Létrehozunk egy teljesen új, üres GameObjectet a hierarchiában "GridLine" névvel
        GameObject lineObj = new("GridLine");

        // 2. Beállítjuk a vonal objektum szülõjét, hogy ne ömlesztve legyen a Scene gyökerében (pl. a Grid alá)
        lineObj.transform.parent = parent;

        // 3. Rápakolunk egy LineRenderer komponenst a frissen létrehozott objektumra, ez felel a vonal kirajzolásáért
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        // 4. Átadjuk a kapott anyagot (színt/textúrát) a LineRenderernek, különben csúnya rózsaszín (Missing Material) lenne
        lr.material = mat;

        // 5. Beállítjuk a vonal vastagságát a kezdõpontjánál (0.5 egység széles)
        lr.startWidth = 0.5f;

        // 6. Beállítjuk a vonal vastagságát a végpontjánál (mivel ez is 0.5f, így végig egyenletes vastagságú lesz)
        lr.endWidth = 0.5f;

        // 7. Engedélyezzük a világkoordináták használatát. Ha a szülõ objektum elmozdul, a vonal pontjai fixen a helyükön maradnak
        lr.useWorldSpace = true;

        // 8. Megadjuk, hogy a vonalunk hány töréspontból áll. Mivel ez egy egyenes, pontosan 2 pontra van szükségünk
        lr.positionCount = 2;

        // 9. Beállítjuk a vonal legelsõ pontját (0-s index) a megadott kezdõpont koordinátáira
        lr.SetPosition(0, start);

        // 10. Beállítjuk a vonal második pontját (1-es index) a megadott végpont koordinátáira
        lr.SetPosition(1, end);

        // 11. Statikusnak jelöljük az objektumot. Mivel a rácsvonalak nem mozognak a játék alatt, 
        // ez segít a Unity-nek optimalizálni a renderelést (Batching), így jobb lesz a teljesítmény (FPS)
        lineObj.isStatic = true;
    }
}
