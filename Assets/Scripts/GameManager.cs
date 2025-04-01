using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Text scoreText;
    public Text heartsText;
    private int score = 0;
    private int hearts = 3;

    void Awake()
    {
        Instance = this;
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void LoseHeart()
    {
        hearts--;
        heartsText.text = hearts.ToString();
        if (hearts <= 0)
        {
            Debug.Log("Game Over!");
            Time.timeScale = 0; // Spiel pausieren
        }
    }
}