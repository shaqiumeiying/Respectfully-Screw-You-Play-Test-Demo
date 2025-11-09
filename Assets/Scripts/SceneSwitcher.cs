using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string[] sceneNames = { "A", "B", "C" };
    private int currentSceneIndex = 0;
    private static SceneSwitcher instance;

    void Awake()
    {
        // Singleton pattern to prevent duplicates
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Press Enter or Controller Y (JoystickButton3)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            SwitchScene();
        }
    }

    void SwitchScene()
    {
        currentSceneIndex = (currentSceneIndex + 1) % sceneNames.Length;
        SceneManager.LoadScene(sceneNames[currentSceneIndex]);
    }
}
