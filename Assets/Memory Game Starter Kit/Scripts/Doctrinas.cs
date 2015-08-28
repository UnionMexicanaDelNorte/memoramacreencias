using UnityEngine;
using System.Collections;

public class Doctrinas : MonoBehaviour {
	
	bool isInGame = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
			RaycastHit hit;
			
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) { 
				if (hit.transform.name == this.gameObject.name){
					Application.LoadLevel("game"); 
					isInGame = true;
					

				}
			}
		}
	
	}
}
