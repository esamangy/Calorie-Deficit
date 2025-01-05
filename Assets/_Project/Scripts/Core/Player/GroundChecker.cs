using UnityEngine;

public class GroundChecker : MonoBehaviour {
    [SerializeField] private float groundDistance = .02f;
    [SerializeField] private float heightOffset = .05f; 
    [SerializeField] private LayerMask groundLayers;
    
    public bool IsGrounded {get; private set;}

    void Update() {
        IsGrounded = Physics.SphereCast(transform.position + (Vector3.up * heightOffset), groundDistance, Vector3.down, out _, groundDistance + heightOffset, groundLayers);
    }
}
