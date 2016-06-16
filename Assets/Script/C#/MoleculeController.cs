using UnityEngine;
using System.Collections;

public class MoleculeController : MonoBehaviour {

	// Attributes
	private Rigidbody rb; //force of molecule
	private float[] speed; // x=[0] y=[1] z=[2]
	private char[] dir; //xd=[0] yd=[1] zd=[2]
	private float velocity; //velocity of molecule
	private int count;
	private Vector3 movement;

	// Additional Functions

	
	bool checkDirections() {
		// if all dir are 's' or 'null' return -1, otherwise, return 1
		if (dir[0] ==  dir[1] && dir[1] == dir[2])
			return false;
		else
			return true;
	}

	char randomDirection() {
		// Random 0, 1, 2
		//Have 3 State f = Forward, s = Still, b = Backward

		int rnd = Random.Range(0,3);
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

	void setDirection(){
		for (int i = 0; i < 3; i++) {
			dir[i] = randomDirection();
		}
	}
	
	


	void setSpeed(){
		float rV = velocity / count;
		Debug.Log ("x " +dir[0]+ " y " +dir[1]+ " z "+dir[2]);
		for( int i = 0 ; i < 3 ; i++ ) {
			if(dir[i] == 'f')
				speed[i] = rV;
			else if(dir[i] == 'b')
				speed[i] = -rV;
			else if(dir[i] == 's')
				speed[i] = 0;
		}
		this.count = 0;
	}

	void Start() {
		rb = GetComponent<Rigidbody> ();
		velocity = 5.0f;

		speed = new float[3];
		dir = new char[3];
		//random direction
		bool check = false;
		while(check == false) {
			this.count = 0;
			setDirection();
			check = checkDirections();
		}
		// Set speed of each direction
		setSpeed ();

		
		movement = new Vector3 (speed [0], speed [1], speed [2]);
	}


	void OnCollisionEnter (Collision col)
	{

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
			if (col.gameObject.name == "Front" || col.gameObject.name == "Back") {
				//speed [2] = -speed [2];
				movement.z = - movement.z;
			}
		}

		//Crash another molecule
		if (col.gameObject.tag == "Molecule") {
			Debug.Log("Crash with ball");
			Vector3 v1 = rb.velocity;
			Vector3 r1 = transform.position; // position of this molecule
			Vector3 r2 = col.rigidbody.position; // position of another molecule
			Debug.Log ("r1 = "+r1+" |r2 = "+r2); // print position of this ball and another ball
			//find unit vector; unit vector(e) = r2-r1/|r2-r1|
			Vector3 r3 = r2 - r1; // r3 is the value of r2 - r1
			Debug.Log("r3 = "+r3); // print r3
			float range_r3 = Mathf.Sqrt(Mathf.Pow(r2.x-r1.x,2)+Mathf.Pow(r2.y-r1.y,2)+Mathf.Pow(r2.z-r1.z,2)); // this is |r2-r1|
			Vector3 e = r3 / range_r3; // r2-r1/|r2-r1|
			Debug.Log ("e ="+e); // print unit vector

			// Equation : v1,0 = (e (dot product) v1) * e
			float dotProduct = Vector3.Dot(e, v1); // e (dot product) v1 = float
			Debug.Log("Dot Product = "+dotProduct); // print dot product
			Vector3 v10 = dotProduct * e; // dot product * e
			Debug.Log("v1,0 = "+v10); // print v1,0
			// Equation : v1,2 = v1 - v1,0
			Vector3 v12 = v1 - v10;
			Debug.Log("v1,2 = "+v12); // print v1,2
			Vector3 newV1 = -v10 + v12;
			Debug.Log("newV1 = " + newV1); // print new V
			movement = newV1;
		}
	
	}

	void Update() {
		// transform.Translate(vector3* Time.deltaTime);
		rb.AddForce (movement);
	}
}
