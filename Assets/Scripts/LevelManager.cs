using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
  
    public void Respawn()
    {

        // Restart the Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Reset Player's Position


    }

}
