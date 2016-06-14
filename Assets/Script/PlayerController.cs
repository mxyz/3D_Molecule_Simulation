using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Attributes
	private Rigidbody rb; //force of molecule
	private float[] speed; // x=[0] y=[1] z=[2]
	private char[] dir; //xd=[0] yd=[1] zd=[2]
	private float velocity; //velocity of molecule
	private int count;

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
		Debug.Log ("x" +dir[0]+ "y"+dir[1] + "z"+dir[2]);
		Debug.Log ("Count" + count);
		Debug.Log("rV="+rV+"from"+velocity+"and"+count);
		for( int i = 0 ; i < 3 ; i++ ) {
			if(dir[i] == 'f')
				speed[i] = rV;
			else if(dir[i] == 'b')
				speed[i] = -rV;
			else if(dir[i] == 's')
				speed[i] = 0;
		}
	}

	void Start() {
		rb = GetComponent<Rigidbody> ();
		velocity = 0.8f;

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
	}

	void OnCollisionEnter (Collision col)
	{
		//Crash walls
		if (col.gameObject.tag == "Wall") {
			if (col.gameObject.name == "Ground" || col.gameObject.name == "Roof") {
				speed [1] = -speed [1];
			}
			if (col.gameObject.name == "Left" || col.gameObject.name == "Right") {
				speed [0] = -speed [0];
			}
			if (col.gameObject.name == "Front" || col.gameObject.name == "Back") {
				speed [2] = -speed [2];
			}
		} 
		//Crash others molecule
		else if (col.gameObject.tag == "Molecule") {
			//Physics condition here
			Vector3 temp = rb.velocity;
			speed[0] = temp.x;
			speed[1] = temp.y;
			speed[2] = temp.z;
			Debug.Log("x"+temp.x+" y"+temp.y+" z"+temp.z);
		} else {
			//do nothing
		}


	}

	void FixedUpdate() {
		Vector3 movement = new Vector3 ( speed[0], speed[1], speed[2] );
		rb.AddForce (movement);
	}
}
