using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class ColocarTableroEnMesa : MonoBehaviour
{
 [SerializeField] private ARRaycastManager raycastManager;
 [SerializeField] private GameObject tableroPrefab;
 private GameObject tableroActual;
 private static readonly List<ARRaycastHit> impactos = new();
 private void OnEnable() => EnhancedTouchSupport.Enable();
 private void OnDisable() => EnhancedTouchSupport.Disable();
 private void Update()
 {
 if (Touch.activeTouches.Count == 0) return;
 var toque = Touch.activeTouches[0];
 if (toque.phase != UnityEngine.InputSystem.TouchPhase.Began) return;
 if (raycastManager == null || tableroPrefab == null)
 {
 Debug.LogError("AR: Asigna Raycast Manager y Tablero Prefab");
 return;
 }
 if (!raycastManager.Raycast(toque.screenPosition,
 impactos, TrackableType.PlaneWithinPolygon)) return;
 // La base del tablero tiene su cara inferior en y local = 0.
 Vector3 puntoMesa = impactos[0].pose.position;
 if (tableroActual == null)
 {
 tableroActual = Instantiate(tableroPrefab,
 puntoMesa, Quaternion.identity);
 Debug.Log("AR: Tablero creado");
 }
 else
 {
 tableroActual.transform.position = puntoMesa;
 Debug.Log("AR: Tablero reposicionado");
 }
 }
}
