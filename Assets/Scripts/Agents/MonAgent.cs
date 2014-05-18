using UnityEngine;
using System;
using System.Collections;

public class MonAgent : MonoBehaviour {

	public enum TypeType
	{
		Fire = 0,
		Water = 1,
		Inferno = 2,
		Ocean = 3,
		Steam = 4,
		Invalid = 5,
	}

	public enum Attack1Type
	{
		Punch = 0,
		Kick = 1,
		Tackle = 2,
		Invalid = 3,
	}

	public enum Attack2Type
	{
		Boomerang = 0,
		NinjaStars = 1,
		Invalid = 2,
	}
	
	public GameObject monPrefab;
	public GameObject shotPrefab;

	private static int TypeTypeLength = Enum.GetNames( typeof( TypeType ) ).Length - 1;
	private static int Attack1TypeLength = Enum.GetNames( typeof( Attack1Type ) ).Length - 1;
	private static int Attack2TypeLength = Enum.GetNames( typeof( Attack2Type ) ).Length - 1;
	private static int MaxID = ( TypeTypeLength * Attack1TypeLength * Attack2TypeLength ) - 1;

	private int currentMonID = -1;
	private int lastFoundMonID = -1;

	public struct Mon
	{
		public TypeType currentTypeType { get; private set; }
		public Attack1Type currentAttack1Type { get; private set; }
		public Attack2Type currentAttack2Type { get; private set; }

		public Mon( TypeType newTypeType, Attack1Type newAttack1Type, Attack2Type newAttack2Type )
		{
			currentTypeType = newTypeType;
			currentAttack1Type = newAttack1Type;
			currentAttack2Type = newAttack2Type;
		}

		public string ToString()
		{
			return "" + currentTypeType + ", " + currentAttack1Type + ", " + currentAttack2Type;
		}
	}
	
	private static MonAgent mInstance = null;
	public static MonAgent instance
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
			Debug.LogError( string.Format( "Only one instance of MonAgent allowed! Destroying:" + gameObject.name +", Other:" + mInstance.gameObject.name ) );
			Destroy( gameObject );
			return;
		}
		
		mInstance = this;
	}

	/*
	void Start()
	{
		//Debug.Log( MaxID );
		int test = 23;
		Mon mon = MonFromID( test );

		Debug.Log( mon.ToString() );
		Debug.Log( IDFromMon( mon ) );
	}
	*/

	public static int GetCurrentMonID()
	{
		if( instance )
			return instance.currentMonID;

		return -1;
	}

	public static void SetCurrentMonID( int newMonID )
	{
		if( instance )
			instance.currentMonID = newMonID;
	}

	public static int GetLastFoundMonID()
	{
		if( instance )
			return instance.lastFoundMonID;
		
		return -1;
	}
	
	public static void SetLastFoundMonID( int newMonID )
	{
		if( instance )
			instance.lastFoundMonID = newMonID;
	}

	public static Mon MonFromID( int ID )
	{
		if( ID < 0 || ID > MaxID )
			return new Mon( TypeType.Invalid, Attack1Type.Invalid, Attack2Type.Invalid );

		Attack2Type attack2Type = (Attack2Type)( ID / ( TypeTypeLength * Attack1TypeLength ) );
		Attack1Type attack1Type = (Attack1Type)( ( ID / TypeTypeLength ) % Attack1TypeLength );
		TypeType typeType = (TypeType)( ID % TypeTypeLength );

		return new Mon( typeType, attack1Type, attack2Type );
	}

	public static int IDFromMon( Mon mon )
	{
		return (int)mon.currentTypeType + ( (int)mon.currentAttack1Type ) * TypeTypeLength + ( (int)mon.currentAttack2Type ) * TypeTypeLength * Attack1TypeLength;
	}

	public static TypeType GetComboTypeType( TypeType newTypeType, TypeType oldTypeType )
	{
		if( newTypeType == TypeType.Fire )
		{
			if( oldTypeType == TypeType.Fire )
				return TypeType.Inferno;
			else if( oldTypeType == TypeType.Water )
				return TypeType.Steam;
		}
		else if( newTypeType == TypeType.Water )
		{
			if( oldTypeType == TypeType.Water )
				return TypeType.Ocean;
			else if( oldTypeType == TypeType.Fire )
				return TypeType.Steam;
		}

		return newTypeType;
	}

	public static GameObject GetMonPrefab()
	{
		if( instance )
			return instance.monPrefab;

		return null;
	}

	public static GameObject GetShotPrefab()
	{
		if( instance )
			return instance.shotPrefab;
		
		return null;
	}
}
