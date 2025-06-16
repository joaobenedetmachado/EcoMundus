using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Nome da Scene do Menu")]
    public string menuSceneName = "MenuScene";
    
    [Header("Configura��es do Jogo")]
    public float gameTime = 60f; // 1 minuto
    public int winScore = 1000; // Pontos para ganhar
    
    [Header("UI References")]
    public Text timerText;
    public GameObject winPanel;
    public GameObject losePanel;
    public Text finalScoreText;
    
    private float currentTime;
    private bool gameActive = true;
    private ScoreManager scoreManager;
    
    void Start()
    {
        // Trava o cursor no jogo (para o mouse look funcionar)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Inicializa o timer
        currentTime = gameTime;
        
        // Encontra o ScoreManager
        scoreManager = FindFirstObjectByType<ScoreManager>();
        
        // Certifica que os pain�is est�o desativados
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }
    
    void Update()
    {
        if (gameActive)
        {
            // Atualiza o timer
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
            
            // Verifica condi��es de fim de jogo
            CheckGameConditions();
        }
        
        // Pressione ESC para voltar ao menu (apenas se n�o estiver em tela de fim)
        if (Input.GetKeyDown(KeyCode.Escape) && gameActive)
        {
            ReturnToMenu();
        }
    }
    
    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
    
    void CheckGameConditions()
    {
        // Verifica se ganhou
        if (scoreManager != null && scoreManager.currentScore >= winScore)
        {
            WinGame();
        }
        // Verifica se perdeu (tempo acabou)
        else if (currentTime <= 0)
        {
            LoseGame();
        }
    }
    
    void WinGame()
    {
        gameActive = false;
        Debug.Log("VIT�RIA!");
        
        // Mostra tela de vit�ria
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            UpdateFinalScore();
        }
        
        // Destrava cursor para clicar nos bot�es
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void LoseGame()
    {
        gameActive = false;
        Debug.Log("DERROTA!");
        
        // Mostra tela de derrota
        if (losePanel != null)
        {
            losePanel.SetActive(true);
            UpdateFinalScore();
        }
        
        // Destrava cursor para clicar nos bot�es
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    void UpdateFinalScore()
    {
        if (finalScoreText != null && scoreManager != null)
        {
            finalScoreText.text = "Pontua��o Final: " + scoreManager.currentScore.ToString();
        }
    }
    
    // M�todos para os bot�es das telas de fim
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ReturnToMenu()
    {
        Debug.Log("Voltando ao menu...");
        SceneManager.LoadScene(menuSceneName);
    }
}
