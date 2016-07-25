using UnityEngine;
using System.Collections;

public class CreateController : MonoBehaviour {

	// Use this for initialization
	public static void Create (int amount) {
		for (int i = 1; i <= amount; i++) {
			GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			sphere.name = "Argon " + i;
			sphere.AddComponent<Rigidbody> ();
			sphere.tag = "Molecule";
			sphere.GetComponent<Rigidbody> ().useGravity = false;
			sphere.AddComponent<MoleculeController> ();
			sphere.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		}
	}
	
}
