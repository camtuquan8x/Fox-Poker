using UnityEngine;
using System.Collections;

public class LobbyRowType1 : MonoBehaviour
{

	public GameObject[] slots;
	void Start ()
	{

//		NGUITools.FindInParents<CUICenterOnChild> (gameObject).onCenter = onCenterObject;
	}
	


	
	// Update is called once per frame
	void Update ()
	{
		float value = Mathf.Lerp(0.731f, 1f, 0.02f / Vector3.SqrMagnitude(transform.position - LobbyScene.centerObject));
		gameObject.transform.localScale = new Vector3(value, value, 1f);
	}
}

