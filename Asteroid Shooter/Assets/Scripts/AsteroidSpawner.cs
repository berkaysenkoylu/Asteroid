using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {

    public Transform[] spawnLocations;
    public Transform enemySpawnPoint;
    public GameObject bigAsteroid;
    public GameObject mediumAsteroid;
    public GameObject enemy;
    public LevelManager levelManager;

    int maxAsteroidCount = 6;
    int currentAsteroidCount;
    int asteroidsToSpawn;
    int maxEnemyNumber = 1;
    int currentEnemyNumber = 0;
    int waveNumber = 0;

    private void Start()
    {
        waveNumber = 1;
        levelManager.UpdateWaveNumber(waveNumber);
        StartCoroutine(SpawnAsteroids());
    }

    private void Update()
    {
        CountCurrentAsteroids();
        if (currentAsteroidCount == 0)
        {
            OnAllAsteroidsDestroyed();
            SpawnWaves();
        }
        
        if(waveNumber % 2 == 0 && currentEnemyNumber < maxEnemyNumber) 
        {
            SpawnEnemyShip();
        }
    }

    void CountCurrentAsteroids()
    {
        currentAsteroidCount = GameObject.FindGameObjectsWithTag("Asteroid").Length;
    }

    IEnumerator SpawnAsteroids()
    {
        // Generate a random integer
        int randomInteger = (int)Random.Range(0f, 10f);

        // Choose a random spawn point
        Transform randomSpawn = spawnLocations[randomInteger];

        // Spawn the asteroid
        GameObject newAsteroid = Instantiate(bigAsteroid, randomSpawn.position, Quaternion.identity);

        yield return new WaitForSeconds(1.5f);
    }

    int GetAsteroidNumberForWave(int waveNumber)
    {
        int asteroidNumber = waveNumber + 1;

        return asteroidNumber;
    }

    void OnAllAsteroidsDestroyed()
    {
        // Increase wave number
        waveNumber += 1;

        levelManager.UpdateWaveNumber(waveNumber);

        // Get the necessary number of asteroids to spawn (If wave number exceeds some number, flat it)
        if (waveNumber < 5)
            asteroidsToSpawn = GetAsteroidNumberForWave(waveNumber);
        else
            asteroidsToSpawn = maxAsteroidCount;
    }

    void SpawnWaves()
    {
        for(int i = 0; i < asteroidsToSpawn; i++)
            StartCoroutine(SpawnAsteroids());
    }

    void SpawnEnemyShip()
    {
        // Instantiate a new enemy
        GameObject newEnemy = Instantiate(enemy, enemySpawnPoint.position, Quaternion.identity);

        // Set a new parent
        newEnemy.transform.SetParent(enemySpawnPoint);

        currentEnemyNumber += 1;
    }

    public void OnEnemyDeath(float scoreValue)
    {
        // Decrement the number of enemy active
        currentEnemyNumber -= 1;

        // Increment the score
        levelManager.IncreaseScore(scoreValue);
    }
}
