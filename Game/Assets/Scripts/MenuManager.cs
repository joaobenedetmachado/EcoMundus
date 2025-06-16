using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Nome da Scene do Jogo")]
    public string gameSceneName = "PanelsScene"; // Mude para o nome da sua scene do jogo

    void Start()
    {
        // Destrava o cursor no menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // M�todo chamado pelo bot�o Jogar
    public void StartGame()
    {
        Debug.Log("Iniciando jogo...");
        SceneManager.LoadScene(gameSceneName);
    }

    // M�todo chamado pelo bot�o Sair
    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");

        // Funciona apenas no build, n�o no editor
        Application.Quit();

        // Para testar no editor Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
