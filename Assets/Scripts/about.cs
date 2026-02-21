using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;

public class about : MonoBehaviour
{private UIDocument document;
    private TextField nameInput;
    private Button confirmButton;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        var root = document.rootVisualElement;

        nameInput = root.Q<TextField>("playerNameInput");
        confirmButton = root.Q<Button>("confirmButton");

        // Регистрируем событие кнопки
        confirmButton.RegisterCallback<ClickEvent>(OnConfirm);
    }

    private void OnDestroy()
    {
        // Отписываемся, чтобы не было утечек
        confirmButton?.UnregisterCallback<ClickEvent>(OnConfirm);
    }

    private void OnConfirm(ClickEvent evt)
    {
        string playerName = nameInput.value;

        if (string.IsNullOrEmpty(playerName))
            return; // не позволяем пустое имя

        // Сохраняем имя игрока
        PlayerPrefs.SetString("PLAYER_NAME", playerName);
        PlayerPrefs.Save();

        // Загружаем следующий уровень
        SceneController.Instance.LoadLevel();
    }
}
