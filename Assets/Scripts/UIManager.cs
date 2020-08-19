using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText = null;

    [SerializeField]
    private Image _lives = null;

    [SerializeField]
    private Text _gameOverText = null;

    [SerializeField]
    private Sprite[] _livesSprites = null;

    private bool _isGameOver = false;

    [SerializeField]
    private float _gameOverFlickerInterval = 1.5f;

    [SerializeField]
    private Text _resetText = null;

    // Start is called before the first frame update
    void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _resetText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void UpdateScore(int score)
    {
        // _scoreText.text = "Score: " + score.ToString();
        _scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateLivesDisplay(int lives)
    {
        _lives.sprite = _livesSprites[lives];
        if (lives <= 0)
        {
            _gameOverText.gameObject.SetActive(true);
            _isGameOver = true;
            StartCoroutine(GameOverFlicker());
            _resetText.gameObject.SetActive(true);
        }
    }

    private IEnumerator GameOverFlicker() {
        while (_isGameOver)
        {
            yield return new WaitForSeconds(_gameOverFlickerInterval);
            _gameOverText.gameObject.SetActive(!_gameOverText.gameObject.activeSelf);
        }
    }
}
