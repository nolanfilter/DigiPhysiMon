using UnityEngine;
using System.Collections;

public class StatsController : MonoBehaviour {

	private GUIStyle textStyle;
	private Rect textRect;
	private string content;

	void Start () {
	
		textStyle = FontAgent.GetTextStyle();
		textRect = new Rect( 0f, Screen.height * 2f / 3f, Screen.width, Screen.height / 3f );
	}

	void OnEnable()
	{
		content = "Find a friend!";

		int monID = MonAgent.GetCurrentMonID ();

		if( monID != -1 )
			content = MonAgent.MonFromID( monID ).ToString();

		VisualEncryptorAgent.DisplaySprite( monID );
	}

	void OnGUI()
	{
		GUI.Label( textRect, content, textStyle );
	}
}
