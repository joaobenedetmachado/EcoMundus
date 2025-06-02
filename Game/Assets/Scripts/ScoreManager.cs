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

        // Mostra pontua��o no console tamb�m
        Debug.Log("Pontua��o atual: " + currentScore);
    }

    void UpdateScoreDisplay()
    {
        if (legacyScoreText != null)
        {
            legacyScoreText.text = "Pontos: " + currentScore.ToString();
        }
    }
}