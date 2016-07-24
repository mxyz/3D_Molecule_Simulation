using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PassValue : MonoBehaviour {

	public InputField nomField, mdField, epField, sigField;
	public string nomS, mdS, epS, sigmaS;
	public int nom;
	public float md, epsilon, sigma;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {
		nomS = nomField.text.ToString();
		if(nomS!="")
		nom = int.Parse(nomS);
		mdS = mdField.text.ToString();
        if(mdS!="")
        md = float.Parse(mdS);
		epS = epField.text.ToString();
        if(epS!="")
        epsilon = float.Parse(epS);
		sigmaS = sigField.text.ToString();
        if(sigmaS!="")
        sigma = float.Parse(sigmaS);

        //Debug.Log(nom);
	}
	void Awake() {
        DontDestroyOnLoad(this);

    }
}
