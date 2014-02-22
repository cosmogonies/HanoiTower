using UnityEngine;
using System.Collections;

public class Main_GUI : MonoBehaviour {

	public GUISkin theProxySkin;

	public int nbMoves;
	
//	private int screenButtonHeight = 32;
	
	public int currentlySelected = -1;
	
	private DiscManager myDM ;
	
	private Rect TouchZoneLeft;
	private Rect TouchZoneMiddle;
	private Rect TouchZoneRight;
		
	public GameObject[] ClassicObjectsLibrary; //In order: Base, Stick, Disc, Env
	public GameObject[] ZenObjectsLibrary;
	public GameObject[] SteamObjectsLibrary;
	public GameObject[] ChineseObjectsLibrary;
	public GameObject[] SciFiObjectsLibrary;
	
	private float MAGIC_SeparateFactor = 600.0f;
	
	public GameObject myTest;
	
	public Texture Text_Exit;
	public Texture Text_Up ;
	public Texture Text_Down;
	
	public Font FontForLatinNumbers;
	public Material FontMat;
	
	private int bestScoreForThatDifficulty;
	
	public bool isTouching = false;
	public bool isPushing = false;
	//int PushingBuffer=0;
	
	
	private const bool TOUCH_ONLY=false;
	
	//private string[] Symbols = {"I","II","III","IV","V","VI","VII","VIII"};
	private string[] Symbols = {"I","II","III","VI","V","IV","IIV","IIIV","XI","X"};
	
	// Use this for initialization
	void Start () 
	{
		myDM = GetComponent(typeof(DiscManager)) as DiscManager;

		TouchZoneLeft 	= new Rect(0,Text_Up.height,Screen.width*0.33f,Screen.height-Text_Up.height);
		TouchZoneMiddle = new Rect(Screen.width*0.33f,Text_Up.height,Screen.width*0.33f,Screen.height-Text_Up.height);
		TouchZoneRight = new Rect(Screen.width*0.66f,Text_Up.height,Screen.width*0.34f,Screen.height-Text_Up.height);
		
		nbMoves = PlayerPrefs.GetInt("CurrentScore");
		
		int myStyle = PlayerPrefs.GetInt("LastSkinChoosen");
		CreateSet(myStyle);
		
		myDM.Initialize();
		
		switch (PlayerPrefs.GetInt("LastDifficultyChoosen"))
		{	
			case 0 :  
				bestScoreForThatDifficulty =PlayerPrefs.GetInt("BestScore5");	
				break;
			case 1 :  
				bestScoreForThatDifficulty=PlayerPrefs.GetInt("BestScore6");	
				break;
			case 2 :  
				bestScoreForThatDifficulty=PlayerPrefs.GetInt("BestScore7");	
				break;
			case 3 :  
				bestScoreForThatDifficulty=PlayerPrefs.GetInt("BestScore8");	
				break;
			case 4 :  
				bestScoreForThatDifficulty=PlayerPrefs.GetInt("BestScore9");	
				break;
			case 5 :  
				bestScoreForThatDifficulty=PlayerPrefs.GetInt("BestScore10");	
				break;
			default:
				break;
		}	
	}

