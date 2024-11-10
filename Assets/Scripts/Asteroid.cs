using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private ParticleSystem destroyedParticles;
    public int size = 3;

    public GameManager gameManager;

    public void Start() {
        transform.localScale = 0.5f * size * Vector3.one;

        transform.rotation = Random.rotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        Vector2 direction = new Vector2(Random.value, Random.value).normalized;
        float spawnSpeed = Random.Range(4f - size, 5f - size);
        rb.AddForce(direction * spawnSpeed, ForceMode.Impulse);

        gameManager.asteroidCount++;
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.CompareTag("Bullet")) {
            gameManager.asteroidCount--;
            Destroy(collision.gameObject);

            if (size > 1) {
                for (int i = 0; i < 2; i++) {
                    Asteroid newAsteroid = Instantiate(this, transform.position, Quaternion.identity);
                    newAsteroid.size = size - 1;
                    newAsteroid.gameManager = gameManager;
                }
            }
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
