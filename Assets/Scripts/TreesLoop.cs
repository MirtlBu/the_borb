using UnityEngine;

public class TreesLoop : MonoBehaviour
{
    private GameSpeed gameSpeed;

    private SpriteRenderer sr;
    private float spriteWidth;
    private bool spawnedNext = false;

    void Start()
    {
        gameSpeed = FindObjectOfType<GameSpeed>();
        sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        transform.position += Vector3.left * gameSpeed.speed / 3 * Time.deltaTime;

        float leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero).x;

        if (!spawnedNext && transform.position.x < spriteWidth / 2f)
        {
            SpawnNext();
            spawnedNext = true;
        }

        if (transform.position.x + spriteWidth / 2f < leftEdge)
        {
            Destroy(gameObject);
        }
    }

    void SpawnNext()
    {
        Vector3 newPos = transform.position;
        newPos.x += spriteWidth;
        Instantiate(gameObject, newPos, Quaternion.identity);
    }
}