	void CreateSet(int _style)
	{
		GameObject[] theLibrary = ClassicObjectsLibrary;
		
		if(_style==0)
			theLibrary = ClassicObjectsLibrary;
		if(_style==1)
			theLibrary = ZenObjectsLibrary;
		if(_style==2)
			theLibrary = SteamObjectsLibrary;		
		if(_style==3)
			theLibrary = ChineseObjectsLibrary;		
		if(_style==4)
			theLibrary = SciFiObjectsLibrary;		
		
		//base
		GameObject myBase = Instantiate(theLibrary[0]) as GameObject;
		myBase.name="myBase";

		
		//Stick
		for(int i=0;i<3;i++)
		{
			GameObject myStick = Instantiate(theLibrary[1]) as GameObject;
			myStick.transform.position = new Vector3(0,myStick.transform.position.y,0);
			myStick.transform.localScale = new Vector3(0.8f,1.0f,0.8f); //Since 10 difficulty discs, we have to thicker the stick !
			myStick.transform.Translate(-MAGIC_SeparateFactor +(i*MAGIC_SeparateFactor),0,0);
			
			//myDM.TheLightList[i].transform.position = new Vector3(myStick.transform.position.x,myStick.transform.position.y+1000.0f, myStick.transform.position.z);
			//myDM.TheLightList[i].transform.position = new Vector3(myStick.transform.position.x,myStick.transform.position.y+1000.0f, 0.0f);
			if(_style ==4)	//ScyFy	
				myStick.AddComponent<StaffBehaviour>();
		}
		
		//Discs
		//float lastDiscHeight = myBase.gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.y;
		int numberOfDiscs = 5+PlayerPrefs.GetInt("LastDifficultyChoosen");
		myDM.TheDiscList = new GameObject[numberOfDiscs];
		for(int i=0;i< numberOfDiscs;i++)
		{
			GameObject myCurrentDisc = Instantiate(theLibrary[2]) as GameObject;
			myCurrentDisc.name = "Disc_"+i;
			//Debug.Log( myCurrentDisc.transform.position.y);
			
			
			float ScaleOffsetY = myCurrentDisc.transform.localScale.y-(i/10.0f);
			ScaleOffsetY = Mathf.Clamp(ScaleOffsetY,0.5f,10.0f);
			//ScaleOffsetY=myCurrentDisc.transform.localScale.y;
			
			myCurrentDisc.transform.localScale = new Vector3(myCurrentDisc.transform.localScale.x-(i/10.0f),ScaleOffsetY,myCurrentDisc.transform.localScale.z-(i/10.0f)); 
			
			//myCurrentDisc.transform.Translate(MAGIC_SeparateFactor,lastDiscHeight,0);
			myCurrentDisc.transform.Translate(MAGIC_SeparateFactor,0,0);
			

			
			if(_style!=3)//Except for Chinese Style
				myCurrentDisc.transform.Rotate(new Vector3(0.0f,Random.Range(-25,25),0.0f));
			
			
			myDM.TheDiscList[i] = myCurrentDisc;
			
			
			//Nunmerotation
			GameObject MyCurrentID = new GameObject("ID");			
			TextMesh toto = MyCurrentID.AddComponent<TextMesh>() as TextMesh;
			//MeshRenderer blop = MyCurrentID.AddComponent<MeshRenderer>() as MeshRenderer;
			MeshRenderer blop = MyCurrentID.GetComponent<MeshRenderer>() as MeshRenderer;
			blop.material = FontMat;
			
			toto.text = Symbols[numberOfDiscs-i-1];
			toto.font = FontForLatinNumbers;
			MyCurrentID.transform.position = myCurrentDisc.transform.position;
			MyCurrentID.transform.Translate(45.0f,0,myCurrentDisc.gameObject.GetComponent<MeshFilter>().mesh.bounds.extents.x-(i*30.0f),Space.World);
			MyCurrentID.transform.localScale *=10.0f;
			MyCurrentID.transform.parent = myCurrentDisc.transform;
			
			SelfLookAt MyComp = MyCurrentID.AddComponent<SelfLookAt>() as SelfLookAt;
			MyComp.target = Camera.main.gameObject;
		}
		
		//EnvMap
		GameObject myEnvGO = Instantiate(theLibrary[3]) as GameObject;
		myEnvGO.name="EnvieDeKiwi";
		
	}
	
	
	void Update()
	{	

		//TACTILE MANAGEMENT *************************************************************************************************
		if(currentlySelected==-1)
		{
			if (Input.touchCount > 0) 
			{
				if(Input.GetTouch(0).tapCount   > 0)
				{					
					if(Input.GetTouch(0).phase == TouchPhase.Began)
					{
						//Debug.Log("TOUCH POSITION = "+Input.GetTouch(0).position.x+" , "+Input.GetTouch(0).position.y);
						
						Vector2 FingerPos = Input.GetTouch(0).position;
	
						if(FingerPos.x < TouchZoneLeft.xMax)
						{
							currentlySelected=myDM.TakeOff(0,true);
						}
						if( (FingerPos.x > TouchZoneLeft.xMax) &&(FingerPos.x < TouchZoneMiddle.xMax) )
						{
							currentlySelected=myDM.TakeOff(1,true);
						}
						if(FingerPos.x > TouchZoneRight.xMin)
						{
							currentlySelected=myDM.TakeOff(2,true);
						}
						//Debug.Log("SELECTION OF ="+currentlySelected);
					}
				}
			}
			else
			{
				isTouching=false;
			}
		}
		else
		{
			if (Input.touchCount > 0) 
			{
				isTouching=true;
				if(Input.GetTouch(0).tapCount   > 0)
				{	
					if(Input.GetTouch(0).phase == TouchPhase.Ended)
					{
						
						Vector2 FingerPos = Input.GetTouch(0).position;
	
						if(FingerPos.x < TouchZoneLeft.xMax) 
						{
							if(myDM.amIAllowed(currentlySelected,0))
							{	
								nbMoves++;
								myDM.MoveDiscToAxis(currentlySelected,0);
								currentlySelected=-1;
							}
						}
						if( (FingerPos.x > TouchZoneLeft.xMax) && (FingerPos.x < TouchZoneMiddle.xMax) )
						{
							if(myDM.amIAllowed(currentlySelected,1))
							{	
								nbMoves++;
								myDM.MoveDiscToAxis(currentlySelected,1);
								currentlySelected=-1;
							}
						}
						if(FingerPos.x > TouchZoneRight.xMin) 
						{
							if(myDM.amIAllowed(currentlySelected,2))
							{	
								nbMoves++;
								myDM.MoveDiscToAxis(currentlySelected,2);
								currentlySelected=-1;
							}
						}
						
					}
					else
					{			
						//if( (Screen.height - Input.GetTouch(0).position.y) > TouchZoneMiddle.yMin)
						//{   
							//Debug.Log("STILL TOUCHING = "+Input.GetTouch(0).position.x+" , "+Input.GetTouch(0).position.y+"   and FYI="+TouchZoneMiddle.yMin);
						Ray myFuckingRay = Camera.mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
						Vector3 myFuckingVector = myFuckingRay.GetPoint(Vector3.Distance(Camera.mainCamera.transform.position,new Vector3(0,0,0)));
						myDM.TheDiscList[currentlySelected].transform.position = myFuckingVector;
						//}
					}
				}
			}	
			else
			{
				isTouching=false;
			}
		}
		
	}
	
