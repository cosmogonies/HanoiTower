using UnityEngine;
using System;
using System.Collections;

using UnityEngine.UI;

public class MainMenu_GUI : MonoBehaviour {
	
	public GUISkin MAinGUISkin;
	
	public GUISkin theProxySkin;
	public GUISkin theProxySkin1;

	private int screenButtonHeight;
	private float[] ButtonLocationY;

	bool isPlaying= false;
	bool isViewingRules= false;
	
	private string[] MenuList_ENG = {"Start Game","Resume LastGame","View Rules","Difficulty","Choose Style","Choose Language","Quit"}	; //TODO : Replace it by a Matrix[3][6]
	private string[] MenuList_FR = {"Demarrer le jeu","Reprendre le jeu","Voir les regles","Difficulte","Choisir le style","Choisir le langage","Quitter"}	;
	private string[] MenuList_SPA = {"Empezar el juego","Continuar el juego","Ver las reglas","Dificultad","Escoger el estilo","Escoger el lenguaje","Cerrar"}	;
	private string[] MenuList;
	
	private string[] DifficultyList = {"5 Discs (very easy)", "6 Discs (easy)", "7 Discs (normal)","8 Discs (hard)","9 Discs (very hard)","10 Discs (extreme)"};
	
	private string[] SkinList = {"Classic", "Zen", "SteamPunk","Chinese","SciFi"};

	private string[] LanguageList = {"English", "French", "Spanish"};
	
	private string RulesHanoi_ENG = "Goal:You must move the disc stack to the right.\n1) Only one disk may be moved at a time.\n2) A disc can only be put on top a disc stack.\n3) You can't put a disc on top of a smaller one.\nThe base floor is considered as a bigger disc.";	
	private string RulesHanoi_FR = "But:Bouger la pile sur l'axe de droite.\n1) Vous pouvez bouger seulement un disque a la fois.\n2) Un disque ne peut etre deplace qu'au sommet d'un pile.\n3) Vous ne pouvez pas poser un disque au dessus d un autre plus petit.\nLe sol est considere comme etant plus grand que chaque disque.";	
	private string RulesHanoi_SPA = "Fin: mover la pila sobre el eje de derecha\n1) Solo un disco puede ser movido en un tiempo \n2) Un disco solo puede ser puesto en la cima. \n3) Usted no puede poner un disco sobre la cima de un mas pequeno.\n Piso de base es considerado como un disco mas grande.";	
	
	public Texture Rules1;	
	public Texture Rules2;
	
	private bool isChoosingDifficulty=false;
	public string ChoosenDifficulty;
	
	//private int LastDifficultyChoosenBuffer=0;
	
	private bool isChoosingSkin=false;
	public string ChoosenSkin;
	
	private bool isChoosingLanguage=false;
	public string ChoosenLanguage;
	
	public int LastPlayedGame;
	
	public GameObject LoadingScreen;
	

	bool isStartingGame=false;
	public GameObject CanvasMain;
	public GameObject CanvasCustomize;


	public GameObject PanelRule;
	public GameObject PanelLanguage;
	//public GameObject[] LanguageButtons;

	public GameObject PanelDifficulty;
	public GameObject PanelStyle;

	//public Color TextColor;
	//public Color SelectedTextColor;




	// Use this for initialization
	void Start () 
	{
		LoadingScreen.SetActive(false);
		
		MenuList = MenuList_ENG;
		UpdatePayersPref();
				
		screenButtonHeight = Screen.height / (MenuList.Length+1);

		//DontDestroyOnLoad(this);  <============================================= Utility of this ?????????????????????????????,
		
		ButtonLocationY = new float[MenuList.Length];
		for(int i=0;i<MenuList.Length;i++)
		{
			ButtonLocationY[i] = i* (Screen.height / (float)MenuList.Length);
		}
         		
	}


