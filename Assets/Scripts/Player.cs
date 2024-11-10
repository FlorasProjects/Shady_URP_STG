using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
  [Header("Ship parameters")]
  [SerializeField] private float shipAcceleration = 10f;
  [SerializeField] private float shipMaxVelocity = 10f;
  [SerializeField] private float shipRotationSpeed = 180f;
  [SerializeField] private float bulletSpeed = 8f;

  [Header("Object references")]
  [SerializeField] private Transform bulletSpawn;
  [SerializeField] private Rigidbody bulletPrefab;
  [SerializeField] private ParticleSystem destroyedParticles;

  private Rigidbody shipRigidbody;
  private bool isAlive = true;
  private bool isAccelerating = false;

  private void Start() {
    shipRigidbody = GetComponent<Rigidbody>();
  }

  private void Update() {
    if (isAlive) {
      HandleShipAcceleration();
      HandleShipRotation();
      HandleShooting();
    }
  }

  private void FixedUpdate() {
    if (isAlive && isAccelerating) {
      // Increase velocity upto a maximum.
      shipRigidbody.AddForce(shipAcceleration * transform.up);
      shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
    }
  }

  private void HandleShipAcceleration() {
    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
      isAccelerating = true;
    } else {
      isAccelerating = false;
    }
  }

  private void HandleShipRotation() {
    if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
      transform.Rotate(shipRotationSpeed * Time.deltaTime * transform.forward);
    } else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
      transform.Rotate(-shipRotationSpeed * Time.deltaTime * transform.forward);
    }
  }

  private void HandleShooting() {
    // Shooting.
    if (Input.GetKeyDown(KeyCode.Space)) {

      Rigidbody bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

      Vector2 shipVelocity = shipRigidbody.velocity;
      Vector2 shipDirection = transform.up;
      float shipForwardSpeed = Vector2.Dot(shipVelocity, shipDirection);

      if (shipForwardSpeed < 0) { 
        shipForwardSpeed = 0; 
      }

      bullet.velocity = shipDirection * shipForwardSpeed;

      bullet.AddForce(bulletSpeed * transform.up, ForceMode.Impulse);
    }
  }

  private void OnTriggerEnter(Collider collision) {
    if (collision.CompareTag("Asteroid")) {

      isAlive = false;

      GameManager gameManager = FindAnyObjectByType<GameManager>();
      gameManager.GameOver();
      Instantiate(destroyedParticles, transform.position, Quaternion.identity);
      Destroy(gameObject);
    }
  }
}