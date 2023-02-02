using UnityEngine;
using UnityEngine.SceneManagement;

//класс для переходов между сценами
public class SceneFinder : MonoBehaviour
{
    private string _sceneName;
    public void LoadScene(string newScene) 
    {   
        Time.timeScale = 1;
        _sceneName = newScene;
        ChangeScene();
    }
    public void ApplicationQuit() => Application.Quit(); 
    public void ReastartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    private void ChangeScene() => SceneManager.LoadScene(_sceneName);
}
