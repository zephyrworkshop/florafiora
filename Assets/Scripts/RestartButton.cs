using UnityEngine;
using System.Collections;

public class RestartButton : MonoBehaviour {

	public void RestartLevel()
	{
		Application.LoadLevel("JacobWorkScene");
	}
}
