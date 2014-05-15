using UnityEngine;
using System.Collections;

public class BattleController : MonoBehaviour {

	private GUIStyle textStyle;
	private Rect newMonRect;
	private Rect vsRect;
	private Rect currentMonRect;
	private string vsString;
	private string newMonString;
	private string currentMonString;
	
	private int currentMonID;
	private int newMonID;

	private MonAgent.Mon currentMon;
	private MonAgent.Mon newMon;
	
	void Start () {
		
		textStyle = FontAgent.GetTextStyle();

		newMonRect = new Rect( 0f, Screen.height / 4f, Screen.width, Screen.height / 4f );

		vsRect = new Rect( 0f, 0f, Screen.width, Screen.height );

		currentMonRect = new Rect( 0f, Screen.height * 2f / 4f, Screen.width, Screen.height / 4f );
	}

	void OnEnable()
	{
		currentMonString = "";
		
		int currentMonID = MonAgent.GetCurrentMonID ();
		currentMon = MonAgent.MonFromID( currentMonID );

		if( currentMonID != -1 )
			currentMonString = currentMon.ToString();


		newMonString = "";
		
		int newMonID = MonAgent.GetLastFoundMonID ();
		newMon = MonAgent.MonFromID( newMonID );

		if( newMonID != -1 )
			newMonString = newMon.ToString();

		vsString = "VS";

		StartCoroutine( "DoBattle" );
	}

	void OnGUI()
	{
		GUI.Label( newMonRect, newMonString, textStyle );
		GUI.Label( vsRect, vsString, textStyle );
		GUI.Label( currentMonRect, currentMonString, textStyle );
	}

	private IEnumerator DoBattle()
	{
		yield return new WaitForSeconds( 3f );

		if( currentMon.currentTypeType == MonAgent.TypeType.Invalid )
		{
			currentMon = new MonAgent.Mon( newMon.currentTypeType, newMon.currentAttack1Type, newMon.currentAttack2Type );
		}
		else
		{
			int randomValue = Mathf.FloorToInt( Random.value * 3f );

			switch( randomValue )
			{
				case 0: currentMon = new MonAgent.Mon( MonAgent.GetComboTypeType( newMon.currentTypeType, currentMon.currentTypeType ), currentMon.currentAttack1Type, currentMon.currentAttack2Type ); break;
				case 1: currentMon = new MonAgent.Mon( currentMon.currentTypeType, newMon.currentAttack1Type, currentMon.currentAttack2Type ); break;
				case 2: currentMon = new MonAgent.Mon( currentMon.currentTypeType, currentMon.currentAttack1Type, newMon.currentAttack2Type ); break;
			}
		}

		currentMonString = currentMon.ToString();

		MonAgent.SetCurrentMonID( MonAgent.IDFromMon( currentMon ) );
		MonAgent.SetLastFoundMonID( -1 );

		yield return new WaitForSeconds( 1f );

		newMonString = "";
		vsString = "";

		yield return new WaitForSeconds( 3f );

		StateAgent.ChangeState( StateAgent.State.Showing );
	}
}
