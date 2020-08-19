using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speedUpperBound = 3.0f;
    private float _speedLowerBound = 4.0f;

    [SerializeField]
    private float _speed;
    
    private float _upperBound = 8f;
    private float _lowerBound = -6f;
    private float _leftBound = -10f;
    private float _rightBound = 10f;
    private int _lives = 1;
    
    private Player _player = null;
    private Animator _animator = null;
    private bool _isDestroyed = false;
    private PolygonCollider2D _collider;

    [SerializeField]
    private AudioSource _explosionAudio = null;

    // Start is called before the first frame update
    void Start()
    {
        _speed = Random.Range(_speedLowerBound, _speedUpperBound);
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<PolygonCollider2D>();

        if (_player == null)
        {
            Debug.LogError("The Player is not instantiated.");
        }
        if (_animator == null)
        {
            Debug.LogError("The Animator is not instantiated.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= _lowerBound)
        {
            if (_player)
            {
                _player.AddToScore(-10);
            }
            transform.position = new Vector3(Random.Range(_leftBound, _rightBound), _upperBound, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage(1);
            }

            DestroyEnemy();
        }

        if (other.tag == "Projectile")
        {
            if (_lives > 1 ) {
                _lives--;
            } else {
                DestroyEnemy();
                if (_player != null)
                {
                    _player.AddToScore(5);
                }
            }
            Destroy(other.gameObject);
        }
    }

    private void DestroyEnemy()
    {
        _collider.enabled = false;
        _animator.SetTrigger("OnDestroyed");
        // Debug.Log("Animator State Name: " + _animator.GetCurrentAnimatorStateInfo(0).IsName("Standard"));
        _speed = 0;
        _explosionAudio.Play();
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        while (_isDestroyed == false)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyExplode_anim") &&
                _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Destroy(this.gameObject);
                _isDestroyed = true;
            } else
            {
                yield return null;
            }
        }
    }
}
