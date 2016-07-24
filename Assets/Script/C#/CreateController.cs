using UnityEngine;
using System.Collections;

public class CreateController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		for(int i = 1 ; i <= 40 ; i++){
			 GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			 sphere.name = "Argon "+i;
			 sphere.AddComponent<Rigidbody>();
			 sphere.tag = "Molecule";
			 sphere.GetComponent<Rigidbody>().useGravity = false;
			 sphere.AddComponent<MoleculeController>();
		}
	}
	
	// Update is called once per frame
}
