using UnityEngine;
using System.Collections;

public class tempOwlSpawner : MonoBehaviour {
    public float spawnTimer;
    public GameObject owl;
    int spawned;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > 10 && spawned <= 10)
        {
            spawned++;
            Instantiate(owl, transform.position, transform.rotation);
            spawnTimer = 0;
        }
            
	}
}
