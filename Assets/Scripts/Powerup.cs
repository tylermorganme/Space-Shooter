using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private string _powerupName = null;

    private float _lowerBound = -5.0f;

    [SerializeField]
    private AudioClip _powerupAudioClip = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= _lowerBound ) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Player") {
            AudioSource.PlayClipAtPoint(_powerupAudioClip, transform.position);
            Player player = other.transform.GetComponent<Player>();
            switch(_powerupName)
            {
                case "TripleShot":
                    player.EnableTripleShot();
                    break;
                case "Speed":
                    player.EnableSpeed();
                    break;
                case "Shield":
                    player.EnableShield();
                    break;
                default:
                    break;
            }
            
            Destroy(this.gameObject);
        }
    }
}