	public void OnButton_ViewRules()
	{	//Toggling view Rules visibility.
		if(isChoosingLanguage)
			OnButton_ChoseLanguageMenu();

		isViewingRules = !isViewingRules;

		if(isViewingRules)
			PanelRule.SetActive(true);
		else
			PanelRule.SetActive(false);

	}

	public void OnButton_ChoseLanguageMenu()
	{
		if(isViewingRules)
			OnButton_ViewRules();

		isChoosingLanguage = ! isChoosingLanguage;

		if(isChoosingLanguage)
			PanelLanguage.SetActive(true);
		else
			PanelLanguage.SetActive(false);

	}

	public void OnButton_ChooseLanguage(int _LanguageIndex)
	{
		PlayerPrefs.SetInt("LastLanguageChoosen",_LanguageIndex);

		refreshLanguageButtons(0, _LanguageIndex);
		refreshLanguageButtons(1, _LanguageIndex);
		refreshLanguageButtons(2, _LanguageIndex);

	}
	private void refreshLanguageButtons(int _Index, int  _LanguageIndex)
	{
		//We nee to find and put in italic the current selected language
		bool IsSelected = (_LanguageIndex == _Index);
		/*
		UnityEngine.UI.Text toto = LanguageButtons[_Index].GetComponentInChildren<UnityEngine.UI.Text>() as UnityEngine.UI.Text;


		if(IsSelected)
		{
			toto.fontStyle = FontStyle.BoldAndItalic;
			//toto.color = SelectedTextColor;
		}
		else
		{
			toto.fontStyle = FontStyle.Normal;
			//toto.color = TextColor;
		}
*/
	}


	public void OnToggle_Language(int _LanguageIndex)
	{
		//Debug.Log(UnityEngine.EventSystems.EventSystem.current.name);

		//Debug.Log(UnityEngine.EventSystems.EventSystem.current.gameObject.name);

	
		//TODO: how to know which widgzet trigger the event.

		//Debug.Log(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject());

		//Debug.Log(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

		GameObject EventSource = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

		//UnityEngine.EventSystems.EventSystem.current.

		UnityEngine.UI.Toggle toto = EventSource.GetComponent<Toggle>() as Toggle;

		if( toto.isOn )
		{
			//Debug.Log (_LanguageIndex + "trigerred");
		}

		Toggle[] Toggles = PanelLanguage.GetComponentsInChildren<Toggle>() as Toggle[];
		for (int i = 0; i < Toggles.Length; i++) 
		{
			if( Toggles[i].gameObject ==  UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject)
				if(Toggles[i].isOn)
					if(i==_LanguageIndex)
						localizeGUI(_LanguageIndex);
						//Debug.Log (i + "trigerred");

		}
	}
	public void OnToggle_ChooseDiffculty(int _DifficultyIndex)
	{
		GameObject EventSource = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

		Toggle[] Toggles = PanelDifficulty.GetComponentsInChildren<Toggle>() as Toggle[];
		for (int i = 0; i < Toggles.Length; i++) 
		{
			if( Toggles[i].gameObject ==  EventSource)
				if(Toggles[i].isOn)
					if(i==_DifficultyIndex)
						PlayerPrefs.SetInt("LastDifficultyChoosen",_DifficultyIndex);
		}
	}
	public void OnToggle_ChooseSkin(int _SkinIndex)
	{
		GameObject EventSource = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
		
		Toggle[] Toggles = PanelStyle.GetComponentsInChildren<Toggle>() as Toggle[];
		for (int i = 0; i < Toggles.Length; i++) 
		{
			if( Toggles[i].gameObject ==  EventSource)
				if(Toggles[i].isOn)
					if(i==_SkinIndex)
						PlayerPrefs.SetInt("LastSkinChoosen",_SkinIndex);
		}
	}



	private void localizeGUI(int _LanguageIndex)
	{
		Debug.Log ("localizeGUI for Language = "+_LanguageIndex);
		PlayerPrefs.SetInt("LastLanguageChoosen",_LanguageIndex);
	}



