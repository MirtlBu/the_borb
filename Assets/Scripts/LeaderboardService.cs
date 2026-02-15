using System;
using System.Collections.Generic;
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
    public async Task<List<PlayerScore>> GetLeaderboardAsync()
    {
        await Task.Delay(2000);

        List<PlayerScore> fakeData = new List<PlayerScore>();

        for (int i = 0; i < 10; i++)
        {
            fakeData.Add(new PlayerScore
            {
                playerName = $"Player_{i + 1}",
                score = UnityEngine.Random.Range(0, 10000)
            });
        }

        return fakeData;
    }
}
