using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    public GameObject PauseButton;
    public GameObject PauseMenuUI;

    public void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        PauseButton.GetComponent<Image>().color = new Color(1,1,1,0);
        PauseButton.GetComponent<Button>().interactable = false;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        PauseButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        PauseButton.GetComponent<Button>().interactable = true;
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        //Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene("JacobWorkScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