	/*
	public void OnToggle_Language(GameObject _SourceEvent)
	{
		//Debug.Log (_LanguageIndex+"trigerred");

		UnityEngine.UI.Toggle toto = _SourceEvent.GetComponent<Toggle>() as Toggle;

		if( toto.isOn )
		{


			Debug.Log ("trigerred");
		}

		

	}
	*/

	public void OnButton_StartANewGame()
	{
		isStartingGame = ! isStartingGame;
		
		if(isStartingGame)
		{
			CanvasCustomize.SetActive(true);
			CanvasMain.SetActive(false);
		}




	}

	public void OnButton_ResumeGame()
	{
		isPlaying=true;
		Application.LoadLevel("Level_Wood");
	}
	public void OnButton_StartGame()
	{
		//LoadingScreen.SetActive(true);
		
		isPlaying=true;
		//PlayerPrefs.SetInt("LastDifficultyChoosen",LastDifficultyChoosenBuffer);

		PlayerPrefs.SetInt("LastPlayedGame", Mathf.RoundToInt(Mathf.Pow(2,PlayerPrefs.GetInt("LastDifficultyChoosen")+5))-1);

		PlayerPrefs.SetInt("CurrentScore",0);

		Application.LoadLevel("Level_Wood");
	}


	public void OnButton_ChooseDifficultyMenu()
	{
		
		if(isChoosingSkin)
			OnButton_ChooseSkinMenu();
		
		isChoosingDifficulty = ! isChoosingDifficulty;
		
		if(isChoosingDifficulty)
			PanelDifficulty.SetActive(true);
		else
			PanelDifficulty.SetActive(false);
	}

	public void OnButton_ChooseSkinMenu()
	{
		if(isChoosingDifficulty)
			OnButton_ChooseDifficultyMenu();
		
		isChoosingSkin = ! isChoosingSkin;
		
		if(isChoosingSkin)
			PanelStyle.SetActive(true);
		else
			PanelStyle.SetActive(false);
	}






