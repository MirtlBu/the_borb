using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class game_over : MonoBehaviour
{
    public LeaderboardService leaderboardService;

    private UIDocument document;
    private Button button;
    private async void OnEnable()
    {
        document = GetComponent<UIDocument>();
        button = document.rootVisualElement.Q<Button>("continue");
        button.RegisterCallback<ClickEvent>(OnPlayGameClick);

        await leaderboardService.UpdateLeaderboardAsync(LeaderboardService.GetCurrentPlayerScore());
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
