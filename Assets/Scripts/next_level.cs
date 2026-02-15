using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class next_level_script : MonoBehaviour
{
    public LeaderboardService leaderboardService;

    private UIDocument document;
    private Button button;
    private Label nameLabel;

    private async void Awake()
    {
        LeaderboardService.PlayerScore playerScore = LeaderboardService.GetCurrentPlayerScore();
        var root = GetComponent<UIDocument>().rootVisualElement;
        nameLabel = root.Q<Label>("playerNameLabel");
        nameLabel.text = $"{playerScore.playerName} helped the borb collect {playerScore.score} coin{(playerScore.score != 1 ? "s" : "")}, but it’s not enough, so let’s fly to another city.";
        await leaderboardService.UpdateLeaderboardAsync(playerScore);
    }

    private void OnEnable()
    {
        document = GetComponent<UIDocument>();
        button = document.rootVisualElement.Q<Button>("next");
        button.RegisterCallback<ClickEvent>(OnPlayGameClick);
    }

    private void OnDisable()
    {
        button?.UnregisterCallback<ClickEvent>(OnPlayGameClick);
    }

    private void OnPlayGameClick(ClickEvent evt)
    {
        SceneManager.LoadScene("level");
    }
}
