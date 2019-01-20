using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    List<Bomb> objectsEquipped;
    public int maxNumObjects;
    public Text itemText;


	// Use this for initialization
	void Start () {
        objectsEquipped = new List<Bomb>();
        itemText.text = "Items: " + objectsEquipped.Count + "/" + maxNumObjects;
    }
	
	// Update is called once per frame
	void Update () {
        if (objectsEquipped.Count > 0)
        {
            DisplaySoonestExplosion();
        }

    }

    public int GetNumObjects()
    {
        return objectsEquipped.Count;
    }

    public void AddObject(Bomb objectToAdd)
    {
        if(objectsEquipped.Count < maxNumObjects)
        {
            objectsEquipped.Add(objectToAdd);
            objectToAdd.Prime();
            itemText.text = "Items: " + objectsEquipped.Count + "/" + maxNumObjects;
        }
    }

    public void RemoveObject(int indexToRemove)
    {
        objectsEquipped.RemoveAt(indexToRemove);
        itemText.text = "Items: " + objectsEquipped.Count + "/" + maxNumObjects;
    }

    void DisplaySoonestExplosion()
    {
       // GameObject.Find("BombTimer").GetComponent<Text>().text = "" + objectsEquipped[0].bombTimer;
    }
}
