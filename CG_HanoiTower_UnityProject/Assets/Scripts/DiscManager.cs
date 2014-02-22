using UnityEngine;
using System.Collections;
using System;

public class DiscManager : MonoBehaviour {

	
//3  I		 I  	  I              bool
//2  I 		 I  	  I              bool
//1  I 		 I  	  I              bool
//0  I 	 	 I  	  I              bool
//============================	
//Axis[0..     1	 ..  2]
	
	
	public GameObject[] TheDiscList;
	
	public GameObject TheSnapPoint;
	
	//public GameObject[] TheLightList;
	
	//Every axis is represented by an array of boolean values: is here, is not here, for each Size of Discs.
	private bool[][] AxisStatusArray;
	
	public int currentlySelected;
	
	private float[] allDiscHeight;
	
	private float MAGIC_SeparateFactor = 600.0f;
	
	
	// Use this for initialization
	void Start () 
	{
		//by convention bool[TheDiscList.Length][0] is Left Axis, bool[TheDiscList.Length]20] is Right Axis
		AxisStatusArray = new bool[][]{ new bool[TheDiscList.Length], new bool[TheDiscList.Length],new bool[TheDiscList.Length]};
		
		for(int i=0;i<TheDiscList.Length;i++)
		{
			AxisStatusArray[0][i]=true;
			AxisStatusArray[1][i]=false;
			AxisStatusArray[2][i]=false;		
		}	
		
		currentlySelected=-1;
		
		//Solver toto = new Solver();
		//MLog();
	} 
	
	public void Initialize()
	{
		
		Start();
		
		

		
		RestoreLastGame();
		
//		allDiscHeight = new float[TheDiscList.Length];
//		for(int i=0;i<TheDiscList.Length;i++)
//		{
//			allDiscHeight[i] = TheDiscList[i].transform.position.y;
//			Debug.Log(allDiscHeight[i]);
//		}
		
		//MLog();
	}
	
	
	int GetDiscAtTop(int _theAxis)
	{
		int LastFoundIndex=-345;
		
		for(int i=0;i<AxisStatusArray[_theAxis].Length;i++)
		{
			if(AxisStatusArray[_theAxis][i]==true)
				LastFoundIndex=i;
		}
		return LastFoundIndex;			
	}
	
	
	public int TakeOff(int _Axis, bool touchMode)
	{
		int myDiscIdx = GetDiscAtTop(_Axis);
		//Debug.Log("Removing MyDiscIndex="+myDiscIdx+ " from Axis="+_Axis);
		
		if(myDiscIdx>=0)
		{
			if(!touchMode)
			{
				GameObject myDisc = TheDiscList[myDiscIdx];
				myDisc.transform.position = TheSnapPoint.transform.position;
			}
		}
		
		AxisStatusArray[_Axis][myDiscIdx]=false;		
		return myDiscIdx;
	}
	
	
	
	public bool amIAllowed(int _myDiscIndex, int _axisIndex)
	{
		//I'm FORBIDDEN to put a disc if there is already a smaller disc :
		for(int i=TheDiscList.Length-1;i>_myDiscIndex;i--)
		{
			if(AxisStatusArray[_axisIndex][i]==true)
			{
				return false;
			}
		}
		return true;
	}
	
	
	public bool isAxisEmpty(int _axisIndex)
	{	
		for(int i=0;i<AxisStatusArray[0].Length;i++)
		{
			if(AxisStatusArray[_axisIndex][i]==true)
				return true;
		}
		return false;	
	}
	
	
	public bool isAxisElligable(int _axisIndex, int _discIndex)
	{	
		for(int i=0;i<_discIndex;i++)
		{
			if(AxisStatusArray[_axisIndex][i]==true)
				return true;
		}
		return false;	
	}
	
	
	public bool MoveDiscToAxis(int _currentSelectedDiscIndex, int _DestinationAxis)
	{
		
		//Debug.Log("Putting Disc No="+_currentSelectedDiscIndex+ " on Axis="+_DestinationAxis);
		
		GameObject myDisc = TheDiscList[_currentSelectedDiscIndex];
		//To Add, just verify if it's at snapPoint .....
		
		//To Find Height, we have to find how many we have "true" value :
		//float PosY=TheDiscList[0].transform.position.y; //WRONG?
		float PosY=80.0f; //MAGIC NUMBER: absolute height of bigger disc
		
		int TrueIterator=0;
		for(int i=0;i<AxisStatusArray[_DestinationAxis].Length;i++)
		{
			if(AxisStatusArray[_DestinationAxis][i]==true)
			{
				TrueIterator++;
				
				PosY= TheDiscList[i].transform.position.y +  myDisc.GetComponent<MeshFilter>().mesh.bounds.extents.y;
			}			
		}
			
		AxisStatusArray[_DestinationAxis][_currentSelectedDiscIndex]=true;
		//PosY = allDiscHeight[TrueIterator];
		
		//Change here: find a good way to put it !
		
		myDisc.transform.position = new Vector3(MAGIC_SeparateFactor-(MAGIC_SeparateFactor*_DestinationAxis),PosY ,0.0f);
		
		UpdateMetaData();
		
		return true;
	}
	

	
	private void UpdateMetaData()
	{
		int toto = MConvertToZeInt();
		//Debug.Log("Storing ... ="+toto);
		PlayerPrefs.SetInt("LastPlayedGame",toto);
		
		//int[]  myInts  = new int[1] { toto };
		//BitArray myBA5 = new BitArray( myInts );
		//Debug.Log("Analysed as ="+  myBA5[0]+myBA5[1]+myBA5[2]+myBA5[3]+myBA5[4]+myBA5[5]+myBA5[6]+myBA5[7]);
		//Debug.Log("Analysed as ="+  myBA5[8]+myBA5[9]+myBA5[10]+myBA5[11]+myBA5[12]+myBA5[13]+myBA5[14]+myBA5[15]);
		//Debug.Log("Analysed as ="+  myBA5[16]+myBA5[17]+myBA5[18]+myBA5[19]+myBA5[20]+myBA5[21]+myBA5[22]+myBA5[23]);
		//Debug.Log("Getting ... ="+PlayerPrefs.GetInt("LastPlayedGame"));
		DidIWon();
	}
	
