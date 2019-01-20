using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    Bomb bomb;
    [SerializeField]
    GameObject[] models;
    GameObject model;

	// Use this for initialization
	void Start () {
        bomb = new Bomb();
        model = models[Random.Range(0, models.Length)];
       // Instantiate(model, this.transform, false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("AddObject", bomb, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}
