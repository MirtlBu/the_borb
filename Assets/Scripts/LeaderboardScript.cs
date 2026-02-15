using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Leaderboard : MonoBehaviour
{
    public LeaderboardService leaderboardService;
    public Label scoreList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        scoreList = root.Q<Label>("score_list");
        List<LeaderboardService.PlayerScore> leaderboard =
            await leaderboardService.GetLeaderboardAsync();
        StringBuilder sb = new StringBuilder();
        foreach (LeaderboardService.PlayerScore playerScore in leaderboard)
        {
            sb.AppendLine($"{playerScore.playerName} - {playerScore.score}");
        }
        scoreList.text = sb.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