	public int MConvertToZeInt()
	{
		int NB_MAXIMUM_DISCS=10;
		
		int TheValue=0;
		int j=0;
		foreach(bool[] currentAxis in AxisStatusArray)
		{
			int i=0;
			foreach(bool currentValue in currentAxis)
			{
				if(currentValue)
					TheValue+= Mathf.RoundToInt(Mathf.Pow(2,i+(j*NB_MAXIMUM_DISCS))); 
				i++;
			}
			
			j++;
		}
		return TheValue;
	}

	
	public void RestoreLastGame()
	{
		int theGame = PlayerPrefs.GetInt("LastPlayedGame");
		Debug.Log("Trying to restore ="+theGame);
		
		int[]  myInts  = new int[1] { theGame };
		BitArray myBA5 = new BitArray( myInts );
		//Debug.Log("Analysed as ="+  myBA5[0]+myBA5[1]+myBA5[2]+myBA5[3]+myBA5[4]+myBA5[5]+myBA5[6]+myBA5[7]);
		//Debug.Log("Analysed as ="+  myBA5[8]+myBA5[9]+myBA5[10]+myBA5[11]+myBA5[12]+myBA5[13]+myBA5[14]+myBA5[15]);
		//Debug.Log("Analysed as ="+  myBA5[16]+myBA5[17]+myBA5[18]+myBA5[19]+myBA5[20]+myBA5[21]+myBA5[22]+myBA5[23]);
		
		//MLog();
		
		int NB_MAXIMUM_DISCS = 10;
		
		//Updating AxisStatusArray
		for(int j=0; j<AxisStatusArray.Length;j++)
		{
			for(int i=0; i<AxisStatusArray[j].Length;i++)
			{
				AxisStatusArray[j][i] =  myBA5[i+j*NB_MAXIMUM_DISCS];
			}
		}
		
		//MLog();
		
		//Moving Discs at their location :
		float[] lastHeight =new float[]{80.0f,80.0f,80.0f};//Axis Array : Left => Right
		allDiscHeight = new float[TheDiscList.Length];
		
		for(int j=0; j<AxisStatusArray.Length;j++)
		{//Axis iterator : Left => Right
			
			for(int i=0; i<AxisStatusArray[j].Length;i++)
			{//Disc iterator: Down to up
				
				if(AxisStatusArray[j][i])
				{
					TheDiscList[i].transform.position = new Vector3(-600.0f*j+600,lastHeight[j],0);
					
					//allDiscHeight[i] = lastHeight[j];
					//lastHeight[j]+=TheDiscList[i].gameObject.GetComponent<MeshFilter>().mesh.bounds.size.y;
					lastHeight[j]+=TheDiscList[i].gameObject.GetComponent<MeshRenderer>().bounds.size.y;
				}
			}
		}
		
		allDiscHeight = new float[TheDiscList.Length];
		for(int i=0;i<TheDiscList.Length;i++)
		{
			allDiscHeight[i] = TheDiscList[i].transform.position.y;
			//Debug.Log(allDiscHeight[i]);
		}
	}
	
	
	private void DidIWon()
	{
		bool success=true;
		for(int i=0;i<AxisStatusArray[2].Length;i++)
		{
			success &= AxisStatusArray[2][i];
		}
		
		if(success)
			Application.LoadLevel("WinLevel");	
	}
	
	
	
	
	public void MLog()
	{
		Debug.Log("====================STATUS====================");
		string buffer = "AxisLeft__\t=\t";
		for(int i=0;i<TheDiscList.Length;i++)
			buffer = buffer +"\t"+((AxisStatusArray[0][i])?"1":"0");
		Debug.Log(buffer);
		
		buffer = "AxisMiddle\t=\t";
		for(int i=0;i<TheDiscList.Length;i++)
			buffer = buffer +"\t"+((AxisStatusArray[1][i])?"1":"0");
		Debug.Log(buffer);

		buffer = "AxisRight_\t=\t";
		for(int i=0;i<TheDiscList.Length;i++)
			buffer = buffer +"\t"+((AxisStatusArray[2][i])?"1":"0");
		Debug.Log(buffer);
		Debug.Log("TheValue Is ="+MConvertToZeInt());
		Debug.Log("=============================================");	
	}


}
