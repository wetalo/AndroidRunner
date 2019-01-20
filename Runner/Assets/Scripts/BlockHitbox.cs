using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHitbox : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OntriggerEnter");
        if(other.gameObject.tag == "Player")
        {
            other.SendMessage("Death");
        }
    }
}
