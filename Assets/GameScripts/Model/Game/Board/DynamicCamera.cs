using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.GameScripts.Model.Game.Board
{
    public class DynamicCamera : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 100f;
        [SerializeField] private float zoomSpeed = 100f;
        [SerializeField] private float minHeight = 20f;
        [SerializeField] private float maxHeight = 400f;

        [Header("Rotation Settings")]
        [SerializeField] private float rotationSpeed = 50f;

        private Vector2 moveInput;
        private float zoomInput;

        void Update()
        {
            HandleMovement();
            HandleZoom();
            HandleRotation();
        }

        private void HandleMovement()
        {
            if (Keyboard.current != null)
            {
                float x = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);
                float z = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0);

                Vector3 move = new Vector3(x, 0, z) * moveSpeed * Time.deltaTime;
                transform.Translate(move, Space.World);
            }
        }

        private void HandleZoom()
        {
            if (Mouse.current != null)
            {
                float scroll = Mouse.current.scroll.ReadValue().y;

                if (scroll != 0)
                {
                    Vector3 zoomDir = transform.forward * (scroll * zoomSpeed * Time.deltaTime);
                    Vector3 newPos = transform.position + zoomDir;

                    newPos.y = Mathf.Clamp(newPos.y, minHeight, maxHeight);
                    transform.position = newPos;
                }
            }
        }

        private void HandleRotation()
        {
            if (Keyboard.current != null)
            {
                float rot = (Keyboard.current.eKey.isPressed ? 1 : 0) - (Keyboard.current.qKey.isPressed ? 1 : 0);
                transform.Rotate(Vector3.up, rot * rotationSpeed * Time.deltaTime, Space.World);
            }
        }
    }
}
