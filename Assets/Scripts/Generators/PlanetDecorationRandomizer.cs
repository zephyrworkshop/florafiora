using UnityEngine;
using System.Collections;

public class PlanetDecorationRandomizer : MonoBehaviour {
    public bool shouldRot;
    bool isRotating;
    float rotVal;

	void Start () {
	if (Random.Range(0, 2) >= 1)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-360, 360));
            if (Random.Range(0, 4) >= 1 && shouldRot)
            {
                isRotating = true;
                rotVal = Random.Range(-.025f, .025f);
            }
        }
	}

	void Update () {
	if (isRotating)
        {
            transform.Rotate(0, 0, rotVal);
        }
	}
}
