using UnityEngine;
using System.Collections;
 
/**
 * Change the camera into an orbital camera.An orbital is a camera
 * That Can Be Rotated and That will automatically Reorient to Itself
 * Always points to the target.
 * 
 * The orbit camera allow zooming and dezooming with the mouse wheel.
 *
 * By clicking close the mouse and dragging on the screen, the camera is Moved.
 * The angle of rotation corresponds to the distance the cursor Travelled. 
 *  
 * The camera will keep the angular position When the button is pressed.to
 * Rotate more, simply repress the mouse button and move the cursor.
 *
 * This script must be added on a camera object.
 *
 * @author Mentalogicus
 * @date 11-2011
 */
public class FlyingCamera : MonoBehaviour
{

	// The target of the camera. The camera will always points to this object.
	public Transform _target;

	// The default distance of the camera from the target.
	public float _distance = 1.0f;

	// Control the speed of zooming and dezooming.
	public float _zoomStep = 1.0f;

	// The speed of the camera. Control how fast the camera will rotate.
	public float _xSpeedd = 1f;
	public float _ySpeedd = 1f;

	// The position of the cursor on the screen. Used to rotate the camera.
	private float _x = 0.0f;
	private float _y = 0.0f;

	// Distance vector. 
	private Vector3 _distanceVector;
  
 /**
  * Move the camera to icts original position.
  */
 	void Start ()
	{
		this._target = this.transform;
		_distanceVector = new Vector3 (0.0f, 0.0f, -_distance);

		Vector2 angles = this.transform.localEulerAngles;
		_x = angles.x;
		_y = angles.y;

		this.Rotate (_x, _y);

	}
 
 /**
  * Rotate the camera or zoom DEPENDING on the input of the player.
  */
 	void LateUpdate ()
	{
		if (_target) {
			this.RotateControls ();
			//this.Zoom ();
		}
	}
  
 /**
  * Rotate the camera When the first button of the mouse is pressed.
  *
  */
 	void RotateControls ()
	{
		if (Input.GetButton ("Fire1")) {
			_x += Input.GetAxis ("Mouse X") * _xSpeedd;
			_y += -Input.GetAxis ("Mouse Y") * _ySpeedd;

			this.Rotate (_x, _y);
		}

	}

	/**
	 * Transform the cursor movement in rotation and in a new position
	 * For the camera.
	 */
	void Rotate (float x, float y)
	{
		// Transform angle in degree in quaternion form used by Unity for rotation.
		Quaternion rotation = Quaternion.Euler (y, x, 0.0f);

		// The new position is the target position + the distance vector of the camera
		// Rotated at the specified angle.
		Vector3 position = (rotation * _distanceVector) + _target.position;

		Debug.Log ("Rotation = " + rotation);
		Debug.Log ("DistanceVector = " + _distanceVector.ToString("F5"));
		Debug.Log ("Target Position = " + _target.position.ToString ("F5"));
		Debug.Log ("Position = " + position.ToString ("F5"));
		// Update the rotation and position of the camera.
		transform.rotation = rotation;
		transform.position = position;
	}
  
 /**
  * Zoom gold dezoom DEPENDING on the input of the mouse wheel.
  */
 	void Zoom ()
	{
		if (Input.GetAxis ("Mouse scrollwheel") < 0.0f) {
			this.ZoomOut ();
		} else if (Input.GetAxis ("Mouse scrollwheel") > 0.0f) {
			this.ZoomIn ();
		}

	}
  
 /**
  * Reduce the distance from the camera to the target and
  * Update the position of the camera (with the Rotate function).
  */
 	void ZoomIn ()
	{
		_distance -= _zoomStep;
		_distanceVector = new Vector3 (0.0f, 0.0f, -_distance);
		this.Rotate (_x, _y);
	}
  
 /**
  * Increase the distance from the camera to the target and
  * Update the position of the camera (with the Rotate function).
  */
 	void ZoomOut ()
	{
		_distance += _zoomStep;
		_distanceVector = new Vector3 (0.0f, 0.0f, -_distance);
		this.Rotate (_x, _y);
	}

} // End class