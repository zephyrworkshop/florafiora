using UnityEngine;
using System.Collections;

public class PlanetArtAesthetics : MonoBehaviour {

    //planetoverhaul required for further aesthetics

    
    #region [Fore, Mid, and Background Planets]
    public GameObject forePlan;
    public GameObject midPlan;
    public GameObject backPlan;
    public GameObject tempSeedizen;
    #endregion

    float planetSize;
    public float rotSpeed;

    void Start()
    {
        rotSpeed = Random.Range(-.15f, .15f);
        #region [Randomly scales planets]
        planetSize = Random.Range(.5f, 1.5f);
        //this.transform.localScale = new Vector3(planetSize, planetSize, 1);
        #endregion
        
    }

    void Update()
    {
        #region [Randomly Rotates MidPlanet]
        //forePlan.transform.Rotate(0, 0, rotSpeed);
        midPlan.transform.Rotate(0, 0, rotSpeed / 2);
        tempSeedizen.transform.Rotate(0, 0, .65f);
        //backPlan.transform.Rotate(0, 0, rotSpeed / 4);
        #endregion
    }
}
