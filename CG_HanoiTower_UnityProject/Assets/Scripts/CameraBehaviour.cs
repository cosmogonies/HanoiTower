using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {


	public GameObject myInterest;
	
	
	//private float angleMax=30.0f;
	//private bool increasing=true;
	//private float currentAngle=0.0f;
	
	// Use this for initialization
	void Start () 
	{	
	
	}
	
	void Update ()
	{	
		
		transform.LookAt(myInterest.transform.position);
		
		/*
		transform.transform.RotateAround(myInterest.transform.position,myInterest.transform.up,currentAngle);
		
		
		if(currentAngle>=angleMax)
		{
			increasing=false;
			
		}
		
		if(increasing)
			currentAngle+=0.1f;
		else
			currentAngle-=0.1f;
		*/
		
	}

	
}
