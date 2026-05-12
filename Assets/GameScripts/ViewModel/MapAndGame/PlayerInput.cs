using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.GameScripts.ViewModel.MapAndGame
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private float sizeOfTile = 10f;

        void Update()
        {

            if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
            {
                HandleSelection();
            }
        }

        private void HandleSelection()
        {
            Vector2 mousePos = Pointer.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.name == "GrandFloor")
                {
                    int tileX = Mathf.FloorToInt(hit.point.x / sizeOfTile);
                    int tileZ = Mathf.FloorToInt(hit.point.z / sizeOfTile);

                    Debug.Log($"Clicked Tile: {tileX}, {tileZ}");
                }
            }
        }
    }
}
