using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour {

    public float opacityTimer;
    public float normTimer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        opacityTimer += Time.deltaTime;

        normTimer = Mathf.InverseLerp(opacityTimer, 0, 1);
        Debug.Log(normTimer);
            if (opacityTimer <= 30)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, normTimer);
        }
            else if (opacityTimer >= 330)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(1,1,1,(normTimer * -1));
        }
        



    }
}
