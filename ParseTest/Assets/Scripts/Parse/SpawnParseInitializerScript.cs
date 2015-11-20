using UnityEngine;
using System.Collections;

public class SpawnParseInitializerScript : MonoBehaviour {
	public GameObject parseInitializer;
	GameObject tempObj;
	// Use this for initialization
	void Start () 
	{
		tempObj = GameObject.Find("ParseInitializeBehaviour");
		if(tempObj == null)
		{
			Instantiate(parseInitializer);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
