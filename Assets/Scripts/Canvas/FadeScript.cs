using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeScript : MonoBehaviour {

    public float timer;

    public bool fadingIn;
    public bool fadingOut;

    public bool fadeComplete;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        if (fadingIn && !fadeComplete)
        {
            timer += Time.deltaTime;
            GetComponent<Image>().color = new Color(1, 1, 1, timer / 3);
            if (timer >= 5)
            {
                fadeComplete = true;
                timer = 5;
            }
        }
        if (fadingOut && !fadeComplete)
        {
            timer -= Time.deltaTime;
            GetComponent<Image>().color = new Color(1, 1, 1, timer / 3);
            if (timer <= 0 )
            {
                fadeComplete = true;
                timer = 0;
            }
        }
    }

    public void FadeIn()
    {
        fadeComplete = false;
        fadingOut = false;
        fadingIn = true;

    }
    public void FadeOut()
    {
        fadeComplete = false;
        fadingIn = false;
        fadingOut = true;
    }

}
