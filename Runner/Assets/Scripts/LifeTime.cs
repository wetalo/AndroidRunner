using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour {

    public float lifetime;
    float lifetimeCounter = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        lifetimeCounter += Time.deltaTime;
        if (lifetimeCounter > lifetime)
        {
            Destroy(gameObject);
        }
	}
}
