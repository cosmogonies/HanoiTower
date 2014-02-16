using UnityEngine;
using System.Collections;

public class StaffBehaviour : MonoBehaviour {


	private Mesh CurrentMesh;
	
	// Use this for initialization
	void Start () 
	{	
		CurrentMesh = GetComponent<MeshFilter>().mesh as Mesh;
	}
	
	
	// Update is called once per frame
	void Update ()
	{	
		Vector2[] UVBuffer = new Vector2[CurrentMesh.uv.Length];
		UVBuffer = CurrentMesh.uv;
		
		for(int i=0;i< CurrentMesh.uv.Length;i++)
			UVBuffer[i].y -= Time.deltaTime;
		
		CurrentMesh.uv = UVBuffer;
		
		transform.Rotate(0,Random.Range(-5,5),0); //Removing ?
	}
	
	
}
