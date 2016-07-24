using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PassValue : MonoBehaviour {

	public InputField nomField, mdField, epField, sigField;
	public string nom, md, ep, sigma;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void getValue () {
		nom = nomField.text.ToString();
		md = mdField.text.ToString();
		ep = epField.text.ToString();
		sigma = sigField.text.ToString();

	}
	void Awake() {
        DontDestroyOnLoad(this);
    }
}
