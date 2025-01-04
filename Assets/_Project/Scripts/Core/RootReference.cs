using UnityEngine;

public class RootReference : MonoBehaviour {
    [SerializeField] private Transform reference;

    public Transform GetReference(){
        return reference;
    }
}