	// Update is called once per frame
	void OnGUI ()
	{




		GUI.skin = MAinGUISkin;
		if(!isPlaying)
		{

			/*
			//Start Game.
			if(GUI.Button(new Rect(Screen.width*0.5f-64,ButtonLocationY[0],200,screenButtonHeight),MenuList[0]))
			{
				LoadingScreen.SetActive(true);
				
				isPlaying=true;
				PlayerPrefs.SetInt("LastDifficultyChoosen",LastDifficultyChoosenBuffer);
				PlayerPrefs.SetInt("LastPlayedGame", Mathf.RoundToInt(Mathf.Pow(2,PlayerPrefs.GetInt("LastDifficultyChoosen")+5))-1);
				PlayerPrefs.SetInt("CurrentScore",0);
				Application.LoadLevel("Level_Wood");
			}
			*/

			/*
			//Resume Game
			if(GUI.Button(new Rect(Screen.width*0.5f-64,ButtonLocationY[1],200,screenButtonHeight),MenuList[1]))
			{
				LoadingScreen.SetActive(true);
				
				//PlayerPrefs.SetInt("LastDifficultyChoosen",LastDifficultyChoosenBuffer);
				isPlaying=true;
				Application.LoadLevel("Engine_02");
			}
			*/
			/*			
			//*** BUTTON Rules. ***
			if(!isViewingRules)
			{
				if(GUI.Button(new Rect(Screen.width*0.5f-64,ButtonLocationY[2],200,screenButtonHeight),MenuList[2]))
				{
					ResetButtons();
					isViewingRules = true;
				}
			}
			else
			{	
				if(GUI.Button(new Rect(Screen.width*0.5f-64,ButtonLocationY[2],200,screenButtonHeight),MenuList[2]))
					isViewingRules=false;
				
				//GUI.DrawTexture(new Rect(20,Screen.height*0.1f,Rules1.width,Rules1.height), Rules1);
				//GUI.DrawTexture(new Rect(20,Screen.height*0.6f,Rules2.width,Rules2.height), Rules2);
				
				GUI.DrawTexture(new Rect(5,Screen.height*0.1f,Screen.width*0.4f,Rules1.height), Rules1);
				GUI.DrawTexture(new Rect(5,Screen.height*0.6f,Screen.width*0.4f,Rules2.height), Rules2);
				
				string RulesHanoi="";
				if(PlayerPrefs.GetInt("LastLanguageChoosen")==0)
					RulesHanoi= RulesHanoi_ENG;
				if(PlayerPrefs.GetInt("LastLanguageChoosen")==1)
					RulesHanoi= RulesHanoi_FR;
				if(PlayerPrefs.GetInt("LastLanguageChoosen")==2)
					RulesHanoi= RulesHanoi_SPA;
				
				GUI.skin = theProxySkin1;
				float startPosX =Screen.width*0.5f+200-64+5;
				GUI.Label(new Rect(startPosX,5.0f,Screen.width-startPosX-5,Screen.height-5.0f),RulesHanoi);
				GUI.skin = MAinGUISkin;
			}
			*/
			
			/*
			//*** BUTTON Difficulty. ***
			if(!isChoosingDifficulty)
			{
				if(GUI.Button(new Rect(Screen.width*0.5f-64,ButtonLocationY[3],200,screenButtonHeight),MenuList[3]))
				{
					ResetButtons();
					isChoosingDifficulty=true;
				}
			}
			else
			{
				if(GUI.Button(new Rect(Screen.width*0.5f-64,ButtonLocationY[3],200,screenButtonHeight),MenuList[3]))
					isChoosingDifficulty=false;
				
				float startPosX =Screen.width*0.5f+200-64+5;
				Rect SubMenuRect = new Rect(startPosX,ButtonLocationY[3],Screen.width-startPosX-5,screenButtonHeight*DifficultyList.Length);
				SubMenuRect.y-= SubMenuRect.height*0.5f; 
				ChoosenDifficulty = SelectList( DifficultyList, ChoosenDifficulty,SubMenuRect,theProxySkin.GetStyle("Button"), theProxySkin1.GetStyle("Button") );
				
				if(ChoosenDifficulty==DifficultyList[0])
					LastDifficultyChoosenBuffer=0;
				if(ChoosenDifficulty==DifficultyList[1])
					LastDifficultyChoosenBuffer=1;
				if(ChoosenDifficulty==DifficultyList[2])
					LastDifficultyChoosenBuffer=2;
				if(ChoosenDifficulty==DifficultyList[3])
					LastDifficultyChoosenBuffer=3;
				if(ChoosenDifficulty==DifficultyList[4])
					LastDifficultyChoosenBuffer=4;
				if(ChoosenDifficulty==DifficultyList[5])
					LastDifficultyChoosenBuffer=5;
				
				
			}
			*/
		
			/*
			//*** BUTTON Style. ***
			if(!isChoosingSkin)
			{
				if(GUI.Button(new Rect(Screen.width*0.5f-64,ButtonLocationY[4],200,screenButtonHeight),MenuList[4]))
				{
					ResetButtons();
					isChoosingSkin=true;
				}
			}
			else
			{
				if(GUI.Button(new Rect(Screen.width*0.5f-64,ButtonLocationY[4],200,screenButtonHeight),MenuList[4]))
					isChoosingSkin=false;
				
				float startPosX =Screen.width*0.5f+200-64+5;
				Rect SubMenuRect = new Rect(startPosX,ButtonLocationY[4],Screen.width-startPosX-5,screenButtonHeight*SkinList.Length);
				SubMenuRect.y-= SubMenuRect.height*0.5f; 
				ChoosenSkin = SelectList( SkinList, ChoosenSkin,SubMenuRect,theProxySkin.GetStyle("Button"), theProxySkin1.GetStyle("Button") );
				
				if(ChoosenSkin==SkinList[0])
					PlayerPrefs.SetInt("LastSkinChoosen",0);
				if(ChoosenSkin==SkinList[1])
					PlayerPrefs.SetInt("LastSkinChoosen",1);
				if(ChoosenSkin==SkinList[2])
					PlayerPrefs.SetInt("LastSkinChoosen",2);	
				if(ChoosenSkin==SkinList[3])
					PlayerPrefs.SetInt("LastSkinChoosen",3);
				if(ChoosenSkin==SkinList[4])
					PlayerPrefs.SetInt("LastSkinChoosen",4);
			}
*/
			/*
			//*** BUTTON Language ***
			if(!isChoosingLanguage)
			{
				if(GUI.Button(new Rect(Screen.width*0.5f-64,ButtonLocationY[5],200,screenButtonHeight),MenuList[5]))
				{
					ResetButtons();
					isChoosingLanguage=true;
				}
			}
			else
			{
				if(GUI.Button(new Rect(Screen.width*0.5f-64,ButtonLocationY[5],200,screenButtonHeight),MenuList[5]))
					isChoosingLanguage=false;
				
				float startPosX =Screen.width*0.5f+200-64+5;
				Rect SubMenuRect = new Rect(startPosX,ButtonLocationY[5],Screen.width-startPosX-5,screenButtonHeight*LanguageList.Length);
				SubMenuRect.y-= SubMenuRect.height*0.5f;
				ChoosenLanguage = SelectList( LanguageList, ChoosenLanguage,SubMenuRect,theProxySkin.GetStyle("Button"), theProxySkin1.GetStyle("Button") );
				
				for(int i=0;i<LanguageList.Length;i++)
				{
					if(ChoosenLanguage==LanguageList[i])
					{
						PlayerPrefs.SetInt("LastLanguageChoosen",i);
						
						if(i==0)
							MenuList = MenuList_ENG;
						if(i==1)
							MenuList = MenuList_FR;
						if(i==2)
							MenuList = MenuList_SPA;
					}
				}
			}
			*/
			/*
			//*** BUTTON Quit ***
			if(GUI.Button(new Rect(Screen.width*0.5f-64,ButtonLocationY[6],200,screenButtonHeight),MenuList[6])) //TODO: Make it smoother
			{			
				Application.Quit();
			}
			*/
			
//			if(PlayerPrefs.HasKey("DEBUG"))
//			{
//				string buffer="";
//				//buffer += "\nDEBUG="+PlayerPrefs.GetString("DEBUG");
//				buffer += "\nLastSkinChoosen="+PlayerPrefs.GetInt("LastSkinChoosen");
//				buffer += "\nLastDifficultyChoosen="+PlayerPrefs.GetInt("LastDifficultyChoosen");
//				buffer += "\nSkinUnlocked="+PlayerPrefs.GetInt("SkinUnlocked");
//				buffer += "\nLastLanguageChoosen="+PlayerPrefs.GetInt("LastLanguageChoosen");
//				buffer += "\nLastPlayedGame="+PlayerPrefs.GetInt("LastPlayedGame");	
//				//buffer += "\nBestScore="+PlayerPrefs.GetInt("BestScore");	
//				buffer += "\nCurrentScore="+PlayerPrefs.GetInt("CurrentScore");	
//				GUI.TextArea(new Rect (0,0,200,200),buffer);
//			}
		}
	}
	
	
	private void ResetButtons()
	{
		isViewingRules=false;
		isChoosingDifficulty=false;
		isChoosingSkin=false;
		isChoosingLanguage=false;
	}
	
