using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    public GameObject middleAsteroid;

    float speed = 50f;
    float angularSpeed = 10f;
    float angle = 30f; // This is an angle for spawning new medium asteroids.

    float scoreValue = 10f; // Score value for big asteroid

    Vector3 direction;
    Vector2 spriteSizes;

    EffectSpawner effectSpawner;
    LevelManager levelManager;

    bool isSpawned = false;

    Rigidbody2D rb;

	void Start ()
    {
        // References
        rb = GetComponent<Rigidbody2D>();
        spriteSizes = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
        levelManager = FindObjectOfType<LevelManager>();
        effectSpawner = FindObjectOfType<EffectSpawner>();

        RandomDirection();
        MoveAsteroid();

        InvokeRepeating("CheckMovement", 0.0f, 2.0f);
    }

    void Update()
    {
        ConstraintAsteroidMovement();
    }

    void MoveAsteroid()
    {
        // Add force to the rigidbody2d component of the asteroids
        rb.AddForce(direction * speed);

        // Add angular velocity to the object
        rb.angularVelocity = angularSpeed;

    }

    void ConstraintAsteroidMovement()
    {
        if (transform.position.x - spriteSizes.x / 2.0f > 10.6f)
        {
            transform.position = new Vector2(-(transform.position.x - spriteSizes.x / 2.0f), transform.position.y);
        }
        else if (transform.position.x + spriteSizes.x / 2.0f < -10.6f)
        {
            transform.position = new Vector2(-(transform.position.x + spriteSizes.x / 2.0f), transform.position.y);
        }
        else if (transform.position.y - spriteSizes.y / 2.0f > 6.0f)
        {
            transform.position = new Vector2(transform.position.x, -(transform.position.y - spriteSizes.y / 2.0f));
        }
        else if (transform.position.y + spriteSizes.y / 2.0f < -6.0f)
        {
            transform.position = new Vector2(transform.position.x, -(transform.position.y + spriteSizes.y / 2.0f));
        }
    }

    void RandomDirection()
    {
        int randomDirection = (int)Random.Range(1, 7f);

        switch (randomDirection)
        {
            case 1:
                direction = transform.up;
                break;
            case 2:
                direction = transform.up + new Vector3(-1f, 0f, 0f);
                break;
            case 3:
                direction = -transform.up + new Vector3(-1f, 0f, 0f);
                break;
            case 4:
                direction = -transform.up;
                break;
            case 5:
                direction = -transform.up + new Vector3(1f, 0f, 0f);
                break;
            case 6:
                direction = transform.up + new Vector3(1f, 0f, 0f);
                break;
            default:
                Debug.Log("Something is wrong with random directions");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            Debug.Log(collision.gameObject.transform.up);
            Destroy(collision.gameObject);

            if (!isSpawned)
                SpawnMediumAsteroidsPrime(collision.gameObject.transform.up); // SpawnMediumAsteroidsPrime SpawnMediumAsteroids

            Destroy(gameObject);
            effectSpawner.AsteroidsBlowingUp(transform.position, effectSpawner.asteroidExplosion, 0);
            levelManager.IncreaseScore(scoreValue);
        }

        // CHANGE THIS!
        if(collision.gameObject.tag == "Ship")
        {
            effectSpawner.AsteroidsBlowingUp(collision.gameObject.transform.position, effectSpawner.playerExplosion, 1);
            Destroy(collision.gameObject);
            FindObjectOfType<LevelManager>().OnPlayerDeath();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            // Get the point where hit has happened!
            Vector3 contact = collision.contacts[0].point;

            // Use event subscription her emaybe?
            effectSpawner.AsteroidCollision(contact);


            Debug.Log(contact);
            
        }
    }

    void SpawnMediumAsteroids(Vector3 directionForTrajectory)
    {
        angle = angle * Mathf.PI / 180f;

        Vector3 direction1 = new Vector3(Mathf.Cos(angle) * directionForTrajectory.x - Mathf.Sin(angle) * directionForTrajectory.y,
            Mathf.Sin(angle) * directionForTrajectory.x + Mathf.Cos(angle) * directionForTrajectory.y, directionForTrajectory.z);

        Vector3 direction2 = new Vector3(Mathf.Cos(-angle) * directionForTrajectory.x - Mathf.Sin(-angle) * directionForTrajectory.y,
            Mathf.Sin(-angle) * directionForTrajectory.x + Mathf.Cos(-angle) * directionForTrajectory.y, directionForTrajectory.z);

        string quadrant = "";
        #region SpawnLocations
        // Setup a proper spawning location based on the direction in which bullet was going before hitting the object
        Vector3 spawnLoc1 = new Vector3();
        Vector3 spawnLoc2 = new Vector3();

        if (directionForTrajectory.x > 0 && directionForTrajectory.y > 0)
        {
            // The first quadrant
            spawnLoc1 = transform.position + new Vector3(-0.5f, 0.5f, 0f);
            spawnLoc2 = transform.position + new Vector3(0.5f, -0.5f, 0f);

            quadrant = "first";
        }
        else if (directionForTrajectory.x < 0 && directionForTrajectory.y > 0)
        {
            // The second quadrant
            spawnLoc1 = transform.position + new Vector3(0.5f, 0.5f, 0f);
            spawnLoc2 = transform.position + new Vector3(-0.5f, -0.5f, 0f);

            quadrant = "second";
        }
        else if (directionForTrajectory.x < 0 && directionForTrajectory.y < 0)
        {
            // The third quadrant
            spawnLoc1 = transform.position + new Vector3(-0.5f, 0.5f, 0f);
            spawnLoc2 = transform.position + new Vector3(0.5f, -0.5f, 0f);

            quadrant = "third";
        }
        else if (directionForTrajectory.x > 0 && directionForTrajectory.y < 0)
        {
            // The fourth quadrant
            spawnLoc1 = transform.position + new Vector3(0.5f, -0.5f, 0f);
            spawnLoc2 = transform.position + new Vector3(0.5f, 0.5f, 0f);

            quadrant = "fourth";
        }
        else if (directionForTrajectory.x > 0 && directionForTrajectory.y == 0f)
        {
            // The positive x axis
            spawnLoc1 = transform.position + new Vector3(0f, 0.5f, 0f);
            spawnLoc2 = transform.position + new Vector3(0f, -0.5f, 0f);

            quadrant = "x";
        }
        else if (directionForTrajectory.x == 0f && directionForTrajectory.y > 0)
        {
            // The positive y axis
            spawnLoc1 = transform.position + new Vector3(-0.5f, 0f, 0f);
            spawnLoc2 = transform.position + new Vector3(0.5f, 0f, 0f);

            quadrant = "y";
        }
        else if (directionForTrajectory.x < 0 && directionForTrajectory.y == 0f)
        {
            // The negative x axis
            spawnLoc1 = transform.position + new Vector3(0f, 0.5f, 0f);
            spawnLoc2 = transform.position + new Vector3(0f, -0.5f, 0f);

            quadrant = "-x";
        }
        else if (directionForTrajectory.x == 0.0f && directionForTrajectory.y < 0)
        {
            // The negative y axis
            spawnLoc1 = transform.position + new Vector3(-0.5f, 0f, 0f);
            spawnLoc2 = transform.position + new Vector3(0.5f, 0f, 0f);

            quadrant = "-y";
        }
        #endregion
        
        // Very very inelegant, and in fact; pretty fucking lame solution
        if(quadrant == "first" || quadrant == "x" || quadrant == "y" || quadrant == "-x" || quadrant == "-y")
        {
            // Flip the directions based on spawn locations; quadrant-wise
            Vector3 tempVec = direction1;
            direction1 = direction2;
            direction2 = tempVec;
        }

        //Debug.Log(directionForTrajectory);
        //Debug.Log("Direction1: " + direction1 + ", Direction2: " + direction2);

        if (middleAsteroid != null)
        {
            // Instantiate a new asteroid
            GameObject midAsteroid1 = Instantiate(middleAsteroid, spawnLoc1, Quaternion.identity);

            // Add the necessary component
            midAsteroid1.AddComponent<MiddleAsteroid>();

            // Set up its trajectory
            midAsteroid1.GetComponent<MiddleAsteroid>().direction = direction2;

            // Spawn the second asteroid
            GameObject midAsteroid2 = Instantiate(middleAsteroid, spawnLoc2, Quaternion.identity);
            midAsteroid2.AddComponent<MiddleAsteroid>();
            midAsteroid2.GetComponent<MiddleAsteroid>().direction = direction1;

            isSpawned = true;
        }
    }

    void SpawnMediumAsteroidsPrime(Vector3 directionForTrajectory)
    {
        float angle = 60.0f * Mathf.PI / 180.0f;
        float offset = 1.0f;

        Vector3 direction1 = new Vector3(Mathf.Cos(angle) * directionForTrajectory.x - Mathf.Sin(angle) * directionForTrajectory.y,
            Mathf.Sin(angle) * directionForTrajectory.x + Mathf.Cos(angle) * directionForTrajectory.y, directionForTrajectory.z);

        Vector3 direction2 = new Vector3(Mathf.Cos(-angle) * directionForTrajectory.x - Mathf.Sin(-angle) * directionForTrajectory.y,
            Mathf.Sin(-angle) * directionForTrajectory.x + Mathf.Cos(-angle) * directionForTrajectory.y, directionForTrajectory.z);

        Vector3 spawnLocation1 = transform.position + direction1 * offset;
        Vector3 spawnLocation2 = transform.position + direction2 * offset;

        if(middleAsteroid != null)
        {
            // Instantiate a new asteroid
            GameObject midAsteroid1 = Instantiate(middleAsteroid, spawnLocation1, Quaternion.identity);

            // Add the necessary component
            midAsteroid1.AddComponent<MiddleAsteroid>();

            // Set up its trajectory
            midAsteroid1.GetComponent<MiddleAsteroid>().direction = direction1;

            // Spawn the second asteroid
            GameObject midAsteroid2 = Instantiate(middleAsteroid, spawnLocation2, Quaternion.identity);
            midAsteroid2.AddComponent<MiddleAsteroid>();
            midAsteroid2.GetComponent<MiddleAsteroid>().direction = direction2;

            isSpawned = true;
        }
    }

    void CheckMovement()
    {
        Vector2 velocity = rb.velocity;
        float diversionForce = 50.0f;

        // If the asteroid goes out of screen, then check if it is stuck in an axis
        if(transform.position.x < -9.0f || transform.position.x > 9.0f)
        {
            if (velocity.x == 0f && (velocity.y == 1f || velocity.y == -1f))
            {
                rb.AddForce(new Vector2(1, 0) * diversionForce);
                Debug.Log("Object is stuck in y axis!");
            }
        }

        if(transform.position.y < 5.0f || transform.position.y > 5.0f)
        {
            if (velocity.y == 0f && (velocity.x == 1f || velocity.x == -1))
            {
                rb.AddForce(new Vector2(0, 1) * diversionForce);
                Debug.Log("Object is stuck in x axis!");
            }
        }
    }
}
