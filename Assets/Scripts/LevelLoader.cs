using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetDetails {
	public Vector3 position = Vector3.zero;
	public string type = "ERROR";
}

public class LevelLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static List <PlanetDetails> LoadLevel (string fileName) {
		//Name|Description|Tags|Stats|Actions|Special
		List<PlanetDetails> items = new List<PlanetDetails> ();
		
		string[] text = new string[0];
		try{
			//text = System.IO.File.ReadAllLines(Application.dataPath + "/Resources/" + fileName);//does not work with building
			TextAsset ta = Resources.Load (fileName) as TextAsset;
			text = ta.text.Split ('\n');
		} catch (System.Exception e) {
			Debug.Log ("Unable to read file: " + fileName);
			throw e;
		}
		if (text.Length < 1) {
			Debug.Log ("Could not read: " + fileName);
			return items;
		}
		
		List<string> categories = new List<string> ( text [0].Trim().Split (','));
		
		for (int i = 1; i < text.Length; i++) {
			try{
				if(text[i].Trim() == "")
					continue;//some lines may be blank, I guess
				
				string[] values = text[i].Trim().Split(',');
				string x = values[categories.IndexOf("x")];
				string y = values[categories.IndexOf("y")];
				string type = values[categories.IndexOf("type")];
				
				PlanetDetails item = new PlanetDetails ();
				item.position = new Vector3 (float.Parse (x), float.Parse (y), 0f);
				item.type = type;

				items.Add (item);
			} catch (System.Exception e) {
				Debug.Log ("Failure to read planet: " + text[i] + " from level " + fileName);
				Debug.Log (e.Message);
				//throw e;
			}
		}
		
		return items;
	}
}
