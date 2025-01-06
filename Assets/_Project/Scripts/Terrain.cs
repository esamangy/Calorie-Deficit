using UnityEngine;

public class Terrain : MonoBehaviour {
    private const int GROUND_LAYER = 6;
    [SerializeField] private TerrainInfoSO.TerrainType terrainType;
    private void Awake() {
        gameObject.layer = GROUND_LAYER;
    }
    public TerrainInfoSO.TerrainType TerrainType {
        get => terrainType;
    }
}
