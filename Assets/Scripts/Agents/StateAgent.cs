using UnityEngine;
using System.Collections;

public class StateAgent : MonoBehaviour {

	public enum State
	{
		Looking = 0,
		Showing = 1,
		Playing = 2,
		Invalid = 3,
	}
	private State currentState = State.Invalid;

	public GameObject[] lookingObjects;
	public GameObject[] showingObjects;
	public GameObject[] playingObjects;

	private static StateAgent mInstance = null;
	public static StateAgent instance
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
			Debug.LogError( string.Format( "Only one instance of StateAgent allowed! Destroying:" + gameObject.name +", Other:" + mInstance.gameObject.name ) );
			Destroy( gameObject );
			return;
		}
		
		mInstance = this;
	}

	void Start()
	{
		ChangeState( State.Showing );
	}

	void Update()
	{
		if( isToggle() )
		{
			if( currentState == State.Looking )
				ChangeState( State.Showing );
			else if( currentState == State.Showing )
				ChangeState( State.Looking );
		}
	}

	private bool isToggle()
	{
		if( Application.isEditor )
			return Input.GetKeyDown( KeyCode.Space );
		else
			return ( Input.touchCount > 2 && ( Input.touches[0].phase == TouchPhase.Began || Input.touches[1].phase == TouchPhase.Began || Input.touches[2].phase == TouchPhase.Began ) );
	}

	public static void ChangeState( State newState )
	{
		if( instance )
			instance.internalChangeState( newState );
	}

	private void internalChangeState( State newState )
	{
		if( currentState == newState )
			return;

		currentState = newState;

		for( int i = 0; i < lookingObjects.Length; i++ )
			lookingObjects[i].SetActive( currentState == State.Looking );

		for( int i = 0; i < showingObjects.Length; i++ )
			showingObjects[i].SetActive( currentState == State.Showing );

		for( int i = 0; i < playingObjects.Length; i++ )
			playingObjects[i].SetActive( currentState == State.Playing );
	}
}
