using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class ColocarCuboEnMesa : MonoBehaviour
{
 [Header("Referencias AR")]
 [SerializeField] private ARRaycastManager raycastManager;
 [SerializeField] private GameObject cuboPrefab;
 [SerializeField] private float tamanoCubo = 0.15f;
 private GameObject cuboActual;
 private static readonly List<ARRaycastHit> impactos =
 new List<ARRaycastHit>();
 private void OnEnable()
 {
 EnhancedTouchSupport.Enable();
 }
 private void OnDisable()
 {
 EnhancedTouchSupport.Disable();
 }
 private void Update()
 {
 if (Touch.activeTouches.Count == 0) return;
 var toque = Touch.activeTouches[0];
 if (toque.phase != UnityEngine.InputSystem.TouchPhase.Began)
 return;
 if (raycastManager == null || cuboPrefab == null)
 {
 Debug.LogError("RA: Revisa las referencias en ControlMesa");
 return;
 }
 bool encontroMesa = raycastManager.Raycast(
 toque.screenPosition,
 impactos,
 TrackableType.PlaneWithinPolygon);
 if (!encontroMesa || impactos.Count == 0)
 {
 Debug.Log("RA: El toque no coincide con un plano");
 return;
 }
 Pose pose = impactos[0].pose;
 Vector3 centro = pose.position +
 Vector3.up * (tamanoCubo / 2f);
 if (cuboActual == null)
 {
 cuboActual = Instantiate(
 cuboPrefab, centro, Quaternion.identity);
 cuboActual.transform.localScale =
 Vector3.one * tamanoCubo;
 Debug.Log("RA: Cubo creado");
 }
 else
 {
 cuboActual.transform.position = centro;
 Debug.Log("RA: Cubo reposicionado");
 }
 }
}