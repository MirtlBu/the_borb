using UnityEngine;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    public LeaderboardService backendService;
    public Label scoreList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        scoreList = root.Q<Label>("score_list");
        List<LeaderboardService.PlayerScore> leaderboard =
            await backendService.GetLeaderboardAsync();
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
