using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudGenerator : MonoBehaviour {
    public GameObject[] clouds;
    public float timer;
	public List <GameObject> cloudList = new List <GameObject>();

	void Update () {
        timer += Time.deltaTime;
        if (timer >= 10)
        {
            GameObject clone;
            clone = Instantiate(clouds[Random.Range(0, 7)], transform.position, transform.rotation) as GameObject;
            clone.transform.position += new Vector3(0, Random.Range(-150, 150));
            clone.GetComponent<Rigidbody2D>().velocity = -transform.right * Random.Range(1.55f, 4.5f);
			cloudList.Add (clone);
            Destroy(clone.gameObject, 120);
            timer = Random.Range(0, 3.5f);
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
