using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float _upperBound = 8f;
    private float _leftBound = -10f;
    private float _rightBound = 10f;

    [SerializeField]
    private float _spawnDelayStart = 5.0f;
    private float _spawnDelay = 0.0f;

    [SerializeField]
    private GameObject _enemyPrefab = null;

    [SerializeField]
    private GameObject[] _powerups = null;

    [SerializeField]
    private GameObject _enemyContainer = null;

    [SerializeField]
    private float _powerupMaxWait = 20.0f;
    [SerializeField]
    private float _powerupMinWait = 10.0f;

    private bool _shouldSpawn = true;
    void Start()
    {
        _spawnDelay = _spawnDelayStart;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator EnemySpawnRoutine()
    {
        while (_shouldSpawn == true)
        {
            _spawnDelay = Mathf.Max(_spawnDelayStart - (Time.timeSinceLevelLoad/15.0f), 0.5f);
            yield return new WaitForSeconds(_spawnDelay);
            GameObject newEnemy = Instantiate(
                _enemyPrefab,
                new Vector3(Random.Range(_leftBound, _rightBound), _upperBound, 0),
                Quaternion.identity
            );
            newEnemy.transform.parent = _enemyContainer.transform;
        }
    }

    private IEnumerator PowerupSpawnRoutine(float maxWait)
    {
        while (_shouldSpawn == true)
        {
            yield return new WaitForSeconds(Random.Range(_powerupMinWait, maxWait));
            GameObject newPowerUp = Instantiate(
                _powerups[Random.Range(0, _powerups.Length)],
                new Vector3(Random.Range(_leftBound, _rightBound), _upperBound, 0),
                Quaternion.identity
            );
        }
    }

    public void OnPlayerDeath() {
        _shouldSpawn = false;
    }

    public void StartSpawns() {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine(_powerupMaxWait));
    }

}
