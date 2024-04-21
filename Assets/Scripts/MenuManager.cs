using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Level1Intro()
    {
        // Attempt to load the Level1 scene by build index
        SceneManager.LoadScene(1); // Change the index to match the Level1 scene's build index
    }

    public void Level1()
    {
        // Attempt to load the Level1 scene by build index
        SceneManager.LoadScene(2); // Change the index to match the Level1 scene's build index
    }

    public void Level2Intro()
    {
        // Attempt to load the Level1 scene by build index
        SceneManager.LoadScene(3); // Change the index to match the Level1 scene's build index
    }

    public void Level2()
    {
        // Attempt to load the Level1 scene by build index
        SceneManager.LoadScene(4); // Change the index to match the Level1 scene's build index
    }

    public void Level3Intro()
    {
        // Attempt to load the Level1 scene by build index
        SceneManager.LoadScene(5); // Change the index to match the Level1 scene's build index
    }

    public void Level3()
    {
        // Attempt to load the Level1 scene by build index
        SceneManager.LoadScene(6); // Change the index to match the Level1 scene's build index
    }

    public void MainMenu()
    {
        // Attempt to load the Level1 scene by build index
        SceneManager.LoadScene(0); // Change the index to match the Level1 scene's build index
    }

    public void OpenSettings()
    {
        // Logic to open settings; this could load another scene or toggle a settings panel
        Debug.Log("Open settings panel or scene.");
    }

    public void OpenTutorial()
    {
        // Add logic to open the tutorial scene or show a tutorial panel
        SceneManager.LoadScene("TutorialScene"); // Load the tutorial scene by name
    }

    public void ExitGame()
    {
        Application.Quit();  // This will only work in a build, not in the Unity editor
        Debug.Log("Exit game.");
    }
}