using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PitchScreen : MonoBehaviour {

    public AudioSource PitchLoop;

	public Image[] splashImages;
	//public SpriteRenderer[] splashSprites;

	public Canvas splashCanvas;

	public Button nextImage;

	private int currentPage = 0;

	void Start () {
		nextImage.enabled = false;
		SetupSprites();
	}

    void Update()
    {
        //detects if pitch intro is complete and begins loading scene
        if (currentPage >= 4 && splashImages[currentPage].GetComponent<FadeScript>().fadeComplete)
        {
            StartGame();
        }
    }


	public void SetupSprites()
	{
		//Hide all of the pages at the start.
		foreach (Image page in splashImages)
		{
            page.color = new Color(1, 1, 1, 0);
            page.enabled = false;
            
		}

    }
	public void firstPage()
	{
		nextImage.enabled = true;
		splashImages[currentPage].enabled = true;
        PitchLoop.GetComponent<AudioSource>().Play();
        splashImages[currentPage].GetComponent<FadeScript>().FadeIn();
    }

    public void NextPage()
    {
        //intercahnges image alphas for a fade effect
        if (currentPage < splashImages.Length - 1)
        {
            splashImages[++currentPage].enabled = true;
            splashImages[currentPage].GetComponent<FadeScript>().FadeIn();
            if (splashImages[currentPage].GetComponent<FadeScript>().fadeComplete)
            {
                splashImages[currentPage].GetComponent<FadeScript>().FadeOut();
            }
        }
        
    }
		
	public void StartGame()
	{
		SceneManager.LoadScene("JacobWorkScene");
	}
}

