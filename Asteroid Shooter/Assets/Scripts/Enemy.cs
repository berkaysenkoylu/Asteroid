using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    
    public float speed = 5.0f;
    public Transform[] destinations;
    public GameObject enemyBullet;

    AsteroidSpawner Aspawner;
    EffectSpawner effectSpawner;
    float scoreValue = 25.0f;
    Vector3 initialPosition;

    Transform target;
    Vector3 whereToShoot;

	void Start ()
    {
        Aspawner = FindObjectOfType<AsteroidSpawner>();
        effectSpawner = FindObjectOfType<EffectSpawner>();
        target = GameObject.FindGameObjectWithTag("Ship").transform;

        initialPosition = transform.position;

        StartCoroutine(MoveBetweenTwoLocations(destinations));

        InvokeRepeating("ShootPlayerShip", 1.5f, 2.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            // Call the necessary function
            Aspawner.OnEnemyDeath(scoreValue);

            // Destroy the game object
            Destroy(gameObject);

            // Explosion
            effectSpawner.EnemyBlowingUp(transform.position, effectSpawner.enemyExplosion, 1);
        }
    }

    IEnumerator MoveBetweenTwoLocations(Transform[] locations)
    {
        int targetDestinationIndex = 0;

        Vector3 targetPoint = locations[targetDestinationIndex].position;

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

            if (transform.position == targetPoint)
            {
                targetDestinationIndex = (targetDestinationIndex + 1) % locations.Length;
                targetPoint = locations[targetDestinationIndex].position;

                yield return new WaitForSeconds(1.0f);
            }

            yield return null;
        }
    }

    void ShootPlayerShip()
    {
        if (target == null)
            return;

        // Create a new bullet
        GameObject newEnemyBullet = Instantiate(enemyBullet, transform.position, Quaternion.identity);
    }
}