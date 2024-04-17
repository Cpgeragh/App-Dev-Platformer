
using UnityEngine;
using UnityEngine.UI;

public class HeartCount : MonoBehaviour
{
    public Image[] hearts;
    public int heartsRemaining;

    public void LoseHeart()
    {

        if(heartsRemaining == 0)
        {

            return;

        }

        heartsRemaining--;

        hearts[heartsRemaining].enabled = false;
        
        if(heartsRemaining <= 0)
        {
            
            FindObjectOfType<PlayerMovement>().Die();
            FindObjectOfType<LevelManager>().Respawn();
            Debug.Log("YOU LOSE");
            
            

        }

    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Return))
        {

            LoseHeart();

        }

    }

}
