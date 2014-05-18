using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonController : MonoBehaviour {

	public bool isDefender = false;

	private List<GameObject> segments;

	private List<int> currentChannelPositions;
	private int numSegments;

	private List<GameObject> shotsFired;
	private float shotSpeedPercent;

	private List<int> shotChannels;
	private List<int> damagingShotChannels;
	private int shotsToDestroy = 0;
	
	private Quaternion rotation = Quaternion.identity;

	void Start()
	{
		if( MonAgent.GetMonPrefab() == null )
		{
			enabled = false;
			return;
		}

		currentChannelPositions = new List<int>();
		segments = new List<GameObject>();
		shotsFired = new List<GameObject>();
		shotChannels = new List<int>();
		damagingShotChannels = new List<int>();

		Reset();
	}

	public void Reset()
	{
		int randomValue = Mathf.FloorToInt( Random.value * 3f );

		switch( randomValue )
		{
			case 0: numSegments = 1; shotSpeedPercent = 2f; break;
			case 1: numSegments = 2; shotSpeedPercent = 1f; break;
			case 2: numSegments = 3; shotSpeedPercent = 0.5f; break;
		}

		currentChannelPositions.Clear();

		for( int i = 0; i < segments.Count; i++ )
			Destroy( segments[i] );

		segments.Clear();

		StopCoroutine( "RunShotsFired" );
		DestroyShotsFired();

		for( int i = 0; i < numSegments; i++ )
		{
			currentChannelPositions.Add( ( BoardAgent.NumChannels / 2 + ( i - 1 ) * ( isDefender ? 1 : -1 ) ) );

			segments.Add( Instantiate( MonAgent.GetMonPrefab(), BoardAgent.ChannelPositionToScreenPosition( currentChannelPositions[i], isDefender ), rotation ) as GameObject );
			segments[i].transform.parent = transform;
			segments[i].transform.localScale = Vector3.one * BoardAgent.ChannelWidth * 0.75f;
		}
	}

	private void DestroySegment( int segmentIndex )
	{
		if( segmentIndex > segments.Count - 1 )
			return;

		Destroy( segments[ segmentIndex ] );
		segments.RemoveAt( segmentIndex );
		currentChannelPositions.RemoveAt( segmentIndex );
	}

	private void DestroyShotsFired()
	{
		if( shotsFired.Count > 0 )
			for( int i = 0; i < shotsFired.Count; i++ )
				Destroy( shotsFired[i] );

		shotsFired.Clear();
		shotChannels.Clear();
	}

	private void DestroyShot( int index )
	{
		if( index > shotsFired.Count - 1 )
			return;

		Destroy( shotsFired[ index ] );
		shotsFired.RemoveAt( index );
		shotChannels.RemoveAt( index );
	}
	
	private void UpdateScreenPosition()
	{
		for( int i = 0; i < segments.Count; i++ )
			segments[i].transform.position = BoardAgent.ChannelPositionToScreenPosition( currentChannelPositions[i], isDefender );
	}

	public void MoveLeft()
	{
		if( currentChannelPositions.Count == 0 )
			return;

		if( isDefender )
		{
			if( currentChannelPositions[0] > 0 )
				for( int i = 0; i < currentChannelPositions.Count; i++ )
					currentChannelPositions[i]--;
		}
		else
		{
			if( currentChannelPositions[ currentChannelPositions.Count - 1 ] > 0 )
				for( int i = 0; i < currentChannelPositions.Count; i++ )
					currentChannelPositions[i]--;
		}

		UpdateScreenPosition();
	}

	public void MoveRight()
	{
		if( currentChannelPositions.Count == 0 )
			return;

		if( isDefender )
		{
			if( currentChannelPositions[ currentChannelPositions.Count - 1 ] < BoardAgent.NumChannels - 1 )
				for( int i = 0; i < currentChannelPositions.Count; i++ )
					currentChannelPositions[i]++;
		}
		else
		{
			if( currentChannelPositions[0] < BoardAgent.NumChannels - 1 )
				for( int i = 0; i < currentChannelPositions.Count; i++ )
					currentChannelPositions[i]++;
		}
		
		UpdateScreenPosition();
	}

	public void Shoot()
	{
		bool shouldStartCoroutine = ( shotsFired.Count == 0 );

		for( int i = 0; i < segments.Count; i++ )
		{
			int newShotChannel = currentChannelPositions[i];
			shotChannels.Add( newShotChannel );

			GameObject newShot = Instantiate( MonAgent.GetShotPrefab(), BoardAgent.ChannelPositionToScreenPosition( newShotChannel, isDefender ), rotation ) as GameObject;
			newShot.transform.localScale = Vector3.one * BoardAgent.ChannelWidth * 0.25f;

			shotsFired.Add( newShot );
		}

		if( shouldStartCoroutine )
			StartCoroutine( "RunShotsFired" );
	}

	public bool Damage( List<int> damageChannels )
	{
		for( int i = 0; i < damageChannels.Count; i++ )
		{
			for( int j = 0; j < currentChannelPositions.Count; j++ )
			{
				if( damageChannels[i] == currentChannelPositions[j] )
					DestroySegment( j );
			}
		}

		return ( segments.Count == 0 );
	}

	private IEnumerator RunShotsFired()
	{
		while( shotsFired.Count > 0 )
		{
			float distance = Screen.height * shotSpeedPercent * Time.deltaTime * ( isDefender ? 1f : -1f );

			damagingShotChannels.Clear();
			shotsToDestroy = 0;

			for( int i = 0; i < shotsFired.Count; i++ )
			{
				shotsFired[i].transform.position += Vector3.up * distance;

				if( isDefender )
				{
					if( shotsFired[i].transform.position.y > Screen.height - BoardAgent.ChannelWidth )
						damagingShotChannels.Add( shotChannels[i] );
					
					if( shotsFired[i].transform.position.y > Screen.height )
						shotsToDestroy++;
				}
				else
				{
					if( shotsFired[i].transform.position.y < BoardAgent.ChannelWidth )
						damagingShotChannels.Add( shotChannels[i] );
					
					if( shotsFired[i].transform.position.y < 0f )
						shotsToDestroy++;
				}

			}

			BattleAgent.DamageOpponent( damagingShotChannels, isDefender );

			for( int i = 0; i < shotsToDestroy; i++ )
				DestroyShot( 0 );

			if( shotsFired.Count == 0 )
				yield break;

			yield return null;
		}
	}
}
