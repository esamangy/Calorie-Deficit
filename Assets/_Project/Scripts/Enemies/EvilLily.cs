using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EvilLily : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private float warningRange = 2f;
    private Transform player;
    private bool isAnimating = false;
    private void Awake() {
        GetComponent<Collider>().isTrigger = true;
    }
    private void Start() {
        player = PlayerController.Instance.transform;
    }
    private void Update() {
        if(Vector3.Distance(player.position, transform.position) < warningRange){
            if(!isAnimating){
                //start animating
                animator.SetBool("Warning", true);
                isAnimating = true;
            }
        } else {
            if(isAnimating){
                //stop animating
                animator.SetBool("Warning", false);
                isAnimating = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.transform.CompareTag("Player")){
            PlayerHUD.Instance.KillPlayer("You got snatched by an evil Lily");
        }
    }
}
