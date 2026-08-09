
using UnityEngine;

namespace Assets.GameScripts.Model.Game.GameControllerFolder
{
    public class MovementHandlingController : MonoBehaviour
    {
        public static MovementHandlingController Instance { get; private set; }

        private BaseUnit activeMovingUnit = null;

        TurnModel turn;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        private void Start()
        {
            turn = new TurnModel(UserGameController.Instance.GetPlayer);
        }
        public bool HandleMovement(int x, int z, bool isRightClick)
        {
 
            if (UserGameController.Instance == null || !turn.Getgamestarted)
            {
                return false;
            }

            if (isRightClick)
            {
                if (activeMovingUnit != null)
                {
                    ClearMovementSelection();
                    return true; 
                }
                return false; 
            }

            BaseUnit clickedUnit = UserGameController.Instance.GetUnitAt(x, z);

            if (activeMovingUnit == null)
            {
                if (clickedUnit != null)
                {
                    activeMovingUnit = clickedUnit;
                    Debug.Log($"[Movement] Egység kijelölve: {clickedUnit.name} ({x}, {z}). Sebesség: {activeMovingUnit.MovementSpeed}");
                    return true; 
                }
                return false;
            }


            else
            {
                if (clickedUnit == activeMovingUnit)
                {
                    ClearMovementSelection();
                    return true;
                }

                if (clickedUnit != null)
                {
                    Debug.LogWarning("[Movement] A célmező foglalt!");
                    return true;
                }
                int distanceX = Mathf.Abs(x - activeMovingUnit.TileX);
                int distanceZ = Mathf.Abs(z - activeMovingUnit.TileZ);
                int totalDistance = distanceX + distanceZ;

                if (totalDistance <= activeMovingUnit.MovementSpeed)
                {
                    Debug.Log($"[Movement] Sikeres lépés: ({activeMovingUnit.TileX}, {activeMovingUnit.TileZ}) -> ({x}, {z})");

                    activeMovingUnit.TileX = x;
                    activeMovingUnit.TileZ = z;

                    float tileSize = 10f;
                    Vector3 newWorldPosition = new(
                        x * tileSize + (tileSize / 2f),
                        activeMovingUnit.transform.position.y,
                        z * tileSize + (tileSize / 2f)
                    );

                    activeMovingUnit.transform.position = newWorldPosition;

                    ClearMovementSelection();
                }
                else
                {
                    Debug.LogWarning($"[Movement] Túl messze van! Távolság: {totalDistance}, Max sebesség: {activeMovingUnit.MovementSpeed}");
                }

                return true; // Akár sikeres volt a lépés, akár túl messze volt, a kattintást elnyeltük
            }
        }

        public void ClearMovementSelection()
        {
            activeMovingUnit = null;
            Debug.Log("[Movement] Mozgás kijelölés törölve.");
        }
    }
}
