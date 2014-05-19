using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleAgent : MonoBehaviour {

	private enum DragDirection
	{
		Left = 0,
		Right = 1,
		Up = 2,
		Down = 3,
		Invalid = 4,
	}

	public GameObject monPrefab;
	
	private GameObject defendingMon;
	private GameObject attackingMon;

	private MonController defendingMonController;
	private MonController attackingMonController;

	private Dictionary<int, DragDirection> drags;

	private bool isEndingGame;

	private static BattleAgent mInstance;
	public static BattleAgent instance
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
			Debug.LogError( "Only one instance of BattleAgent allowed. Destroying " + gameObject + " and leaving " + mInstance.gameObject );
			Destroy( gameObject );
			return;
		}
		
		mInstance = this;

		drags = new Dictionary<int, DragDirection>();
	}

	void Start()
	{
		if( monPrefab != null )
		{
			defendingMon = Instantiate( monPrefab ) as GameObject;

			defendingMonController = defendingMon.GetComponent<MonController>();

			if( defendingMonController != null )
			{
				defendingMonController.isDefender = true;
			}


			attackingMon = Instantiate( monPrefab ) as GameObject;
			
			attackingMonController = attackingMon.GetComponent<MonController>();
			
			if( attackingMonController != null )
			{
				attackingMonController.isDefender = false;
			}
		}

		isEndingGame = false;
	}

	/*
	void Update()
	{
		if( isToggle() )
		{
			if( defendingMonController )
				defendingMonController.Reset();

			if( attackingMonController )
				attackingMonController.Reset();
		}
	}
	
	private bool isToggle()
	{
		if( Application.isEditor )
			return Input.GetKeyDown( KeyCode.Space );
		else
			return ( Input.touchCount > 2 && ( Input.touches[0].phase == TouchPhase.Began || Input.touches[1].phase == TouchPhase.Began || Input.touches[2].phase == TouchPhase.Began ) );
	}
	*/

	void OnEnable()
	{		
		FingerGestures.OnFingerUp += ReceivePressFingerUp;
		FingerGestures.OnFingerSwipe += ReceivePressSwipe;
		//FingerGestures.OnFingerDragBegin += ReceivePressDragBegin;
		//FingerGestures.OnFingerDragMove += ReceivePressDragMove;
		//FingerGestures.OnFingerDragEnd += ReceivePressDragEnd;
	}
	
	void OnDisable()
	{
		FingerGestures.OnFingerUp -= ReceivePressFingerUp;
		FingerGestures.OnFingerSwipe -= ReceivePressSwipe;
		//FingerGestures.OnFingerDragBegin -= ReceivePressDragBegin;
		//FingerGestures.OnFingerDragMove -= ReceivePressDragMove;
		//FingerGestures.OnFingerDragEnd -= ReceivePressDragEnd;
	}

	private void ReceivePressFingerUp( int fingerIndex, Vector2 fingerPos, float timeHeldDown  )
	{		
		if( fingerPos.y < Screen.height / 4f )
		{
			if( defendingMonController != null )
			{
				if( fingerPos.x < Screen.width / 4f )
					defendingMonController.MoveLeft();
				else if( fingerPos.x > Screen.width * 3f / 4f )
					defendingMonController.MoveRight();
			}
		}
		else if( fingerPos.y > Screen.height * 3f / 4f )
		{
			if( attackingMonController != null )
			{
				if( fingerPos.x < Screen.width / 4f )
					attackingMonController.MoveLeft();
				else if( fingerPos.x > Screen.width * 3f / 4f )
					attackingMonController.MoveRight();
			}
		}
	}

	private void ReceivePressSwipe( int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity )
	{
		if( startPos.y < Screen.height / 2f )
		{
			if( defendingMonController != null && direction == FingerGestures.SwipeDirection.Up )
				defendingMonController.Shoot();
		}
		else
		{
			if( attackingMonController != null && direction == FingerGestures.SwipeDirection.Down )
				attackingMonController.Shoot();
		}
	}
	
	private void ReceivePressDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
	{					
		if( !drags.ContainsKey( fingerIndex ) )
			drags.Add( fingerIndex, DragDirection.Invalid );
		else
			drags[ fingerIndex ] = DragDirection.Invalid;
	}
	
	private void ReceivePressDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
	{
		DragDirection currentDragDirection = DragDirection.Invalid;
		
		if( delta.y > 0f )
			currentDragDirection = DragDirection.Up;
		else if( delta.y < 0f )
			currentDragDirection = DragDirection.Down;
		
		if( !drags.ContainsKey( fingerIndex ) )
			drags.Add( fingerIndex, currentDragDirection );
		else
			drags[ fingerIndex ] = currentDragDirection;
	}
	
	private void ReceivePressDragEnd( int fingerIndex, Vector2 fingerPos )
	{		
		if( fingerPos.y < Screen.height / 2f )
		{
			if( defendingMonController != null && drags.ContainsKey( fingerIndex ) && drags[ fingerIndex ] == DragDirection.Up )
				defendingMonController.Shoot();
		}
		else
		{
			if( attackingMonController != null && drags.ContainsKey( fingerIndex ) && drags[ fingerIndex ] == DragDirection.Down )
				attackingMonController.Shoot();
		}
		
		if( drags.ContainsKey( fingerIndex ) )
			drags.Remove( fingerIndex );
	}

	/*
	private void ReceivePressFingerDown( int fingerIndex, Vector2 fingerPos )
	{		
		if( fingerPos.y < Screen.height / 2f )
		{
			if( defendingMonController != null )
				defendingMonController.Shoot();
		}
		else
		{
			if( attackingMonController != null )
				attackingMonController.Shoot();
		}
	}

	private void ReceivePressDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
	{					
		if( !drags.ContainsKey( fingerIndex ) )
			drags.Add( fingerIndex, DragDirection.Invalid );
		else
			drags[ fingerIndex ] = DragDirection.Invalid;
	}
	
	private void ReceivePressDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
	{
		DragDirection currentDragDirection = DragDirection.Invalid;

		if( delta.x > 0f )
			currentDragDirection = DragDirection.Right;
		else if( delta.x < 0f )
			currentDragDirection = DragDirection.Left;

		if( !drags.ContainsKey( fingerIndex ) )
			drags.Add( fingerIndex, currentDragDirection );
		else
			drags[ fingerIndex ] = currentDragDirection;
	}
	
	private void ReceivePressDragEnd( int fingerIndex, Vector2 fingerPos )
	{			
		if( fingerPos.y < Screen.height / 2f )
		{
			if( defendingMonController != null && drags.ContainsKey( fingerIndex ) )
			{
				if( drags[ fingerIndex ] == DragDirection.Left )
					defendingMonController.MoveLeft();
				else if( drags[ fingerIndex ] == DragDirection.Right )
					defendingMonController.MoveRight();
			}
		}
		else
		{
			if( attackingMonController != null && drags.ContainsKey( fingerIndex ) )
			{
				if( drags[ fingerIndex ] == DragDirection.Left )
					attackingMonController.MoveLeft();
				else if( drags[ fingerIndex ] == DragDirection.Right )
					attackingMonController.MoveRight();
			}
		}

		if( drags.ContainsKey( fingerIndex ) )
			drags.Remove( fingerIndex );
	}
	*/

	public static void DamageOpponent( List<int> shotChannels, bool isDefender )
	{
		if( instance )
			instance.internalDamageOpponent( shotChannels, isDefender );
	}

	private void internalDamageOpponent( List<int> shotChannels, bool isDefender )
	{
		bool gameOver = false;

		if( isDefender )
		{
			if( attackingMonController != null )
				gameOver = attackingMonController.Damage( shotChannels );
		}
		else
		{
			if( defendingMonController != null )
				gameOver = defendingMonController.Damage( shotChannels );
		}

		if( gameOver && !isEndingGame )
			StartCoroutine( "DoEndGame" );
	}

	public static void DamageDealt( int channel, bool isDefender )
	{
		if( instance )
			instance.internalDamageDealt( channel, isDefender );
	}

	private void internalDamageDealt( int channel, bool isDefender )
	{
		if( isDefender )
		{
			if( attackingMonController )
				attackingMonController.DestroyShotAt( channel );
		}
		else
		{
			if( defendingMonController )
				defendingMonController.DestroyShotAt( channel );
		}
	}

	private IEnumerator DoEndGame()
	{
		isEndingGame = true;

		yield return new WaitForSeconds( 3f );

		if( defendingMonController )
			defendingMonController.Reset();
		
		if( attackingMonController )
			attackingMonController.Reset();
		
		isEndingGame = false;
	}
}
