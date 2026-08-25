using Assets.GameScripts.Model.Game.Enums;
using Assets.GameScripts.Model.Game.Board;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Assets.GameScripts.ViewModel.Game.MapAndGame
{
    public class PlayerInput : MonoBehaviour
    {
        void Update()
        {
            if (Mouse.current == null) return;

            // Bal klikk érzékelése
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                HandleInput(isRightClick: false);
            }
            // Jobb klikk érzékelése
            else if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                HandleInput(isRightClick: true);
            }
        }

        private void HandleInput(bool isRightClick)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.name == "GrandFloor")
                {
                    int tileSize = BoardLayout.Instance.GET_SIZE;
                    int tileX = Mathf.FloorToInt(hit.point.x / tileSize);
                    int tileZ = Mathf.FloorToInt(hit.point.z / tileSize);

                    if (UserGameController.Instance == null)
                    {
                        Debug.LogError("PlayerInput: UserGameController.Instance NULL!");
                        return;
                    }

                    GamePhaseEnum currentPhase = UserGameController.Instance.OwnPlayer.CurrentGamePhase;

                    if (currentPhase == GamePhaseEnum.Deployment)
                    {
                        UserGameController.Instance.CallPlacing(tileX, tileZ, isRightClick);
                    }
                    else if (currentPhase == GamePhaseEnum.Movement)
                    {
                        if (!isRightClick)
                        {
                            UserGameController.Instance.SelectUnitToMove(tileX, tileZ);
                        }
                        else
                        {
     
                            UserGameController.Instance.MovingUnit(tileX, tileZ);
                        }
                    }
                }
            }
        }
    }
}
