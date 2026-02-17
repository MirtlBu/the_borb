using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


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

            using UnityWebRequest request = UnityWebRequest.Get(leaderboardUrl);
            var operation = request.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Leaderboard request failed: {request.error}");
                return new List<PlayerScore>();
            }

            string json = request.downloadHandler.text;
            LeaderboardResponse result = JsonUtility.FromJson<LeaderboardResponse>(json);

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
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            using UnityWebRequest request = new UnityWebRequest(leaderboardUrl, UnityWebRequest.kHttpVerbPOST);
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Leaderboard update failed: {request.error}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Leaderboard update failed: {ex.Message}");
            return false;
        }
    }
}
