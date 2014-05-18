using UnityEngine;
using System.Collections;

public class BoardAgent : MonoBehaviour {
		
	public static int NumChannels = 5;
	public static float ChannelWidth;

	public GameObject DefenseButtonPrefab;
	public GameObject AttackButtonPrefab;

	private static BoardAgent mInstance;
	public static BoardAgent instance
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
			Debug.LogError( "Only one instance of BoardAgent allowed. Destroying " + gameObject + " and leaving " + mInstance.gameObject );
			Destroy( gameObject );
			return;
		}
		
		mInstance = this;
	}

	void Start()
	{
		ChannelWidth = Screen.width / NumChannels;

		GameObject temp;
		float buttonScale = Screen.width * 0.66f;

		if( DefenseButtonPrefab )
		{
			temp = Instantiate( DefenseButtonPrefab ) as GameObject;
			temp.transform.position = CameraAgent.MainCameraObject.camera.ScreenToWorldPoint( new Vector3( 0f, 0f, 11f ) );
			temp.transform.localScale = new Vector3( buttonScale, buttonScale, 1f );

			temp = Instantiate( DefenseButtonPrefab ) as GameObject;
			temp.transform.position = CameraAgent.MainCameraObject.camera.ScreenToWorldPoint( new Vector3( Screen.width, 0f, 11f ) );
			temp.transform.localScale = new Vector3( buttonScale, buttonScale, 1f );
		}

		if( AttackButtonPrefab )
		{
			temp = Instantiate( AttackButtonPrefab ) as GameObject;
			temp.transform.position = CameraAgent.MainCameraObject.camera.ScreenToWorldPoint( new Vector3( 0f, Screen.height, 11f ) );
			temp.transform.localScale = new Vector3( buttonScale, buttonScale, 1f );
			
			temp = Instantiate( AttackButtonPrefab ) as GameObject;
			temp.transform.position = CameraAgent.MainCameraObject.camera.ScreenToWorldPoint( new Vector3( Screen.width, Screen.height, 11f ) );
			temp.transform.localScale = new Vector3( buttonScale, buttonScale, 1f );
		}
	}

	public static Vector3 ChannelPositionToScreenPosition( int channelPosition, bool isDefender )
	{
		return new Vector3 (ChannelWidth * (0.5f + channelPosition), (isDefender ? ChannelWidth * 0.5f : Screen.height - ChannelWidth * 0.5f), 0f);
	}
}
