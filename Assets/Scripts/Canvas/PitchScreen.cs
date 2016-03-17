using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PitchScreen : MonoBehaviour {

	public Image[] splashImages;
	//public SpriteRenderer[] splashSprites;

	public Canvas splashCanvas;

	public Button nextImage;

	private int currentPage = 0;

	// Use this for initialization
	void Start () {
		nextImage.enabled = false;
		SetupSprites();
	}

	public void SetupSprites()
	{
		//Hide all of the pages at the start.
		foreach (Image page in splashImages)
		{
			page.enabled = false;
		}
		//Show the starting page.
		//splashImages[currentPage].enabled = true;
	}

	public void firstPage()
	{
		nextImage.enabled = true;
		splashImages[currentPage].enabled = true;
	}

	public void NextPage()
	{

		if (currentPage < splashImages.Length - 1)
		{
			splashImages[currentPage].enabled = false;
			splashImages[++currentPage].enabled = true;
		}
		else
		{
			StartGame ();
		}

	}
		

	public void StartGame()
	{
		Application.LoadLevel("JacobWorkScene");
	}

}

