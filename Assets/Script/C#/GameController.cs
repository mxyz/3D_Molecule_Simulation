using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class GameController : MonoBehaviour {

	private List<GameObject> allMolecule;
	public static float globalTemperature;
	public Text objNameText, speedText, forceText,tempText;
	private string nameFocus;
	private int index;
	private MoleculeController focus;
	private int count, counter, counttemp;
	private PassValue passValue;
	private bool write;
	// Use this for initialization
	void Start () {
		allMolecule = new List<GameObject> ();
		InitList ();
		count = 0;
        passValue = GameObject.FindWithTag("Pass").GetComponent<PassValue>();
        
        CreateController.Create(passValue.nom);
		this.counter = 0;


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
			float v = a.GetComponent<MoleculeController> ().moleculeSpeed*5.0f;
			float m = a.GetComponent<MoleculeController> ().moleculeMass;

			sum += (m * Mathf.Pow (v, 2))/2;
		}
		int n = allMolecule.Count;
		int n2 = 3 * n - 1;
		globalTemperature = (2 / ((1.38065f * Mathf.Pow (10, -23)) * n2)) * sum;
		return globalTemperature;
	}

	void WriteFile ()
	{
		if (counter == 0 ){
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@"Speed.txt", false)) {
				counter++;
			}
		}
		if (counter <= 200) {
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (@"Speed.txt", true)) {
				foreach (GameObject a in allMolecule) {
					float v = a.GetComponent<MoleculeController> ().moleculeSpeed;
					file.WriteLine (v * 5f);
					counter++;
					if (counter == 200) {
						counter = 0;
						break;
					}
				}
			}
			return;
		}
	}
	void WriteTemp ()
	{
		using (System.IO.StreamWriter file = new System.IO.StreamWriter (@"Temperature.txt", false)) {
			for (int i = 0; i <= 100; i++) {
				float funcc = Mathf.Pow (6.6f * Mathf.Pow (10, -26) / (2 * Mathf.PI * (1.38065f * Mathf.Pow (10, -23)) * GameController.globalTemperature), (3.0f / 2.0f));
				float func22 = 4 * Mathf.PI * i * Mathf.Pow (2.71828f, -(6.6f * Mathf.Pow (10, -26) * Mathf.Pow (i, 2)) / (2 * Mathf.PI * (1.38065f * Mathf.Pow (10, -23)) * GameController.globalTemperature));
				float finall = funcc * func22;
				file.WriteLine (finall);
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if(count == 5){
			InitList();
		}
		float t = TemperatureCalculate ();
		if (focus != null) {
			forceText.text = "Force : " + (focus.objForce.magnitude*5f).ToString("F10") + " N";
			objNameText.text = "Name : " + focus.objName;
			float s = focus.moleculeSpeed*5.0f;
			speedText.text = "Speed : " + s + " m/s";

		}
		tempText.text = "Temperature : " + t + " K";
		if (count >= 20) {
			WriteFile ();
			WriteTemp ();
		}
		count++;
	}
	public void changeFocus (MoleculeController molecule)
	{
		if (focus == null) {
			focus = molecule;
		} else {
			focus.changeClickOn ();
			focus = molecule;
		}
	}
	public static GameController getInstance ()
	{
		return GameObject.Find ("MainController").GetComponent<GameController> ();
	}

}
