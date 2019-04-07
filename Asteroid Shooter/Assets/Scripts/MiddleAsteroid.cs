using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleAsteroid : MonoBehaviour {

    float speed = 50f;
    float angularSpeed = 10f;

    float scoreValue = 5f; // Score value for medium asteroids

    public Vector3 direction;

    Vector2 spriteSizes;
    EffectSpawner effectSpawner;
    LevelManager levelManager;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteSizes = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
        levelManager = FindObjectOfType<LevelManager>();
        effectSpawner = FindObjectOfType<EffectSpawner>();

        //RandomDirection();
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
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);

            Destroy(gameObject);

            levelManager.IncreaseScore(scoreValue);

            effectSpawner.AsteroidsBlowingUp(transform.position, effectSpawner.mediumAsteroidExplosion, 0);

        }

        // CHANGE THIS!
        if (collision.gameObject.tag == "Ship")
        {
            effectSpawner.AsteroidsBlowingUp(collision.gameObject.transform.position, effectSpawner.playerExplosion, 1);
            Destroy(collision.gameObject);
            FindObjectOfType<LevelManager>().OnPlayerDeath();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            // Get the point where hit has happened!
            Vector3 contact = collision.contacts[0].point;

            // Instantiate the collision effect
            FindObjectOfType<EffectSpawner>().AsteroidCollision(contact);
        }
    }

    void CheckMovement()
    {
        Vector2 velocity = rb.velocity;
        float diversionForce = 50.0f;

        // If the asteroid goes out of screen, then check if it is stuck in an axis
        if (transform.position.x < -9.0f || transform.position.x > 9.0f)
        {
            if (velocity.x == 0f && (velocity.y == 1f || velocity.y == -1f))
            {
                rb.AddForce(new Vector2(1, 0) * diversionForce);
                Debug.Log("Object is stuck in y axis!");
            }
        }

        if (transform.position.y < 5.0f || transform.position.y > 5.0f)
        {
            if (velocity.y == 0f && (velocity.x == 1f || velocity.x == -1))
            {
                rb.AddForce(new Vector2(0, 1) * diversionForce);
                Debug.Log("Object is stuck in x axis!");
            }
        }
    }
}
