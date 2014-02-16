using UnityEngine;
using System.Collections;

public class SelfLookAt : MonoBehaviour {


	public GameObject target;
	private Transform me;
	
	// Use this for initialization
	void Start () 
	{	
		me = transform;
	}
	
	
	// Update is called once per frame
	void Update ()
	{	
		
		me.LookAt(target.transform.position);
	}
	
	
}
