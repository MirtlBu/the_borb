using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    public string playerName;
    public int score;
    public void ResetData()
    {
        score = 0;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
