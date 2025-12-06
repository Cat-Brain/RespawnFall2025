using TMPro;
using UnityEngine;

public class DifficultyDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;

    GameManager gameManager;

    void Update()
    {
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();
        text.text = gameManager.difficulty.ToString();
    }
}
