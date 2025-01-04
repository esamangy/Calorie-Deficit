using UnityEngine;

public class Terrain : MonoBehaviour {
    [SerializeField] private TerrainInfoSO.TerrainType terrainType;

    public TerrainInfoSO.TerrainType TerrainType {
        get => terrainType;
    }
}
