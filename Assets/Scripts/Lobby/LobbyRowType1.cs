using UnityEngine;
using System.Collections;

public class LobbyRowType1 : MonoBehaviour
{
#region Unity Editor
	public GameObject[] slots;
#endregion
    public static LobbyRowType1 Create(UITable parent)
    {
        GameObject go = GameObject.Instantiate(Resources.Load("/Prefabs/Lobby/LobbyRowType1")) as GameObject;
        go.transform.parent = parent.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.GetComponent<UIDragScrollView>().scrollView = parent.GetComponentInParent<UIScrollView>();
        return go.GetComponent<LobbyRowType1>();
    }
	void Start ()
	{
        gameObject.GetComponent<UIEventListener>().onClick += onTableClick;   
	}
    void OnDestroy()
    {
        gameObject.GetComponent<UIEventListener>().onClick -= onTableClick;   
    }
    private void onTableClick(GameObject go)
    {
        Application.LoadLevel(Scene.GameplayScene.ToString());
    }
	 


	
	// Update is called once per frame
	void Update ()
	{
		float value = Mathf.Lerp(0.731f, 1.15f, 0.02f / Vector3.SqrMagnitude(transform.position - LobbyScene.centerObject));
		gameObject.transform.localScale = new Vector3(value, value, 1f);
		if (!gameObject.transform.name.Equals (NGUITools.FindInParents<UICenterOnChild> (gameObject).centeredObject.transform.name)) {
						gameObject.GetComponent<UISprite> ().color = new Color (69f/255f, 69f/255f, 69f/255f);
		} else {
			gameObject.GetComponent<UISprite> ().color = new Color (1f, 1f, 1f);
		}
	}
}

