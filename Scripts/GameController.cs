using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject [] hazards;
    public Vector3 spawnValues;
    private float hazardCount;
    private float spawnWait;
    public float startWait;
    public float waveWait;
    public GameObject background;

    public Text ScoreText;
    public Text restartText;
    public Text gameOverText;
    public Text winText;

    public AudioSource musicSource;
    public AudioClip musicWin;
    public AudioClip musicLose;
    public AudioClip musicBackground;

    private bool gameOver;
    private bool restart;
    private int score;
    private readonly Mover mover;

    void Start()
    {
        spawnWait = 0.6f;
        hazardCount = 10;
        musicSource.clip = musicBackground;
        musicSource.Play();
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        winText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }
    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            hazardCount = 25;
            spawnWait = 0.3f;
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
    IEnumerator SpawnWaves()
    {
        
yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'Y' for Restart";
                restart = true;
                break;
            }
            if (score >= 300)
            {
                GameWin();
                restartText.text = "Press 'Y' for Restart";
                restart = true;
                break;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        ScoreText.text = "Points: " + score;
    }
    public void GameOver()
    {
        musicSource.clip = musicLose;
        musicSource.Play();
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
    public void GameWin()
    {
        musicSource.clip = musicWin;
        musicSource.Play();
        winText.text = "You Win! Game created by Dominic Lincourt";
        Destroy(GameObject.Find("hazard"));
        Destroy(GameObject.Find("Background"));
        Destroy(GameObject.Find("StarField"));
        gameOver = true;
    }
}