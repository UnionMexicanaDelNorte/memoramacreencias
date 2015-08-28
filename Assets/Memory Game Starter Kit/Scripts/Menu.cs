using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	//public Texture2D card;

	// Use this for initialization
	void Start () {
		/*
		if (Input.GetButtonDown ("Fire1")) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);     
			
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) { 
				if (hit.transform.name.ToUpper ().StartsWith ("CARD")) {
					Debug.Log ("Objeto seleccionado : " + hit.transform.name);
				}
			}
			
		}*/
		
	}

	
	// Update is called once per frame
	void Update () {


			if(Input.GetButtonDown("Fire1"))
			{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) { 
				if (hit.transform.name.StartsWith("Diapositiva4")){
					Application.LoadLevel("game3"); 


				Debug.Log("estoy aqui");
					//Destroy(this.gameObject);
				}
				if (hit.transform.name.StartsWith("Diapositiva5")){
					//Application.LoadLevel("game3"); 
					
					Debug.Log("Opciones de memorama");
					//Destroy(this.gameObject);
				}
				if (hit.transform.name.StartsWith("Diapositiva6")){
					//Application.LoadLevel("game3"); 
					
					Debug.Log("Informacion hacerca de esta aplicacion");
					//Destroy(this.gameObject);
				}

			

			}

	
	}

	}
}
