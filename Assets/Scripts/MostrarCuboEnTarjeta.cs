using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
[RequireComponent(typeof(ARTrackedImageManager))]
public class MostrarCuboEnTarjeta : MonoBehaviour{

 [SerializeField] private GameObject cuboPrefab;
 [SerializeField] private float tamanoCubo = 0.15f;
 [SerializeField] private float alturaCubo = 0.08f;
 private ARTrackedImageManager manager;
 private readonly Dictionary<TrackableId, GameObject> cubos = new();
 private void Awake()
 {
 manager = GetComponent<ARTrackedImageManager>();
 }
 private void OnEnable()
 {
 manager.trackablesChanged.AddListener(AlCambiarImagenes);
 }
 private void OnDisable()
 {
 manager.trackablesChanged.RemoveListener(AlCambiarImagenes);
 }
 private void AlCambiarImagenes(
 ARTrackablesChangedEventArgs<ARTrackedImage> cambios)
 {
 foreach (var imagen in cambios.added) Actualizar(imagen);
 foreach (var imagen in cambios.updated) Actualizar(imagen);
 foreach (var item in cambios.removed)
 {
 if (cubos.TryGetValue(item.Key, out var objeto))
 {
 Destroy(objeto);
 cubos.Remove(item.Key);
 }
 }
 }
 private void Actualizar(ARTrackedImage imagen)
 {
 if (cuboPrefab == null) return;
 if (!cubos.TryGetValue(imagen.trackableId, out var cubo))
 {
 cubo = Instantiate(cuboPrefab, imagen.transform);
 cubo.transform.localPosition = new Vector3(0f, alturaCubo, 0f);
 cubo.transform.localRotation = Quaternion.identity;
 cubo.transform.localScale = Vector3.one * tamanoCubo;
 cubos.Add(imagen.trackableId, cubo);
 }
 cubo.SetActive(imagen.trackingState == TrackingState.Tracking);
 }
}