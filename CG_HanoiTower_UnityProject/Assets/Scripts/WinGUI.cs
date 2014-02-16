using UnityEngine;
using System;
using System.Collections;

public class WinGUI : MonoBehaviour {
	
	public GUISkin MainGUISkin;

	private int language;	
	
	private int moves;
	
	private int Difficulty;
	
	private int BestScore;
	
	private bool BestMoveEver=false;
	
	// Use this for initialization
	void Start () 
	{
		language = PlayerPrefs.GetInt("LastLanguageChoosen");
		moves = PlayerPrefs.GetInt("CurrentScore");
		
		Difficulty = PlayerPrefs.GetInt("LastDifficultyChoosen");
		switch (Difficulty)
		{	
			case 0 :  
				BestScore=PlayerPrefs.GetInt("BestScore5");	
				//Debug.Log("YOUR BEST IS  = "+BestScore);
				break;
			case 1 :  
				BestScore=PlayerPrefs.GetInt("BestScore6");	
				break;
			case 2 :  
				BestScore=PlayerPrefs.GetInt("BestScore7");	
				break;
			case 3 :  
				BestScore=PlayerPrefs.GetInt("BestScore8");	
				break;
			case 4 :  
				BestScore=PlayerPrefs.GetInt("BestScore9");	
				break;
			case 5 :  
				BestScore=PlayerPrefs.GetInt("BestScore10");	
				break;
			
			default:
				break;
		}
		
		if( (moves<BestScore) || (BestScore==0) )
		{
			BestMoveEver=true;
			
			switch (Difficulty)
			{	
				case 0 :  
					PlayerPrefs.SetInt("BestScore5",moves);	
					//Debug.Log("WRITING BESTSCORE5 With Value = "+moves);
					break;
				case 1 :  
					PlayerPrefs.SetInt("BestScore6",moves);	
					break;
				case 2 :  
					PlayerPrefs.SetInt("BestScore7",moves);	
					break;
				case 3 :  
					PlayerPrefs.SetInt("BestScore8",moves);	
					break;
				case 4 :  
					PlayerPrefs.SetInt("BestScore9",moves);	
					break;
				case 5 :  
					PlayerPrefs.SetInt("BestScore10",moves);	
					break;
				
				
				
				default:
					break;
			}

		}
	}
	
	// Update is called once per frame
	void OnGUI ()
	{
		
		GUI.skin = MainGUISkin;
		
		string theText="";;
		string ReturnButtonLabel="";
		
		string theText_ENG ="Congratulations ! You Made it within "+moves+" moves.\n";
		if(BestMoveEver)
			theText_ENG +="And it is your BEST score for current difficulty at "+(Difficulty+5)+" discs !\n";
		theText_ENG +="For your information, the best you can do is "+ (Mathf.Pow(2,Difficulty+5)-1) +" moves !\n";
		string ReturnButtonLabel_ENG="Return to Main Menu";
		
		string theText_FR ="Felicitations ! Vous avez reussi en "+moves+" mouvements.\n";
		if(BestMoveEver)
			theText_FR +="Et c'est votre meilleur score avec "+(Difficulty+5)+" disques !\n";
		theText_FR +="Pour votre information, le meilleur score possible est de "+ (Mathf.Pow(2,Difficulty+5)-1) +" mouvements !\n";
		string ReturnButtonLabel_FR="Retour au menu";
		
		string theText_SPA ="Felicitaciones! Usted tuvo exito en "+moves+" movimientos.\n";
		if(BestMoveEver)
			theText_SPA +="Y es su mejor tanteo con "+(Difficulty+5)+" discos !\n";
		theText_SPA +="Para su informacion, el mejor tanteo posible es de "+(Mathf.Pow(2,Difficulty+5)-1)+" movimientos !\n";		
		string ReturnButtonLabel_SPA="Vuelta al menu";
		
		
		theText = theText_ENG;	
		ReturnButtonLabel = ReturnButtonLabel_ENG;
		if(language==1)
		{
			theText = theText_FR;	
			ReturnButtonLabel = ReturnButtonLabel_FR;
		}
		if(language==2)
		{
			theText = theText_SPA;	
			ReturnButtonLabel = ReturnButtonLabel_SPA;
		}
				
		
		
		GUI.Label(new Rect(Screen.width*0.1f,Screen.height*0.1f,Screen.width*0.8f,Screen.height*0.5f),theText);
	
		if(GUI.Button(new Rect(Screen.width*0.2f,Screen.height*0.7f,Screen.width*0.6f,Screen.height*0.2f),ReturnButtonLabel))
		{
			Application.LoadLevel("MainMenu_01");
		}
	}
	
}