	void OnGUI ()
	{
		if(GUI.Button(new Rect(Screen.width-Text_Exit.width,0,Text_Exit.width,Text_Exit.height),Text_Exit))
		{
			Application.LoadLevel("MainMenu_01");
			//Application.Quit();
		}
		
		GUI.Label(new Rect(Screen.width-100,Text_Exit.height,100,36),"Score=\t\t"+nbMoves+"\nBest=\t\t"+bestScoreForThatDifficulty);
		PlayerPrefs.SetInt("CurrentScore",nbMoves);
		
		
		// BUTTON INTERFACE ********************************************************************************************************
	
			
		if(currentlySelected==-1)
		{
			//Nothing is selected yet, so let's make it possible :
			for(int AxeIndex =0; AxeIndex<3;AxeIndex++) //For all the three axes:
			{			
				if(myDM.isAxisEmpty(AxeIndex))
				{	
					//Toggle this comment to switch into touchable-only gameplay or into pc_mouse interaction
					
					GUI.Box(new Rect(50.0f+Screen.width*((float)AxeIndex/3.0f),0,Text_Up.width,Text_Up.height),Text_Up);
					
					if(!TOUCH_ONLY)
					{
						if(GUI.Button(new Rect(50.0f+Screen.width*((float)AxeIndex/3.0f),0,Text_Up.width,Text_Up.height),Text_Up))
						{
							currentlySelected=myDM.TakeOff(AxeIndex,false);
						}
					}
				}
			}
		}
		else //There is a disc selected !
		{
			for(int AxeIndex =0; AxeIndex<3;AxeIndex++) //For all the three axes:
			{
				if(myDM.amIAllowed(currentlySelected,AxeIndex))
				{
					//Toggle this comment to switch into touchable-only gameplay or into pc_mouse interaction					
					
					GUI.Box(new Rect(50.0f+Screen.width*((float)AxeIndex/3.0f),0,Text_Down.width,Text_Down.height),Text_Down);
					
					if(!TOUCH_ONLY)
					{					
						if(GUI.Button(new Rect(50.0f+Screen.width*((float)AxeIndex/3.0f),0,Text_Down.width,Text_Down.height),Text_Down))
						{
							nbMoves++;
							
							myDM.MoveDiscToAxis(currentlySelected,AxeIndex);
							currentlySelected=-1;
						}
					}
				}
			}	
		}
			
		
		//VisibleDEBUG TOUCH ZONE
		//GUI.Box(TouchZoneLeft,"*1*");
		//GUI.Box(TouchZoneMiddle,"*2*");
		//GUI.Box(TouchZoneRight,"*3*");	
	}
}
