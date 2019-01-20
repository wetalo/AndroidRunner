using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb {

    public string name;
    public float bombTimer = 0f;
    public float primeTime = 5f;
    bool isPrimed = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isPrimed)
        {
            bombTimer -= Time.deltaTime;
        }
	}

    public void Prime()
    {
        isPrimed = true;
        bombTimer = primeTime;
    }
}
