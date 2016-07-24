using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoleculeController : MonoBehaviour
{

	// Attributes
	private Rigidbody rb; //force of molecule
	private float [] speed; // x=[0] y=[1] z=[2]
	private char [] dir; //xd=[0] yd=[1] zd=[2]
	private float velocity; //velocity of molecule
	private float realV;
	private int count;
	private Vector3 movement;
	private float maxVelocity;
	private float sqrMaxVelocity;
	private float tempX;
	private float tempY;
	private float tempZ;
	public string moleculeName;
	public float moleculeMass;
	public float moleculeSpeed;
	public bool clickOn;

	// Attributes for Lannard Jones potential
	private float maxDistance; // Maximun distance
	private float sigma; // σ is the main force for Attractive and Rupulsive force should greater than Maximum Distance
	private float epsilon; // Ɛ 

	// Another Attributes
	private int numberOfMolecule;
	public string objName;
	private Vector3 tempObjPos; // Use when want to set it stay at -15
	public Vector3 objForce;
	private Vector3[] forceFromObj;
	private bool [] _fromObj;
	private List<GameObject> relation, relation2; // Molecule that relate with this molecule
	// Functions
	bool checkDirections ()
	{
		// if all dir are 's' or 'null' return -1, otherwise, return 1
		if (dir [0] == dir [1] && dir [1] == dir [2])
			return false;
		else
			return true;
	}
	void randomPosition ()
	{
		float rndPosx = Random.Range (-13, 13);
		float rndPosy = Random.Range (-14, 9);
		float rndPosz = Random.Range (-13, 13);
		transform.position = new Vector3 (rndPosx, rndPosy, rndPosz);
	}


	char randomDirection ()
	{
		// Random 0, 1, 2
		//Have 3 State f = Forward, s = Still, b = Backward

		int rnd = Random.Range (0, 3);
		if (rnd == 0) {
			this.count++;
			return 'f';
		} else if (rnd == 1) {
			return 's';
		} else if (rnd == 2) {
			this.count++;
			return 'b';
		} else
			return 'n';
	}

	void setDirection ()
	{
		for (int i = 0; i < 3; i++) {
			dir [i] = randomDirection ();
		}
	}




	void setSpeed ()
	{
		float rV = velocity / count;
		//Debug.Log ("x " +dir[0]+ " y " +dir[1]+ " z "+dir[2]);
		for (int i = 0; i < 3; i++) {
			if (dir [i] == 'f')
				speed [i] = rV;
			else if (dir [i] == 'b')
				speed [i] = -rV;
			else if (dir [i] == 's')
				speed [i] = 0;
		}
		this.count = 0;
	}
	void SetMaxVelocity (float maxVelocity)
	{
		this.maxVelocity = maxVelocity;
		sqrMaxVelocity = maxVelocity * maxVelocity;
	}



	/* This is function to re-position of molecule */
	void changePosition ()
	{
		Vector3 pos = transform.position;
		if (pos.x >= 14.5f) {
			pos.x = -14.5f;
		} else if (pos.x <= -14.5f) {
			pos.x = 14.5f;
		} else if (pos.y >= 12.5f) {
			pos.y = -17;
		} else if (pos.y <= -17) {
			pos.y = 12.5f;
		} else if (pos.z >= 15) {
			pos.z = -15;
		} else if (pos.z <= -15) {
			pos.z = 15;
		}
		rb.MovePosition (pos);
	}

	void vdwEquation ()
	{
		for (int i = 1; i <= this.numberOfMolecule; i++) {
			string name = "Argon " + i;
			if (name == this.objName) {
				continue;
			}
			// if(this.name == "Argon 1"){
			// 	Debug.Log("1");
			// }
			GameObject temp = GameObject.Find (name);
			//Debug.Log("1");
			/* 
			 * Check Distance between this molecule and another molecule
			 * if distance is lower than maximum distance of molecule, do equation, otherwise, do nothing
			*/
			Vector3 temppos = temp.transform.position;
			Vector3 pos = transform.position;
			setTempPos ();
			float distance = Mathf.Sqrt (Mathf.Pow (temppos.x - pos.x, 2) + Mathf.Pow (temppos.y - pos.y, 2) + Mathf.Pow (temppos.z - pos.z, 2)); //distance between this molecule and another molecule
			float distance2 = Mathf.Sqrt (Mathf.Pow (temppos.x - tempObjPos.x, 2) + Mathf.Pow (temppos.y - tempObjPos.y, 2) + Mathf.Pow (temppos.z - tempObjPos.z, 2)); //distance between this molecule and another molecule 
			if (distance <= maxDistance) {
				this._fromObj [i] = true;
				//Debug.Log ("Name = " + name);
				//Debug.Log ("Temppos = " + temppos.ToString ("F10") + " pos = " + pos.ToString ("F10"));
				//Debug.Log ("Distance = " + distance.ToString("F10"));
				Vector3 irij = temppos - pos;
				float evdw = this.epsilon * (Mathf.Pow ((this.sigma / distance), 12) - Mathf.Pow ((this.sigma / distance), 6)); // Energy VDW
																																//Debug.Log ("Energy VDW = " + evdw.ToString ("F10"));
				float equation = ((12 * this.epsilon) / Mathf.Pow (distance, 2)) * (Mathf.Pow ((this.sigma / distance), 12) - ((1.0f / 2.0f) * Mathf.Pow ((this.sigma / distance), 6)));
				//Debug.Log ("Equation =" + equation);
				Vector3 force = equation * irij;
				//Debug.Log ("Force = " + force);
				//temp.GetComponent<Rigidbody> ().AddForce (force);
				//temp.GetComponent<MoleculeController> ().AddObjForce (force);
				this.rb.AddForce (-force);
				this.DelObjForce (forceFromObj [i]);
				forceFromObj [i] = -force;
				this.AddObjForce (forceFromObj [i]);
				//Debug.Log ("Name = " + name);
				//if (this.objName == "Copy Molecule (1)") {
				//	Debug.Log ("Force = " + force + " From " + name);
				//	Debug.Log ("Total Force = " + this.objForce);
				//	Debug.Log ("True Count = " + this.trueCount ());
				//}


			} else if (distance2 <= maxDistance) {
				
				this._fromObj [i] = true;
				//Debug.Log ("Name = " + name);
				//Debug.Log ("Temppos = " + temppos.ToString ("F10") + " pos = " + pos.ToString ("F10"));
				//Debug.Log ("Distance = " + distance2.ToString("F10"));
				Vector3 irij = temppos - tempObjPos;
				float evdw = this.epsilon * (Mathf.Pow ((this.sigma / distance2), 12) - Mathf.Pow ((this.sigma / distance2), 6)); // Energy VDW
																																  //Debug.Log ("Energy VDW = " + evdw.ToString ("F10"));
				float equation = ((12 * this.epsilon) / Mathf.Pow (distance2, 2)) * (Mathf.Pow ((this.sigma / distance2), 12) - ((1.0f / 2.0f) * Mathf.Pow ((this.sigma / distance2), 6)));
				//Debug.Log ("Equation =" + equation);
				Vector3 force = equation * irij;
				//Debug.Log ("Force = " + force);
				//temp.GetComponent<Rigidbody> ().AddForce (force);
				//temp.GetComponent<MoleculeController> ().AddObjForce (force);
				this.rb.AddForce (-force);
				this.DelObjForce (forceFromObj [i]);
				forceFromObj [i] = -force;
				this.AddObjForce (forceFromObj [i]);
				//Debug.Log ("Name = " + name);
				//if (this.objName == "Copy Molecule (1)") {
				//	Debug.Log ("Force = " + forceFromObj [i] + " From " + name);
				//	Debug.Log ("Total Force = " + this.objForce);
				//	Debug.Log ("True Count = " + this.trueCount ());
				//}

			}
			else{
				if (_fromObj [i] == true) {
					this.DelObjForce (forceFromObj [i]);
					//if (this.objName == "Copy Molecule (1)")
					//	Debug.Log ("RemoveForce =" + forceFromObj [i] + "From " + name);
					forceFromObj [i] = new Vector3 (0, 0, 0);
					_fromObj [i] = false;
					//if (this.objName == "Copy Molecule (1)") {
					//	Debug.Log ("2");
					//	Debug.Log ("Total Force = " + this.objForce);
					//	Debug.Log ("True Count = " + this.trueCount ());
					//}
				}
			}



		}
		//if (this.objName == "Copy Molecule (1)")
		//	Debug.Log ("Real Total Force = " + this.objForce);
	}
	
	void OnMouseDown ()
	{
		clickOn = true;
	}

	int trueCount ()
	{
		int count = 0;
		for (int i = 1; i < this.numberOfMolecule + 1; i++) {
			if (_fromObj [i] == true) {
				count++;
			}
		}
		return count;
	}
	void DelObjForce (Vector3 force)
	{
		this.objForce -= force;
	}
	void AddObjForce (Vector3 force)
	{
		this.objForce += force;
	}
	public Vector3 GetVelocity ()
	{
		return this.rb.velocity;
	}

	void setXPos ()
	{
		if (tempObjPos.x == Mathf.Abs (tempObjPos.x)) {
			tempObjPos.x -= 30;
		} else {
			tempObjPos.x += 30;
		}
	}
	void setYPos ()
	{
		if (tempObjPos.y == Mathf.Abs (tempObjPos.y)) {
			tempObjPos.y -= 20;
		} else {
			tempObjPos.x += 20;
		}
	}
	void setZPos ()
	{
		if (tempObjPos.z == Mathf.Abs (tempObjPos.z)) {
			tempObjPos.z -= 30;
		} else {
			tempObjPos.z += 30;
		}
	}
	void setTempPos ()
	{
		this.tempObjPos = transform.position;
		setXPos ();
		setYPos ();
		setZPos ();
	}


	/* Initialize molecule */
	void Start ()
	{
		this.objName = this.gameObject.name;
		this.numberOfMolecule = 40;
		rb = GetComponent<Rigidbody> ();
		randomPosition (); // Random position of Molecule
		realV = 5.0f;
		velocity = realV/5.0f;


		this.forceFromObj = new Vector3 [this.numberOfMolecule+1];
		this._fromObj = new bool [this.numberOfMolecule + 1];
		speed = new float [3];
		dir = new char [3];

		//random direction of vector3
		bool check = false;
		while (check == false) {
			this.count = 0;
			setDirection ();
			check = checkDirections ();
		}

		//end of random direction of vector3
		// Set speed of each direction
		setSpeed ();
		SetMaxVelocity (100);
		movement = new Vector3 (speed [0], speed [1], speed [2]);
		rb.velocity = movement;
		this.maxDistance = 10.0f;
		this.epsilon = 1.0f;
		this.sigma = 1.0f;
		this.objForce = new Vector3 ();
		this.relation = new List<GameObject>();
		this.moleculeName = "Argon";
		this.moleculeMass = 6.6f * Mathf.Pow (10, -26);
		this.clickOn = false;
	}
	void checkOnClick(){
		if(clickOn){
			this.GetComponent<Renderer>().material.color = Color.red;
		}
		else if(!clickOn){
			this.GetComponent<Renderer>().material.color = Color.blue;
		}
	}
	public void changeClickOn(){
		this.clickOn = false;
	}

	void FixedUpdate ()
	{
		setTempPos ();
		vdwEquation ();
		changePosition ();
		if (rb.velocity.sqrMagnitude > sqrMaxVelocity) {
			rb.velocity = rb.velocity.normalized * maxVelocity;
		}
		moleculeSpeed = this.rb.velocity.magnitude;
		checkOnClick();
		// if (this.objName == "Argon 1") {
		// 	float peak = 0;
		// 	int number = 0;
		// 	for (int i = 1; i <= 20; i++) {
		// 		float func = Mathf.Pow (moleculeMass / (2 * Mathf.PI * (1.38065f * Mathf.Pow (10, -23)) * GameController.globalTemperature), (3.0f / 2.0f));
		// 		float func2 = 4 * Mathf.PI * i * Mathf.Pow (2.71828f, -(moleculeMass * Mathf.Pow (i, 2)) / (2 * Mathf.PI * (1.38065f * Mathf.Pow (10, -23)) * GameController.globalTemperature));
		// 		float final = func * func2;
		// 		if (final > peak) {
		// 			peak = final;
		// 			number = i;
		// 		}
		// 	}
		// 	//Debug.Log ("Final From " + number +" = " + peak);

		// 	float funcc = Mathf.Pow (moleculeMass / (2 * Mathf.PI * (1.38065f * Mathf.Pow (10, -23)) * GameController.globalTemperature), (3.0f / 2.0f));
		// 	float func22 = 4 * Mathf.PI * this.velocity * Mathf.Pow (2.71828f, -(moleculeMass * Mathf.Pow (this.velocity, 2)) / (2 * Mathf.PI * (1.38065f * Mathf.Pow (10, -23)) * GameController.globalTemperature));
		// 	float finall = funcc * func22;
		// 	//Debug.Log ("This Object Function = " + finall);
		// }
	}
}

