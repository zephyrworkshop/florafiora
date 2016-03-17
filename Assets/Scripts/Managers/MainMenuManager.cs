using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	public PitchScreen splashCanvas;
	public CloudGeneratorMainMenu menuClouds;
	public Canvas playCanvas;

    public void StartGame()
    {
    }

	public void SplashGUI()
	{
		menuClouds.enabled = false;
		playCanvas.enabled = false;
		splashCanvas.firstPage ();
	}
}
