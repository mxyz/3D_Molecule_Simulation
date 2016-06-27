using UnityEngine;
using System.Collections;

public class MoleculeController : MonoBehaviour
{

	// Attributes
	private Rigidbody rb; //force of molecule
	private float [] speed; // x=[0] y=[1] z=[2]
	private char [] dir; //xd=[0] yd=[1] zd=[2]
	private float velocity; //velocity of molecule
	private int count;
	private Vector3 movement;
	private float maxVelocity;
	private float sqrMaxVelocity;
	private float tempX;
	private float tempY;
	private float tempZ;

	// Additional Functions


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

	/* Initialize molecule */
	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		randomPosition (); // Random position of Molecule
		velocity = 5.0f;

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
		SetMaxVelocity (5.0f);
		movement = new Vector3 (speed [0], speed [1], speed [2]);
		rb.velocity = movement;
	}

	/* Function for collision */
	void OnCollisionEnter (Collision col)
	{
		/*
		//Crash walls
		if (col.gameObject.tag == "Wall") {
			if (col.gameObject.name == "Ground" || col.gameObject.name == "Roof") {
				//speed [1] = -speed[1];
				movement.y = -movement.y;
			}
			if (col.gameObject.name == "Left" || col.gameObject.name == "Right") {
				//speed [0] = -speed [0];
				movement.x = -movement.x;
			}
			if ((col.gameObject.name == "Front" || col.gameObject.name == "Back")) {
				//speed [2] = -speed [2];
				movement.z = -movement.z;
			}
		}
		*/
		//Crash another molecule
		if (col.gameObject.tag == "Molecule") {
			Debug.Log ("Crash with ball");
			/* prepare value for equation */
			Vector3 v1 = rb.velocity;
			Vector3 v2 = col.rigidbody.velocity;
			Vector3 r1 = transform.position; // position of this molecule
			Vector3 r2 = col.rigidbody.position; // position of another molecule
			/* Equation of this molecule */
			//find unit vector; unit vector(e) = r2-r1/|r2-r1|
			Vector3 r3 = r2 - r1; // r3 is the value of r2 - r1
			float range_r3 = Mathf.Sqrt (Mathf.Pow (r2.x - r1.x, 2) + Mathf.Pow (r2.y - r1.y, 2) + Mathf.Pow (r2.z - r1.z, 2)); // this is |r2-r1|
			Vector3 e = r3 / range_r3; // r2-r1/|r2-r1|
			// Equation : v1,0 = (e (dot product) v1) * e
			float dotProduct = Vector3.Dot (e, v1); // e (dot product) v1 = float
			Vector3 v10 = dotProduct * e; // dot product * e
			// Equation : v1,2 = v1 - v1,00
			Vector3 v12 = v1 - v10;
			/* End of this molecule */

			/* Equation of other molecule (col) */
			//find unit vector of col molecule
			Vector3 r4 = r1 - r2; // r4 is value of r1 - r2
			float range_r4 = Mathf.Sqrt (Mathf.Pow (r1.x - r2.x, 2) + Mathf.Pow (r1.y - r2.y, 2) + Mathf.Pow (r1.z - r2.z, 2)); // this is |r1-r2|
			Vector3 e2 = r4 / range_r4; //e2 = r1-r2/|r1-r2|
			// Equation : v2,0 = (e (dot product) v1) * e 
			float dotProduct2 = Vector3.Dot (e, v2); // e (dot product) v1 = floata
			Vector3 v20 = dotProduct2 * e2; // dot product * e
											// Equation : v1,2 = v1 - v1,00
			Vector3 v22 = v2 - v20; 
			/* End of other molecule (col) */
			Vector3 newV1 = v12 + v20;
			rb.velocity = newV1;

			/* Debug Equation */
			//Debug.Log ("v1 = " + v1.ToString("F10"))
			//Debug.Log ("r1 = " + r1.ToString("F10") + " |r2 = " + r2.ToString("F10")); // print position of this ball and another ball;
			//Debug.Log ("r3 = " + r3.ToString("F10")); // print r33
			//Debug.Log ("e =" + e.ToString("F10")); // print unit vectorr
			//Debug.Log ("Dot Product = " + dotProduct); // print dot productt
			//Debug.Log ("v1,0 = " + v10.ToString("F10")); // print v1,00
			Debug.Log ("v1,2 = " + v12.ToString("F10")); // print v1,22
			Debug.Log ("v2,0 = " + v20.ToString ("F10")); // print v2,0
			Debug.Log ("newV1 = " + newV1.ToString("F10")); // print new V
		}

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


	void Update ()
	{
		changePosition ();
		if (rb.velocity.sqrMagnitude > sqrMaxVelocity) {
			rb.velocity = rb.velocity.normalized * maxVelocity;
		}
		//rb.AddForce (movement);
	}
}

