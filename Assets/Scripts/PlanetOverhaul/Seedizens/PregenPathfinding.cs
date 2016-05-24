using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PregenPathfinding {

	public static PregenPlanet GetDirection (PregenPlanet planet, PregenSeedizen seedizen) {
		var q = new Queue <PregenPlanet> ();
		var discovered = new HashSet <PregenPlanet> ();
		var pathTo = new Dictionary <PregenPlanet, PregenPlanet> ();

		q.Enqueue (planet);
		discovered.Add (planet);

		while (q.Count > 0) {
			var p = q.Dequeue ();

			if (p == null || p.planetType == null)
				continue;

			//check for victory
			if (PlanetIsDestination (p, seedizen)) {
				//success! construct and return the path
				PregenPlanet curPlanet = p;
				while (curPlanet != null && pathTo.ContainsKey (curPlanet)) {
					var prevPlanet = pathTo [curPlanet];
					if (!pathTo.ContainsKey (prevPlanet))
						return curPlanet;
					curPlanet = prevPlanet;
				}
			}

			var neighbors = new List <PregenPlanet> (p.connectedPlanets);
			var neighbors2 = new List <PregenPlanet> ();
			//randomize neighbors
			while (neighbors.Count > 0) {
				int at = Random.Range (0, neighbors.Count);
				neighbors2.Add (neighbors [at]);
				neighbors.RemoveAt (at);
			}

			foreach (var n in neighbors2) {
				if (discovered.Contains (n))
					continue;

				discovered.Add (n);
				pathTo [n] = p;
				q.Enqueue (n);
			}
		}

		//welp, return a random one
		var options = new List <PregenPlanet> (planet.connectedPlanets);
		foreach (var p in planet.connectedPlanets)
			if (planet.vines [p].dispreferred)
				options.Remove (p);
		if (options.Count == 0) {
			return null;
		}
		return options [Random.Range (0, options.Count)];
	}

	private static bool PlanetIsDestination (PregenPlanet planet, PregenSeedizen seedizen) {
		if (seedizen.hasPollen) {
			return planet.hasDemands.NeedsPollenOf (seedizen);
		} else {
			return planet.makesPollen;
		}
	}
}
