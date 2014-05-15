using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrameMarkerController : MonoBehaviour {

	private List<GameObject> children;
	private int randomIndex;

	void Awake()
	{
		children = new List<GameObject>();

		foreach( Transform child in transform )
			children.Add( child.gameObject );

		VisualEncryptorAgent.SetSprites( children );
	}
}
