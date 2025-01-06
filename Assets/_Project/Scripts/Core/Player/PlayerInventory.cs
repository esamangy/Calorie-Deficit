using System.Collections;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    private const float DIST_FROM_CAMERA = .4f;
    public static PlayerInventory Instance { get; private set;}
    [SerializeField] private float smoothingMultiplier = 3f;
    [SerializeField] private Transform leftHandPosition;
    [SerializeField] private Transform rightHandPosition;
    [SerializeField] private Transform cameraRoot;
    private Selectable leftHandSelecting;
    private Selectable rightHandSelecting;
    private Vector3 leftSmoothedPosiiton;
    private Vector3 rightSmoothedPosiiton;
    private Coroutine moveCoroutineOther;
    private Coroutine moveCoroutine;
    private void Awake() {
        if(Instance != null) {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start() {
        leftSmoothedPosiiton = leftHandPosition.position;
        rightSmoothedPosiiton = rightHandPosition.position;
    }

    private void Update() {
        if(!PlayerInteraction.Instance.isUsing){
            leftSmoothedPosiiton = Vector3.Lerp(leftSmoothedPosiiton, leftHandPosition.position, Time.deltaTime * smoothingMultiplier);
            rightSmoothedPosiiton = Vector3.Lerp(rightSmoothedPosiiton, rightHandPosition.position, Time.deltaTime * smoothingMultiplier);
        }
        if(leftHandSelecting != null){
            leftHandSelecting.transform.position = leftSmoothedPosiiton;
            leftHandSelecting.transform.rotation = cameraRoot.transform.rotation;
        }
        if(rightHandSelecting != null){
            rightHandSelecting.transform.position = rightSmoothedPosiiton;
            rightHandSelecting.transform.rotation = cameraRoot.transform.rotation;
        }
    }

    public void UpdateRightUntilStopped(bool onOff){
        if(onOff){
            moveCoroutineOther = StartCoroutine(UpdateRight());
        } else {
            StopCoroutine(moveCoroutineOther);
        }
    }

    private IEnumerator UpdateRight() {
        while(true){
            rightSmoothedPosiiton = Vector3.Lerp(rightSmoothedPosiiton, rightHandPosition.position, Time.deltaTime * smoothingMultiplier);
            yield return null;
        }
    }

    public void UpdateLeftUntilStopped(bool onOff){
        if(onOff){
            moveCoroutineOther = StartCoroutine(UpdateLeft());
        } else {
            StopCoroutine(moveCoroutineOther);
        }
    }

    private IEnumerator UpdateLeft() {
        while(true){
            leftSmoothedPosiiton = Vector3.Lerp(leftSmoothedPosiiton, leftHandPosition.position, Time.deltaTime * smoothingMultiplier);
            yield return null;
        }
    }

    public Selectable GetSelecteable(PlayerInteraction.Handedness handedness){
        if(handedness == PlayerInteraction.Handedness.Right) {
            return GetRightHandSelecting();
        } else {
            return GetLeftHandSelecting();
        }
    }

    public Selectable GetLeftHandSelecting() {
        return leftHandSelecting;
    }

    public Selectable GetRightHandSelecting() {
        return rightHandSelecting;
    }

    public void SetSelecting(Selectable item, PlayerInteraction.Handedness handedness) {
        if(handedness == PlayerInteraction.Handedness.Right) {
            SetRightHandSelecting(item);
        } else {
            SetLeftHandSelecting(item);
        }
    }
    public void SetLeftHandSelecting(Selectable item) {
        leftHandSelecting = item;
        if(leftHandSelecting != null){
            leftSmoothedPosiiton = item.transform.position;
        }
    }

    public void SetRightHandSelecting(Selectable item) {
        rightHandSelecting = item;
        if(rightHandSelecting != null){
            rightSmoothedPosiiton = item.transform.position;
        }
    }

    public bool HasLeftSelecting() {
        return leftHandSelecting != null;
    }

    public bool HasRightSelecting() {
        return rightHandSelecting != null;
    }

    public Vector3 GetHandLocation(PlayerInteraction.Handedness handedness){
        return handedness == PlayerInteraction.Handedness.Left ? leftSmoothedPosiiton : rightSmoothedPosiiton;
    }

    public void MoveLeftToAndBack(){
        StartCoroutine(MoveLeftToFaceAndBack(.1f, .9f, 1f));
    }

    public void MoveRightToAndBack() {
        StartCoroutine(MoveRightToFaceAndBack(.1f, .9f, 1f));
    }

    public void MoveLeftTo(float value) {
        UpdateRightUntilStopped(true);
        StartCoroutine(MoveLeftToFace(value));
    }

    public void MoveLeftBack() {
        StopCoroutine(moveCoroutine);
        UpdateRightUntilStopped(false);
        StartCoroutine(MoveLeftBack(.1f));
    }

    public void MoveRightTo(float value) {
        UpdateLeftUntilStopped(true);
        StartCoroutine(MoveRightToFace(value));
    }

    public void MoveRightBack() {
        StopCoroutine(moveCoroutine);
        UpdateLeftUntilStopped(false);
        StartCoroutine(MoveRightBack(.1f));
    }

    private IEnumerator MoveLeftToFaceAndBack(float moveToTime, float stayTime, float moveBackTime){
        yield return StartCoroutine(MoveLeftToFace(moveToTime));
        float timer = 0;
        while(timer < stayTime - moveToTime){
            leftSmoothedPosiiton = cameraRoot.position + cameraRoot.forward * DIST_FROM_CAMERA;
            rightSmoothedPosiiton = Vector3.Lerp(rightSmoothedPosiiton, rightHandPosition.position, Time.deltaTime * smoothingMultiplier);
            timer += Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(MoveLeftBack(moveBackTime - stayTime));
        moveCoroutineOther = null;
        if(moveCoroutine != null){
            StopCoroutine(moveCoroutine);
        }
    }
    private IEnumerator MoveLeftToFace(float moveTime) {
        float timer = 0;
        Vector3 startPos = leftSmoothedPosiiton;
        while(timer < moveTime) {
            leftSmoothedPosiiton = Vector3.Lerp(startPos, cameraRoot.position + cameraRoot.forward * DIST_FROM_CAMERA, timer / moveTime);
            timer += Time.deltaTime;
            yield return null;
        }
        moveCoroutine = StartCoroutine(UpdateLeftMain());
    }

    private IEnumerator MoveLeftBack(float moveTime) {
        float timer = 0;
        while(timer < moveTime){
            leftSmoothedPosiiton = Vector3.Lerp(cameraRoot.position + cameraRoot.forward * DIST_FROM_CAMERA, leftHandPosition.position, timer / moveTime);
            timer += Time.deltaTime;
            yield return null;
        }
        leftSmoothedPosiiton = leftHandPosition.position;
        if(moveCoroutine != null){
            StopCoroutine(moveCoroutine);
        }
    }

    private IEnumerator MoveRightToFaceAndBack(float moveToTime, float stayTime, float moveBackTime){
        yield return StartCoroutine(MoveRightToFace(moveToTime));
        float timer = 0;
        while(timer < stayTime - moveToTime){
            rightSmoothedPosiiton = cameraRoot.position + cameraRoot.forward * DIST_FROM_CAMERA;
            leftSmoothedPosiiton = Vector3.Lerp(leftSmoothedPosiiton, leftHandPosition.position, Time.deltaTime * smoothingMultiplier);
            timer += Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(MoveRightBack(moveBackTime - stayTime));
        moveCoroutineOther = null;
    }
    private IEnumerator MoveRightToFace(float moveTime) {
        float timer = 0;
        Vector3 startPos = rightSmoothedPosiiton;
        while(timer < moveTime) {
            rightSmoothedPosiiton = Vector3.Lerp(startPos, cameraRoot.position + cameraRoot.forward * DIST_FROM_CAMERA, timer / moveTime);
            timer += Time.deltaTime;
            yield return null;
        }
        moveCoroutine = StartCoroutine(UpdateRightMain());
    }

    private IEnumerator MoveRightBack(float moveTime) {
        float timer = 0;
        while(timer < moveTime){
            rightSmoothedPosiiton = Vector3.Lerp(cameraRoot.position + cameraRoot.forward * DIST_FROM_CAMERA, rightHandPosition.position, timer / moveTime);
            timer += Time.deltaTime;
            yield return null;
        }
        rightSmoothedPosiiton = rightHandPosition.position;
        if(moveCoroutine != null){
            StopCoroutine(moveCoroutine);
        }
    }

    private IEnumerator UpdateLeftMain() {
        while(true) {
            leftSmoothedPosiiton = cameraRoot.position + cameraRoot.forward * DIST_FROM_CAMERA;
            yield return null;
        }
    }

    private IEnumerator UpdateRightMain() {
        while(true) {
            rightSmoothedPosiiton = cameraRoot.position + cameraRoot.forward * DIST_FROM_CAMERA;
            yield return null;
        }
    }
}
