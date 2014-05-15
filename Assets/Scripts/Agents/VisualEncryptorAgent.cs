using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisualEncryptorAgent : MonoBehaviour {

	private int lastFoundTrackableID = -1;
	private List<GameObject> sprites;

	private static VisualEncryptorAgent mInstance = null;
	public static VisualEncryptorAgent instance
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
			Debug.LogError( string.Format( "Only one instance of VisualEncryptorAgent allowed! Destroying:" + gameObject.name +", Other:" + mInstance.gameObject.name ) );
			Destroy( gameObject );
			return;
		}
		
		mInstance = this;
	}

	/*
	void OnGUI()
	{
		if( lastFoundTrackableID != -1 )
			GUI.Label( new Rect( 10f, 10f, 100f, 100f ), "" + lastFoundTrackableID );
	}

	public static void TrackableFound( int ID )
	{
		if( instance )
			instance.internalTrackableFound( ID );
	}

	private void internalTrackableFound( int ID )
	{
		StopCoroutine( "WaitAndResetTrackableID" );

		lastFoundTrackableID = ID;

		StartCoroutine( "WaitAndResetTrackableID" );
	}
	*/

	public static void SetSprites( List<GameObject> newSprites )
	{
		if( instance )
			instance.internalSetSprites( newSprites );
	}

	private void internalSetSprites( List<GameObject> newSprites )
	{
		sprites = newSprites;
	}

	public static void DisplaySprite( int index )
	{
		if( instance )
			instance.internalDisplaySprite( index );
	}

	private void internalDisplaySprite( int index )
	{
		for( int i = 0; i < sprites.Count; i++ )
			sprites[i].SetActive( i == index );
	}

	/*
	private IEnumerator WaitAndResetTrackableID()
	{
		yield return new WaitForSeconds( 5f );

		lastFoundTrackableID = -1;
	}
	*/
}