	#region SelectList_AddOn
	//http://www.unifycommunity.com/wiki/index.php?title=SelectList
	
	public string SelectList( ICollection list, string selected, Rect thePosition ,GUIStyle defaultStyle, GUIStyle selectedStyle )
        {       
			int i=0;
			float ButtonHeight = thePosition.height / list.Count;
		
			//GUILayout.BeginArea(thePosition);
            foreach( string item in list )
            {
				//if( GUILayout.Button( item.ToString(), ( selected == item ) ? selectedStyle : defaultStyle ) )
				if( GUI.Button( new Rect(thePosition.xMin,thePosition.yMin+(i*ButtonHeight),thePosition.width,ButtonHeight*0.9f),item.ToString(), ( selected == item ) ? selectedStyle : defaultStyle ) )
                {
                    if( selected == item )
                    {// Clicked an already selected item. Deselect.
                        selected = null;
                    }
                    else
                    {
                        selected = item;
						isChoosingLanguage=false; //GON_ADD
						isChoosingSkin=false; //GON_ADD
						isChoosingDifficulty=false; //GON_ADD
                    }
                }
				i++;
            }
			//GUILayout.EndArea();
            return selected;
        }
        
        
        
    public delegate bool OnListItemGUI( string item, bool selected, ICollection list );
    
