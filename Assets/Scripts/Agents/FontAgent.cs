using UnityEngine;
using System.Collections;

public class FontAgent : MonoBehaviour {

	public GUIStyle textStyle;

	private static FontAgent mInstance = null;
	public static FontAgent instance
	{
		get
		{
			return mInstance;
		}
	}
	
	void Awake()
	{
		if( mInstance != null )
		{
			Debug.LogError( string.Format( "Only one instance of FontAgent allowed! Destroying:" + gameObject.name +", Other:" + mInstance.gameObject.name ) );
			Destroy( gameObject );
			return;
		}
		
		mInstance = this;
	}

	public static GUIStyle GetTextStyle()
	{
		if( instance )
			return instance.textStyle;

		return new GUIStyle();
	}
}
