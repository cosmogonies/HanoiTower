using UnityEngine;
using System.Collections;


public class Solver
{
	//http://www.cs.cmu.edu/~cburch/survey/recurse/hanoiimpl.html


	//N  I		 I  	  I              int
	//...
	//3  I		 I  	  I              int
	//2  I 		 I  	  I              int
	//1  I 		 I  	  I              int
	//0  I 	 	 I  	  I              int
	//============================	
	//Axis[0..     1	 ..  2]
	private int[][] AxisStatusArray;	//Virtual duplicate of current Hanoi status to simulate solution.

	//Where to put the next Disc
	/*
		FUNCTION MoveTower(disk, source, dest, spare):
		IF disk == 0, THEN:
			move disk from source to dest
		ELSE:
			MoveTower(disk - 1, source, spare, dest)   // Step 1 above
			move disk from source to dest              // Step 2 above
			MoveTower(disk - 1, spare, dest, source)   // Step 3 above
		END IF
	*/

	//Solution
	int AxisIndexFrom;
	int AxisIndexTo;

	int findDiscHeight(int DiscNumber) //Smallest is 0
	{
		int LENGTH = 5;

		for(int i=0;i<LENGTH;i++)
		{
			if(AxisStatusArray[0][i]==DiscNumber)
				return i;
			if(AxisStatusArray[1][i]==DiscNumber)
				return i;
			if(AxisStatusArray[2][i]==DiscNumber)
				return i;
		}
		return -666;
	}
	
	int GetNextAvailableHeight(int _theAxis)
	{
		for(int i=0; i<AxisStatusArray[_theAxis].Length;i++)
		{
			if(AxisStatusArray[_theAxis][i]==-1)
				return i;
		}
		return -1;
	}


	public void MLog()
	{
		int LENGTH = 5;

		Debug.Log("====================STATUS====================");
		string buffer = "AxisLeft__\t=\t";
		for(int i=0;i<LENGTH;i++)
			buffer = buffer +"\t"+((AxisStatusArray[0][i]!=-1)?"1":"0");
		Debug.Log(buffer);
		
		buffer = "AxisMiddle\t=\t";
		for(int i=0;i<LENGTH;i++)
			buffer = buffer +"\t"+((AxisStatusArray[1][i]!=-1)?"1":"0");
		Debug.Log(buffer);
		
		buffer = "AxisRight_\t=\t";
		for(int i=0;i<LENGTH;i++)
			buffer = buffer +"\t"+((AxisStatusArray[2][i]!=-1)?"1":"0");
		Debug.Log(buffer);
		Debug.Log("=============================================");	
	}

	public Solver()
	{
		int LENGTH = 5;
		this.AxisStatusArray = new int[][]{ new int[LENGTH], new int[LENGTH],new int[LENGTH]};

		//Init
		for(int i=0;i<LENGTH;i++)
		{
			AxisStatusArray[0][i]= LENGTH-1-i ;	//O is smallest disc
			AxisStatusArray[1][i]=-1;
			AxisStatusArray[2][i]=-1;		
		}

		move(4, 0, 2, 1 );
	}

	/*
	public Solver(bool[][] _CurrentStatus)
	{
		int LENGTH = 5;
		this.AxisStatusArray = new int[][]{ new int[LENGTH], new int[LENGTH],new int[LENGTH]};
		
		//Init
		for(int i=0;i<LENGTH;i++)
		{
			if( _CurrentStatus[0][i])


		}
		
		move(4, 0, 2, 1 );
	}
	*/






	public void move(int _Disc, int _SourceAxisIndex, int _DestAxisIndex, int _OtherAxisIndex)
	{
		this.MLog();

		if(_Disc == 0)	//Smaller Disc
		{
			//move disk from source to dest
			AxisStatusArray[_SourceAxisIndex][ findDiscHeight(0) ]=-1;
			AxisStatusArray[_DestAxisIndex][ GetNextAvailableHeight(_DestAxisIndex) ]=_Disc;
		}
		else
		{
			//MoveTower(disk - 1, source, spare, dest)   // Move disks 4 and smaller from peg A (source) to peg C (spare), using peg B (dest) as a spare. How do we do this? By recursively using the same procedure. After finishing this, we'll have all the disks smaller than disk 4 on peg C. (Bear with me if this doesn't make sense for the moment - we'll do an example soon.) 
			move(_Disc-1, _SourceAxisIndex, _OtherAxisIndex, _DestAxisIndex);

			//move disk from source to dest              // Now, with all the smaller disks on the spare peg, we can move disk 5 from peg A (source) to peg B (dest). 
			AxisStatusArray[_SourceAxisIndex][ findDiscHeight(_Disc) ]=-1;
			AxisStatusArray[_DestAxisIndex][ GetNextAvailableHeight(_DestAxisIndex) ]=_Disc;

			//MoveTower(disk - 1, spare, dest, source)   // Finally, we want disks 4 and smaller moved from peg C (spare) to peg B (dest). We do this recursively using the same procedure again. After we finish, we'll have disks 5 and smaller all on dest. 
			move(_Disc-1, _OtherAxisIndex, _DestAxisIndex, _SourceAxisIndex);
		}
	}
}



