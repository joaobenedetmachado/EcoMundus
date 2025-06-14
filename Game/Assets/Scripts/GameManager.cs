using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Nome da Scene do Menu")]
    public string menuSceneName = "MenuScene";
    
    void Start()
    {
        // Trava o cursor no jogo (para o mouse look funcionar)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        // Pressione ESC para voltar ao menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
        }
    }
    
    public void ReturnToMenu()
    {
        Debug.Log("Voltando ao menu...");
        SceneManager.LoadScene(menuSceneName);
    }
    
    // Método para reiniciar o jogo
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
