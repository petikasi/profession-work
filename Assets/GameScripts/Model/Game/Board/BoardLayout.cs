using Assets.GameScripts.Model.Game.Board;
using Assets.GameScripts.Model.Game.GameControllerFolder;
using Assets.GameScripts.ViewModel.Game.UnitSelectorCanvas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameScripts.Model.Game.Board { 
    public class BoardLayout : MonoBehaviour
    {
        public static BoardLayout Instance { get; private set; }

        [Header("Board Settings")]
        [SerializeField] private int widthOfTable = 15;
        [SerializeField] private int heightOfTable = 10;
        [SerializeField] private int sizeOfTile = 10;

        [Header("Floor")]
        private GameObject floor;

        [Header("Materials")]
        [SerializeField] private Material grassMaterial;

        [Header("Prefabs")]
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private GameObject[] baseGrassPrefabs;
        [SerializeField] private GameObject[] detailFlowerPrefabs;

        [Header("Highlights")]
        [SerializeField] private GameObject movementHighlightPrefab;

        [Header("Meadow Density Settings")]
        [UnityEngine.Range(0, 1)]
        [SerializeField] private float decorationChance = 1.0f;
        [SerializeField] private int densityPerTile = 20;
        [SerializeField] private float scaleMin = 2.5f;
        [SerializeField] private float scaleMax = 4.5f;
        [SerializeField] private float pivotOffset = 0.5f;

        public int GET_SIZE => sizeOfTile;
        public int Width => widthOfTable;
        public int Height => heightOfTable;

        private bool isGenerated = false;
        private UnitRegistry unitRegistry = new();
        public int PLAYER_ZONE = 4;

        private List<GameObject> activeHighlightObjects = new List<GameObject>();
        // Nyilvántartjuk a futó animációkat egységenként
        private Dictionary<Transform, Coroutine> activeMovements = new Dictionary<Transform, Coroutine>();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public BaseUnit GetUnitAt(int x, int z)
        {
            // Megjegyzés: Teljesítmény szempontjából érdemesebb a UserGameController.Instance.GetUnitAt(x, z)-t használni!
            BaseUnit[] allUnits = FindObjectsOfType<BaseUnit>();
            foreach (BaseUnit unit in allUnits)
            {
                if (unit.TileX == x && unit.TileZ == z)
                {
                    return unit;
                }
            }
            return null;
        }

        public bool IsTileOccupied(int x, int z)
        {
            return GetUnitAt(x, z) != null;
        }

        public void HighlightMovementTiles(List<Vector2Int> tiles)
        {
            ClearHighlightVisuals();

            GameObject container = GameObject.Find("MovementHighlightsContainer");
            if (container == null)
            {
                container = new GameObject("MovementHighlightsContainer");
                container.transform.SetParent(transform);
            }

            foreach (Vector2Int tile in tiles)
            {
                Vector3 worldPos = GetWorldPositionFromTile(tile.x, tile.y);
                worldPos.y = 0.05f;

                if (movementHighlightPrefab != null)
                {
                    GameObject highlightObj = Instantiate(movementHighlightPrefab, worldPos, Quaternion.identity, container.transform);
                    activeHighlightObjects.Add(highlightObj);
                }
                else
                {
                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.position = worldPos;
                    quad.transform.rotation = Quaternion.Euler(90, 0, 0);
                    quad.transform.localScale = new Vector3(sizeOfTile, sizeOfTile, 1f);
                    quad.transform.SetParent(container.transform);

                    if (quad.TryGetComponent<Collider>(out var col)) Destroy(col);
                    if (quad.TryGetComponent<MeshRenderer>(out var ren))
                    {
                        ren.material = new Material(Shader.Find("Sprites/Default"));
                        ren.material.color = new Color(0f, 0.5f, 1f, 0.4f);
                    }

                    activeHighlightObjects.Add(quad);
                }
            }
        }

        public void ClearHighlightVisuals()
        {
            foreach (GameObject obj in activeHighlightObjects)
            {
                if (obj != null) Destroy(obj);
            }
            activeHighlightObjects.Clear();
        }

        public void GenerateBoardLayout()
        {
            if (isGenerated) return;
            isGenerated = true;

            GenerateOneBigFloor(widthOfTable, heightOfTable, sizeOfTile);
            PopulateEntireMeadow(widthOfTable, heightOfTable, sizeOfTile);
            FitCameraToMap();
            GenerateGridLines();
            StaticBatchingUtility.Combine(gameObject);
            unitRegistry.InitializeRegistry(prefabs);
            CreateAreaForUnits();
        }

        private void GenerateOneBigFloor(int tileCountX, int tileCountZ, float tileWidth)
        {
            floor = new("GrandFloor");
            floor.transform.parent = transform;
            floor.isStatic = true;

            MeshFilter meshFilter = floor.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = floor.AddComponent<MeshRenderer>();
            meshRenderer.material = grassMaterial;

            Mesh mesh = new();
            int vCount = (tileCountX + 1) * (tileCountZ + 1);
            Vector3[] vertices = new Vector3[vCount];
            Vector2[] uvs = new Vector2[vCount];

            for (int n = 0, z = 0; z <= tileCountZ; z++)
            {
                for (int x = 0; x <= tileCountX; x++)
                {
                    vertices[n] = new Vector3(x * tileWidth, 0, z * tileWidth);
                    uvs[n] = new Vector2(x, z);
                    n++;
                }
            }

            int[] triangles = new int[tileCountX * tileCountZ * 6];
            int vert = 0;
            int tris = 0;
            for (int z = 0; z < tileCountZ; z++)
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
                // JAVÍTVA: Random.Range felsõ határa kizáró, így tömb-hosszúságot adunk meg a (Length - 1) helyett
                GameObject prefab = (Random.value < 0.7f)
                    ? baseGrassPrefabs[Random.Range(0, baseGrassPrefabs.Length)]
                    : detailFlowerPrefabs[Random.Range(0, detailFlowerPrefabs.Length)];

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

        public BaseUnit PlaceUnitAtTile(int x, int z, FactionsEnum fac, UnitTypesEnum unit, UnitCountpanel cardPanel)
        {
            UnitRegistryEntry entry = unitRegistry.GetEntry(unit, fac);

            if (entry == null)
            {
                Debug.LogError($"BoardLayout: Nem található bejegyzés a regiszterben ehhez az egységhez: {fac} - {unit}");
                return null;
            }

            Vector3 spawnPos = GetWorldPositionFromTile(x, z);

            GameObject unitObj = Instantiate(entry.Prefab, spawnPos, Quaternion.identity);
            unitObj.transform.rotation = Quaternion.Euler(-90, 0, 0);

            BaseUnit unitScript = unitObj.GetComponent(entry.ScriptType) as BaseUnit;

            if (unitScript == null)
            {
                unitScript = unitObj.AddComponent(entry.ScriptType) as BaseUnit;
                Debug.Log($"BoardLayout: A script nem volt rajta a Prefabon, ezért dinamikusan hozzáadtam: {entry.ScriptType.Name}");
            }

            if (unitScript != null)
            {
                unitScript.TileX = x;
                unitScript.TileZ = z;
                unitScript.MyCardPanel = cardPanel;
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
            lineMat.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);

            for (int z = 0; z <= heightOfTable; z++)
            {
                CreateLine(
                    new Vector3(0, 0.1f, z * sizeOfTile),
                    new Vector3(widthOfTable * sizeOfTile, 0.1f, z * sizeOfTile),
                    gridContainer.transform, lineMat
                );
            }

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

        private void CreateAreaForUnits()
        {
            float zoneWidth = widthOfTable * sizeOfTile;
            float zoneHeight = PLAYER_ZONE * sizeOfTile;

            Vector3 p1Center = new(zoneWidth / 2f, 0.01f, zoneHeight / 2f);
            CreateZoneVisual(p1Center, zoneWidth, zoneHeight, "Player1_Zone");

            float fullMapHeight = heightOfTable * sizeOfTile;
            Vector3 p2Center = new(zoneWidth / 2f, 0.01f, fullMapHeight - (zoneHeight / 2f));
            CreateZoneVisual(p2Center, zoneWidth, zoneHeight, "Player2_Zone");
        }

        private void CreateZoneVisual(Vector3 center, float width, float height, string name)
        {
            GameObject zone = GameObject.CreatePrimitive(PrimitiveType.Quad);
            zone.name = name;
            zone.transform.SetParent(floor.transform);

            zone.transform.position = center;
            zone.transform.rotation = Quaternion.Euler(90, 0, 0);
            zone.transform.localScale = new Vector3(width, height, 1f);

            if (zone.TryGetComponent<MeshCollider>(out var collider))
            {
                Destroy(collider);
            }

            if (zone.TryGetComponent<MeshRenderer>(out var renderer))
            {
                renderer.material = new Material(Shader.Find("Sprites/Default"));
                renderer.material.color = new Color(0f, 0f, 0f, 0.7f);
            }
        }

        public void RemoveZoneVisuals()
        {
            Transform p1Zone = floor.transform.Find("Player1_Zone");
            if (p1Zone != null) Destroy(p1Zone.gameObject);

            Transform p2Zone = floor.transform.Find("Player2_Zone");
            if (p2Zone != null) Destroy(p2Zone.gameObject);
        }

        public Vector3 GetWorldPositionFromTile(int x, int z)
        {
            float worldX = (x * sizeOfTile) + (sizeOfTile / 2f);
            float worldZ = (z * sizeOfTile) + (sizeOfTile / 2f);

            return new Vector3(worldX, 0.5f, worldZ);
        }

        public void MoveUnitOnBoard(BaseUnit unit, int targetX, int targetZ, float duration = 1.0f)
        {
            if (unit == null) return;

            Vector3 startWorldPosition = unit.transform.position;
            Vector3 targetWorldPosition = GetWorldPositionFromTile(targetX, targetZ);

            unit.TileX = targetX;
            unit.TileZ = targetZ;

            // JAVÍTVA: Elõzõ Coroutine leállítása, ha az egység még mozgásban volt
            if (activeMovements.TryGetValue(unit.transform, out Coroutine runningCoroutine))
            {
                if (runningCoroutine != null) StopCoroutine(runningCoroutine);
            }

            activeMovements[unit.transform] = StartCoroutine(AnimateUnitMovement(unit.transform, startWorldPosition, targetWorldPosition, duration));
        }

        private IEnumerator AnimateUnitMovement(Transform unitTransform, Vector3 startPos, Vector3 targetPos, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                if (unitTransform == null) yield break;

                unitTransform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (unitTransform != null)
            {
                unitTransform.position = targetPos;
                // Töröljük a lefutott Coroutine-t a nyilvántartásból
                activeMovements.Remove(unitTransform);
            }
        }
    }
}