using UnityEngine;

// [RequireComponent(typeof(FirstPersonController))]
public class PlayerTerrainController : MonoBehaviour {
    public static PlayerTerrainController Instance {get; private set;}
    [SerializeField] private TerrainInfoSO terrainInfoSO;
    // private FirstPersonController playerController;
    private CharacterController characterController;
    private TerrainInfoSO.TerrainType lastType;
    private void Awake() {
        if(Instance != null) {
            Destroy(this);
            return;
        }
        Instance = this;
        // playerController = GetComponent<FirstPersonController>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start() {
        lastType = PlayerStandingIn();
        UpdatePlayerController(lastType);
    }

    private void Update() {
        TerrainInfoSO.TerrainType type = PlayerStandingIn();
        if(type != lastType) {
            UpdatePlayerController(type);
            lastType = type;
        }

        
    }

    private void UpdatePlayerController(TerrainInfoSO.TerrainType newType){
        TerrainInfoSO.TerrainInfo info = terrainInfoSO.GetTerrainInfo(newType);
        // playerController.MoveSpeed = info.moveSpeed;
        // playerController.SprintSpeed = info.sprintSpeed;
    }

    private TerrainInfoSO.TerrainType PlayerStandingIn() {
        int[] belowtypes = new int[TerrainInfoSO.GetNumTerrainTypes()];

        Vector3 playerPositionAdjusted = transform.position + (Vector3.up * .25f);

        RaycastHit[] hits = Physics.SphereCastAll(playerPositionAdjusted, characterController.radius, -Vector3.up, .3f);

        foreach (RaycastHit hit in hits) {
            if(hit.transform.TryGetComponent(out Terrain terrain)) {
                belowtypes[(int)terrain.TerrainType] ++;
            }
        }
        return GetMostCommon(belowtypes);
    }

    private TerrainInfoSO.TerrainType GetMostCommon(int[] array){
        if(array.Length == 0) return TerrainInfoSO.TerrainType.Normal;
        if(array.Length == 1) return 0;
        int index = 0;
        int most = array[0];

        for(int i = 1; i < array.Length; i ++){
            if(array[i] > most){
                index = i;
                most = array[i];
            }
        }

        return (TerrainInfoSO.TerrainType)index;
    }

    public float GetCurrentMoveSpeedCalorieMultiplier() {
        return terrainInfoSO.GetTerrainInfo(lastType).moveSpeedCalorieMultiplier;
    }

    public int GetCurrentSprintSpeedCalorieMultiplier() {
        return terrainInfoSO.GetTerrainInfo(lastType).sprintSpeedCalorieMultiplier;
    }

    public bool GetTypeSink() {
        return terrainInfoSO.GetTerrainInfo(lastType).sinks;
    }
}
