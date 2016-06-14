#pragma strict

 var rotation : Quaternion;
 var radius = Vector3(100,0,0);
 var currentRotation = 0.0;
 var rotateCamera = 0.0;
 function Update()
 {
     currentRotation += Input.GetAxisRaw("Horizontal")*Time.deltaTime*100;
     rotateCamera = Input.GetAxisRaw("Horizontal");
     rotation.eulerAngles = Vector3(0, currentRotation, 0);
     transform.position = rotation * radius;
     transform.Rotate(0, rotateCamera, 0);
 }
