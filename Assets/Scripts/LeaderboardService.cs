using System;
using System.Collections.Generic;
using System.Net.Http;
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

    private static readonly string leaderboardUrl = "https://jutskat.fi/wp-content/3d/borb/leaderboard_resp.json";


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
}
