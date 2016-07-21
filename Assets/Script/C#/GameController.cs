using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class GameController : MonoBehaviour {

	private List<GameObject> allMolecule;
	public static float globalTemperature;
	public Text countText, objNameText, speedText, vectorText, positionText;
	private string name;
	private int index;
	private MoleculeController focus;
	// Use this for initialization
	void Start () {
		allMolecule = new List<GameObject> ();
		InitList ();
		countText.text = "Count : " + allMolecule.Count;
		this.name = "";
	}

	void InitList ()
	{
		GameObject [] findTag = GameObject.FindGameObjectsWithTag ("Molecule");
		foreach (GameObject m in findTag) {
			allMolecule.Add (m);
		}
	}

	float TemperatureCalculate ()
	{
		float sum = 0;
		foreach (GameObject a in allMolecule) {
			float v = a.GetComponent<MoleculeController> ().moleculeSpeed;
			float m = a.GetComponent<MoleculeController> ().moleculeMass;

			sum += (m * Mathf.Pow (v, 2))/2;
		}
		int n = allMolecule.Count;
		int n2 = 3 * n - 1;
		globalTemperature = (2 / ((1.38065f * Mathf.Pow (10, -23)) * n2)) * sum;
		return globalTemperature;
	}


	// Update is called once per frame
	void FixedUpdate ()
	{
		TemperatureCalculate ();
		checkFocus ();
		//Debug.Log ("Tempurature =" + globalTemperature);
		if (focus != null) {
			countText.text = "Count : " + allMolecule.Count + " Molecules";
			objNameText.text = "Name : " + focus.objName;
			speedText.text = "Velocity : " + focus.moleculeSpeed + " m/s";

		}
	}

	void checkFocus ()
	{
		foreach (GameObject a in allMolecule) {
			MoleculeController temp = a.GetComponent<MoleculeController> ();
			if (temp.clickOn) {
				if (temp.objName == name) {
					index = allMolecule.IndexOf (a);
				} else if (temp.objName != name) {
					allMolecule[index].GetComponent<MoleculeController>().clickOn = false;
					focus = temp;

				}
			}
		}
	}
}
