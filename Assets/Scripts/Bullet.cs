using UnityEngine;

public class Bullet : MonoBehaviour {
  [SerializeField] private float bulletLifetime = 1f;

  // When created, destroy after a set period of time.
  private void Awake() {
    Destroy(gameObject, bulletLifetime);
  }
}