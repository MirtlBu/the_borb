using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BirdCollision : MonoBehaviour
{
    public int buildingCounter = 0;
    private GameSpeed gameSpeed;
    public bird_script bird;
    private bool levelEnding = false;
    private bool hasEnteredScreen = false;
    private Renderer birdRenderer;
    private PowerUpSpawner powerUpSpawner;
    private int powerUpSpawnAt;

    void Start()
    {
        gameSpeed = FindObjectOfType<GameSpeed>();
        bird = GetComponent<bird_script>();
        birdRenderer = GetComponent<Renderer>();
        powerUpSpawner = FindObjectOfType<PowerUpSpawner>();
        powerUpSpawnAt = Random.Range(3, 9);
    }

    void Update()
    {
        if (levelEnding) return;

        Bounds bounds = birdRenderer.bounds;

        Vector3 minViewport = Camera.main.WorldToViewportPoint(bounds.min);
        Vector3 maxViewport = Camera.main.WorldToViewportPoint(bounds.max);

        // Проверяем, полностью ли птица на экране
        if (!hasEnteredScreen)
        {
            if (minViewport.x >= 0 && maxViewport.x <= 1 &&
                minViewport.y >= 0 && maxViewport.y <= 1)
            {
                hasEnteredScreen = true;
            }
            return;
        }

        // Game Over — птица полностью ушла за экран
        if (maxViewport.x < 0 || minViewport.x > 1 ||
            maxViewport.y < 0 || minViewport.y > 1)
        {
            levelEnding = true;
            SceneManager.LoadScene("game_over");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasEnteredScreen) return;

        if (collision.CompareTag("building_counter"))
        {
            buildingCounter++;
            if (buildingCounter > 10)
            {
                StartCoroutine(EndLevel());
            }

            if (buildingCounter == powerUpSpawnAt && powerUpSpawner != null && !powerUpSpawner.HasSpawned)
            {
                powerUpSpawner.TrySpawnPowerUp();
            }
        }
    }

    private IEnumerator EndLevel()
    {
        levelEnding = true;

        if (bird != null)
            yield return StartCoroutine(bird.LeaveScreen(transform.position));

        SceneManager.LoadScene("betweenlevels");
    }
}