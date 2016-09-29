using UnityEngine;
using System.Collections;

public class PollenPlanet : ResourcePlanet
{

    public override void Start()
    {
        planetType = "pollen";
        makesPollen = true;
    }

    public override void OnMouseDown()
    {
		
    }

}
