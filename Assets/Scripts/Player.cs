using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _speed = 0.0f;
    private float _defaultSpeed = 11.0f;
    private float _powerupSpeed = 18.0f;

    [SerializeField]
    private GameObject _laserPrefab = null;

    [SerializeField]
    private GameObject _tripleShotPrefab = null;

    [SerializeField]
    private float _laserOffset = 1.05f;

    [SerializeField]
    private float _fireRate = 0.25f;
    private float _nextFire = 0.0f;

    [SerializeField]
    private float _powerupDuration = 5.0f;

    private SpawnManager _spawnManager = null;
    private UIManager _uiManager = null;

    [SerializeField]
    private bool _isTripleShotEnabled = false;

    [SerializeField]
    private int _shield = 0;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private GameObject _shieldVisual = null;

    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private GameObject _thruster = null;

    [SerializeField]
    private GameObject _leftEngineFire = null;

    [SerializeField]
    private GameObject _rightEngineFire = null;

    [SerializeField]
    private GameObject _explosion = null;

    private bool _isPlayerDestroyed = false;

    [SerializeField]
    private AudioSource _laserAudio = null;

    [SerializeField]
    private AudioSource _tripleShotAudio = null;

    void Start()
    {
        _speed = _defaultSpeed;
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _leftEngineFire.SetActive(false);
        _rightEngineFire.SetActive(false);

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is not instantiated.");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is not instantiated.");
        }

        _thruster.SetActive(false);
    }

    void Update()
    {
        if (_isPlayerDestroyed == false)
        {
            CalculateMovement();
            if (Input.GetKey(KeyCode.Space) && Time.time >= _nextFire)
            {
                ShootLaser();
            }
            CalculateThruster();
        }
    }

    void CalculateThruster() {
        float verticalInput = Input.GetAxis("Vertical");
        if ( verticalInput > 0)
        {
            _thruster.SetActive(true);

        }
        else
        {
            _thruster.SetActive(false);
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        // transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        float upperBound = 0f;
        float lowerBound = -3.8f;
        float leftBound = -11.3f;
        float rightBound = 11.3f;

        // Handle Y Axis
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, lowerBound, upperBound), 0);

        // Handle X Axis
        if (transform.position.x <= leftBound)
        {
            transform.position = new Vector3(rightBound, transform.position.y, 0);
        }
        else if (transform.position.x >= rightBound)
        {
            transform.position = new Vector3(leftBound, transform.position.y, 0);
        }
    }

    void ShootLaser()
    {
        _nextFire = Time.time + _fireRate;
        
        if (_isTripleShotEnabled == true)
        {
            Instantiate(
                 _tripleShotPrefab,
                transform.position + new Vector3(0, 0, 0),
                Quaternion.identity
            );
            _tripleShotAudio.Play();
        }
        else
        {
            Instantiate(
                _laserPrefab,
                transform.position + new Vector3(0, _laserOffset, 0),
                Quaternion.identity
            );
            _laserAudio.Play();
        }
        
    }

    public void Damage(int damage)
    {
        // if (_shield > 0) {
        //     if (_shield >= damage) {
        //         _shield -= damage;
        //     } else {
        //         _shield = 0;
        //         _lives -= damage - _shield;
        //     }   
        // } else {
        //     _lives -= damage;
        // }
        if (_shield >= damage)
        {
            _shield -= damage;
        }
        else
        {
            _shield = 0;
            _lives -= damage - _shield;
        }
        if (_shield <= 0)
        {
            _shieldVisual.SetActive(false);
        }
        
        _uiManager.UpdateLivesDisplay(_lives);

        if (_lives <= 0)
        {
            _isPlayerDestroyed = true;
            _spawnManager.OnPlayerDeath();
            GameObject explosion = Instantiate(
                _explosion,
                transform.position,
                Quaternion.identity
            );
            Destroy(this.gameObject, 0.5f);
        }

        if (_lives <= 2) {
            _rightEngineFire.SetActive(true);
        }
        if (_lives <=1 )
        {
            _leftEngineFire.SetActive(true);
        }
    }

    // public void EnableTripleShot() {
    //     _isTripleShotEnabled = true;
    // }

    public void EnableTripleShot()
    {
        _isTripleShotEnabled = true;
        StartCoroutine(DisableTripleShot());
    }

    IEnumerator DisableTripleShot()
    {
        yield return new WaitForSeconds(_powerupDuration);
        _isTripleShotEnabled = false;
    }

    public void EnableSpeed()
    {
        _speed = _powerupSpeed;
        StartCoroutine(DisableSpeed());
    }

    IEnumerator DisableSpeed()
    {
        yield return new WaitForSeconds(_powerupDuration);
        _speed = _defaultSpeed;
    }

    public void EnableShield()
    {
        _shield = 1;
        _shieldVisual.SetActive(true);
        StartCoroutine(DisableShield());
    }

    IEnumerator DisableShield()
    {
        yield return new WaitForSeconds(_powerupDuration);
        _shieldVisual.SetActive(false);
        _shield = 0;
    }

    public void AddToScore(int score)
    {
        _score += score;
        _score = Mathf.Max(_score, 0);
        _uiManager.UpdateScore(_score);
    }
}
