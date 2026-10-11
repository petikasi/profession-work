using Assets.GameScripts.Model.Game.Enums;
using Assets.GameScripts.Model.Game.Board;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Assets.GameScripts.ViewModel.Game.MapAndGame
{
    public class PlayerInput : MonoBehaviour
    {
        private LayerMask floorLayer;

        private void Awake()
        {
            floorLayer = LayerMask.GetMask("Floor");
        }


        private void Update()
        {
            if (Mouse.current == null) return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                HandleInput(false);
            }
            else if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                HandleInput(true);
            }
        }

        private void HandleInput(bool isRightClick)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Debug.Log(isRightClick);

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                int tileSize = BoardLayout.Instance.GET_SIZE;
                int tileX = Mathf.FloorToInt(hit.point.x / tileSize);
                int tileZ = Mathf.FloorToInt(hit.point.z / tileSize);

                Debug.Log(tileX);
                Debug.Log(tileZ);

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
                    if (isRightClick)
                    {
                        UserGameController.Instance.DeselectUnit();
                    }
                    else
                    {
                        UserGameController.Instance.HandleMovementPhaseClick(tileX, tileZ);
                    }
                }
            }
        }
    }
}
