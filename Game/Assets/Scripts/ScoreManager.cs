using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int currentScore = 0;
    public Text legacyScoreText; // Arraste seu texto UI aqui

    void Start()
    {
        UpdateScoreDisplay();
    }

    public void AddPoints(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();

        // Mostra pontuação no console também
        Debug.Log("Pontuação atual: " + currentScore);
    }

    void UpdateScoreDisplay()
    {
        if (legacyScoreText != null)
        {
            legacyScoreText.text = "Pontos: " + currentScore.ToString();
        }
    }
}