using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class LeaderboardService : MonoBehaviour
{
    [Serializable]
    public class PlayerScore
    {
        public string playerName;
        public int score;
    }

    [Serializable]
    public class LeaderboardResponse
    {
        public List<PlayerScore> leaderboard;
    }

    private static HttpClient httpClient = new HttpClient();

    private static readonly string leaderboardUrl = "https://jutskat.fi/wp-json/borb/leaderboard";


    public static PlayerScore GetCurrentPlayerScore()
    {
        string playerName = PlayerPrefs.GetString("PLAYER_NAME", "Player");
        int coinsCollected = 0;
        if (PlayerData.Instance != null)
        {
            coinsCollected = PlayerData.Instance.score;
        }

        return new PlayerScore
        {
            playerName = playerName,
            score = coinsCollected
        };
    }


    public async Task<List<PlayerScore>> GetLeaderboardAsync()
    {
        try
        {
            Debug.Log("Requesting leaderboard...");

            HttpResponseMessage response = await httpClient.GetAsync(leaderboardUrl);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            LeaderboardResponse result =
                JsonUtility.FromJson<LeaderboardResponse>(json);

            return result.leaderboard;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Leaderboard request failed: {ex.Message}");
            return new List<PlayerScore>();
        }
    }

    public async Task<bool> UpdateLeaderboardAsync(PlayerScore playerScore)
    {
        try
        {
            Debug.Log("Updating leaderboard...");

            string json = JsonUtility.ToJson(playerScore);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(leaderboardUrl, content);
            response.EnsureSuccessStatusCode();

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Leaderboard update failed: {ex.Message}");
            return false;
        }
    }
}
