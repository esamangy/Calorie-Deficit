using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerTerrainController))]
// [RequireComponent(typeof(StarterAssetsInputs))]
public class PlayerSinking : MonoBehaviour {
    [SerializeField] private float sinkTime = 2f;
    [SerializeField] private float escapeTime = 1f;
    [SerializeField] private Transform playerCameraRoot;
    [SerializeField] private Vector3 sunkOffset = new Vector3(0, -.5f, 0);
    private Vector3 initialCameraPosition;
    private PlayerTerrainController terrainController;
    // private StarterAssetsInputs input;
    private bool wasSinkingLastFrame;
    private Coroutine sinkCoroutine;
    private float sinkProgress = 0;

    public event EventHandler OnPlayerSunk;
    private void Awake() {
        terrainController = GetComponent<PlayerTerrainController>();
        // input = GetComponent<StarterAssetsInputs>();
    }

    private void Start() {
        initialCameraPosition = playerCameraRoot.localPosition;
        wasSinkingLastFrame = terrainController.GetTypeSink();
        if(wasSinkingLastFrame){
            sinkCoroutine = StartCoroutine(Sink());
        }
    }

    private void Update() {
        // bool shouldSinkThisFrame = terrainController.GetTypeSink() && input.move == Vector2.zero;
        // if(shouldSinkThisFrame && !wasSinkingLastFrame) {
        //     if(sinkCoroutine != null){
        //         StopCoroutine(sinkCoroutine);
        //     }
        //     sinkCoroutine = StartCoroutine(Sink());
        // } else if(wasSinkingLastFrame && !shouldSinkThisFrame){
        //     if(sinkCoroutine != null){
        //         StopCoroutine(sinkCoroutine);
        //     }
        //     sinkCoroutine = StartCoroutine(Escape());
        // }
        // wasSinkingLastFrame = shouldSinkThisFrame;
    }

    private IEnumerator Sink() {
        float timeSinking = Mathf.Lerp(0, sinkTime, sinkProgress);
        while(sinkProgress < 1){
            playerCameraRoot.localPosition = Vector3.Lerp(initialCameraPosition, initialCameraPosition + sunkOffset, sinkProgress);
            
            timeSinking += Time.deltaTime;
            sinkProgress = Mathf.InverseLerp(0, 1, timeSinking / sinkTime);
            yield return null;
        }
        OnPlayerSunk?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator Escape() {
        float timeEscaping = Mathf.Lerp(escapeTime, 0, sinkProgress);
        while(sinkProgress > 0){
            playerCameraRoot.localPosition = Vector3.Lerp(initialCameraPosition, initialCameraPosition + sunkOffset, sinkProgress);
            
            timeEscaping += Time.deltaTime;
            sinkProgress = Mathf.InverseLerp(0, 1, 1 - (timeEscaping / escapeTime));
            yield return null;
        }
    }
}
