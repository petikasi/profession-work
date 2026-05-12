using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.GameScripts.ViewModel.CameraAndGrapichs.CameraHandling
{
    public class DynamicCamera : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 100f;//A pályán történő mozgatás sebessége
        [SerializeField] private float zoomSpeed = 200f;//A pályára történő közelítés sebessége

        [Header("Borders")]
        [SerializeField] private float minHeight = 10f;// A kamera minimum magassága
        [SerializeField] private float maxHeight = 200f;// A kamera maximum magassága
        [SerializeField] private float groundY = 0f; // A kamera magassága

        [Header("Tilt angel (C and V)")]
        [SerializeField] private float pitchSpeed = 50f;//forgatás gyorsasága
        [SerializeField] private float minPitch = 20f; // Alacsony nézet
        [SerializeField] private float maxPitch = 85f; // Majdnem teljesen felülről

        private Vector3 targetPosition; // Ez a pont, amit a kamera "néz" a földön

        void Start()
        {
            // Kezdéskor a kamera elé állítjuk a fókuszpontot a földön
            Ray ray = new(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPosition = hit.point;
            }
            else
            {
                targetPosition = new Vector3(transform.position.x, groundY, transform.position.z + 50f);
            }
        }

        void Update()
        {
            HandleMovement();
            HandleZoom();
            HandleRotation();
        }

        private void HandleMovement()
        {
            float x = (Keyboard.current.dKey.isPressed ? 2 : 0) - (Keyboard.current.aKey.isPressed ? 2 : 0);
            float z = (Keyboard.current.wKey.isPressed ? 2 : 0) - (Keyboard.current.sKey.isPressed ? 2 : 0);

            if (x != 0 || z != 0)
            {
                // A kamera irányához képest mozgatunk (hogy a 'W' előre menjen, amerre nézünk)
                Vector3 forward = transform.forward;
                forward.y = 0; // Ne menjünk bele a földbe mozgáskor
                Vector3 right = transform.right;

                Vector3 moveDir = (forward.normalized * z + right.normalized * x).normalized;

                // Mozgatjuk a kamerát ÉS a célpontot is
                transform.position += moveDir * moveSpeed * Time.deltaTime;
            }
        }

        private void HandleZoom()
        {
            float scroll = Mouse.current.scroll.ReadValue().y;

            if (scroll != 0)
            {
                // Raycast-ot indítunk az egér pozíciójából, hogy megtudjuk, MIRE akarunk ránagyítani
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // Ez az a pont a földön, ami felé közelíteni fogunk
                    Vector3 zoomTarget = hit.point;

                    // Kiszámoljuk az irányt a kamera és a pont között
                    Vector3 direction = (zoomTarget - transform.position).normalized;

                    // Közelítés mértéke
                    float zoomAmount = scroll * zoomSpeed * Time.deltaTime;
                    Vector3 newPos = transform.position + direction * zoomAmount;

                    // Magasság korlátozása (ne menjünk a föld alá)
                    if (newPos.y > minHeight && newPos.y < maxHeight)
                    {
                        transform.position = newPos;
                    }
                }
            }
        }
        private void HandleRotation()
        {
            if (Keyboard.current == null) return;

            // 1. Oldalirányú forgatás (Q és E) - Opcionális, de hasznos
            float yRot = (Keyboard.current.eKey.isPressed ? 2 : 0) - (Keyboard.current.qKey.isPressed ? 2 : 0);
            if (yRot != 0)
            {
                transform.Rotate(Vector3.up, yRot * pitchSpeed * Time.deltaTime, Space.World);
            }

            // 2. Függőleges dőlésszög (C és V)
            // C = Lefelé néz (nagyobb szög), V = Felfelé néz (kisebb szög)
            float pInput = (Keyboard.current.cKey.isPressed ? 2 : 0) - (Keyboard.current.vKey.isPressed ? 2 : 0);

            if (pInput != 0)
            {
                // Lekérjük a jelenlegi szöget
                Vector3 currentRotation = transform.eulerAngles;
                float newPitch = currentRotation.x + (pInput * pitchSpeed * Time.deltaTime);

                // Korlátozzuk (Clamp), hogy ne forduljon át a kamera
                newPitch = Mathf.Clamp(newPitch, minPitch, maxPitch);

                // Alkalmazzuk az új rotációt
                transform.rotation = Quaternion.Euler(newPitch, currentRotation.y, 0);
            }
        }
    }
}
