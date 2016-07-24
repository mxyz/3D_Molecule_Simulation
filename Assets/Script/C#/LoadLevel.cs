using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	void Update(){
	}
	public void LoadFirstLevel(){
		Application.LoadLevel("Simulation Scene");
	}
	void OnMouseDown(){
     	Application.LoadLevel("Simulation Scene");
 }
}
