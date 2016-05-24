using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudGenerator : MonoBehaviour {
    public GameObject[] clouds;
    public float timer;
	public List <GameObject> cloudList = new List <GameObject>();

	void Update () {
        timer += Time.deltaTime;
        if (timer >= 30)
        {
            GameObject clone;
            clone = Instantiate(clouds[Random.Range(0, 7)], new Vector3(Random.Range(-200, 200), Random.Range(-150, 150), 0), transform.rotation) as GameObject;
            clone.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-4.55f, -1.55f),0, 0);
			cloudList.Add (clone);
            Destroy(clone.gameObject, 360);
            timer = Random.Range(5, 10);
        }
	}

	public void Opacity(float opacity)
	{
		Color cloudColor;
		foreach (GameObject cloud in cloudList)
		{
			if (cloud != null)
			{
				cloudColor = cloud.GetComponent<SpriteRenderer>().material.color;
				cloudColor.a = opacity;
				cloud.GetComponent<SpriteRenderer>().material.color = cloudColor;
			}
		}
		
	}
}
