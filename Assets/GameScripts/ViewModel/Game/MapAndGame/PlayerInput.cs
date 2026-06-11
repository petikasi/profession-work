using Assets.GameScripts.Model.Game.GameControllerFolder;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Assets.GameScripts.ViewModel.Game.MapAndGame
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private float sizeOfTile = 10f;

        void Update()
        {
            // Ha nincs egér csatlakoztatva, ne csináljon semmit
            if (Mouse.current == null) return;

            // BAL KLIKK ÉRZÉKELÉSE (Lehelyezés)
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                HandleSelection(isRightClick: false);
            }
            // JOBB KLIKK ÉRZÉKELÉSE (Visszavétel)
            else if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                HandleSelection(isRightClick: true);
            }
        }

        private void HandleSelection(bool isRightClick)
        {

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                // Azonnal megszakítjuk a függvény futását, így nem fog tovább raycastolni a pályára!
                return;
            }

            // Megszerezzük az egér aktuális pozícióját a képernyőn
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Ellenőrizzük, hogy a nagy generált padlóra kattintottunk-e
                if (hit.collider.name == "GrandFloor")
                {
                    // Kiszámoljuk a rács koordinátáit
                    int tileX = Mathf.FloorToInt(hit.point.x / sizeOfTile);
                    int tileZ = Mathf.FloorToInt(hit.point.z / sizeOfTile);

                    Debug.Log($"Kattintott mező: {tileX}, {tileZ} | Jobb klikk: {isRightClick}");

                    if (GameController.Instance != null)
                    {
                        // Továbbítjuk a koordinátákat ÉS azt, hogy jobb klikk volt-e
                        GameController.Instance.CallPlacing(tileX, tileZ, isRightClick);
                    }
                    else
                    {
                        Debug.LogError("PlayerInput: A GameController.Instance NULL!");
                    }
                }
            }
        }
    }
}
