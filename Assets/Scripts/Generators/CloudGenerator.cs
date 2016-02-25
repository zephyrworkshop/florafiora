using UnityEngine;
using System.Collections;

public class CloudGenerator : MonoBehaviour {
    public GameObject[] clouds;
    public float timer;

	void Update () {
        timer += Time.deltaTime;
        if (timer >= 5)
        {
            GameObject clone;
            clone = Instantiate(clouds[Random.Range(0, 7)], transform.position, transform.rotation) as GameObject;
            clone.transform.position += new Vector3(0, Random.Range(-100, 100));
            clone.GetComponent<Rigidbody2D>().velocity = -transform.right * Random.Range(1.55f, 4.5f);
            Destroy(clone.gameObject, 30);
            timer = Random.Range(0, 2.5f);
        }
	}
}
