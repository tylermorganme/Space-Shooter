using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField]
    private GameObject _explosion = null;
    private SpawnManager _spawnManager = null;

    private float _rotationSpeed = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is not instantiated.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            _spawnManager.StartSpawns();
            GameObject explosion = Instantiate(
                _explosion,
                transform.position,
                Quaternion.identity
            );
            Destroy(this.gameObject, 0.5f);
            Destroy(other.gameObject);
        }
    }
}
