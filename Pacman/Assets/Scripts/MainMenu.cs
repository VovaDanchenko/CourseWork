using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ShowScores()
    {
        SceneManager.LoadScene(2);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void EasyDiff()
    {
        SceneManager.LoadScene(5);
    }
    public void MidDiff()
    {
        SceneManager.LoadScene(1);
    }
    public void HighDiff()
    {
        SceneManager.LoadScene(4);
    }
    public void ChooseDiff()
    {
        SceneManager.LoadScene(6);
    }
    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }
}