    public string SelectList( ICollection list, string selected , OnListItemGUI itemHandler )
    {
        ArrayList itemList;
        
        itemList = new ArrayList( list );
        
        foreach( string item in itemList )
        {
            if( itemHandler( item, item == selected, list ) )
            {
                selected = item;
            }
            else if( selected == item )
            // If we *were* selected, but aren't any more then deselect
            {
                selected = null;
            }
        }
        return selected;
    }
	#endregion
	
	
	
	
	
	public void UpdatePayersPref()
	{
		
		if(! PlayerPrefs.HasKey("SkinUnlocked"))
			PlayerPrefs.SetInt("SkinUnlocked",0);
		
		
		if(! PlayerPrefs.HasKey("LastDifficultyChoosen"))
		{
			PlayerPrefs.SetInt("LastDifficultyChoosen",0);
			ChoosenDifficulty = DifficultyList[0];
		}
		else
			ChoosenDifficulty = DifficultyList[PlayerPrefs.GetInt("LastDifficultyChoosen")];
			
			
		if(! PlayerPrefs.HasKey("LastSkinChoosen"))
		{
			PlayerPrefs.SetInt("LastSkinChoosen",0);
			ChoosenSkin = SkinList[0];
		}
		else
			ChoosenSkin = SkinList[PlayerPrefs.GetInt("LastSkinChoosen")];
		
		
		
		if(! PlayerPrefs.HasKey("LastLanguageChoosen"))
		{
			PlayerPrefs.SetInt("LastLanguageChoosen",0);
			ChoosenLanguage = LanguageList[0];
			MenuList = MenuList_ENG;
		}
		else
			ChoosenLanguage = LanguageList[PlayerPrefs.GetInt("LastLanguageChoosen")];
		
		
		if(! PlayerPrefs.HasKey("LastPlayedGame"))
		{
			PlayerPrefs.SetInt("LastPlayedGame",0);
			LastPlayedGame = 0;
		}
		else
			LastPlayedGame = PlayerPrefs.GetInt("LastPlayedGame");	
		
		if(! PlayerPrefs.HasKey("BestScore5"))
		{
			PlayerPrefs.SetInt("BestScore5",0);
		}

		if(! PlayerPrefs.HasKey("BestScore6"))
		{
			PlayerPrefs.SetInt("BestScore6",0);
		}
	
		if(! PlayerPrefs.HasKey("BestScore7"))
		{
			PlayerPrefs.SetInt("BestScore7",0);
		}
		
		if(! PlayerPrefs.HasKey("BestScore8"))
		{
			PlayerPrefs.SetInt("BestScore8",0);
		}			
		if(! PlayerPrefs.HasKey("BestScore9"))
		{
			PlayerPrefs.SetInt("BestScore9",0);
		}			
		if(! PlayerPrefs.HasKey("BestScore10"))
		{
			PlayerPrefs.SetInt("BestScore10",0);
		}			
						
		
		
		
		
		if(! PlayerPrefs.HasKey("CurrentScore"))
		{
			PlayerPrefs.SetInt("CurrentScore",0);
		}	
	}
}
