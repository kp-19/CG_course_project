using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {   
        //add name of the scene you have given
        SceneManager.LoadScene("GameScene1");
    }

    public void QuitGame()
    {
        //Debug.Log("Quit button clicked - Application would quit in build");
        Application.Quit();
    }
}
