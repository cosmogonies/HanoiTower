using UnityEngine;
using System.Collections;

public class SlideUV : MonoBehaviour {
	
	public eSlideDirection m_SlideDirection;
	public float m_Speed=0.01f;
	public bool m_Reverse;
	
	
	public enum eSlideDirection
	{
		kHorizontal,
		kVertical
	}
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector2[] uvs = new Vector2[mesh.uv.Length];
		int i = 0;
		while (i < uvs.Length) 
		{
			uvs[i]=mesh.uv[i];
			
			if(m_SlideDirection==eSlideDirection.kHorizontal)
			{
				if(m_Reverse)
					uvs[i].x -=m_Speed*Time.deltaTime; 
				else
					uvs[i].x +=m_Speed*Time.deltaTime; 
			}
			else
			{
				if(m_Reverse)
					uvs[i].y -=m_Speed*Time.deltaTime; 
				else
					uvs[i].y +=m_Speed*Time.deltaTime; 
			}
			i++;
		}
		mesh.uv = uvs;
	
	}
}
