using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	public void LoadFirstLevel(){
		Application.LoadLevel("Simulation Scene");
	}
	void OnMouseDown(){
     	Application.LoadLevel("Simulation Scene");
 }
}
