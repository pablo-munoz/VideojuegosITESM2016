using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {
	public Texture heartImage;
	public PlayerController player;

	void Start() {
		player = transform.parent.GetComponent<PlayerController> ();
	}

	private void OnGUI()
	{
		Rect r = new Rect(0,0,Screen.width, Screen.height);
		GUILayout.BeginArea(r);
		GUILayout.BeginHorizontal();
		ImagesForInteger(player.getHitPoints(), heartImage);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private void ImagesForInteger(int total, Texture icon)
	{
		for(int i=0; i < player.getHitPoints(); i++)
		{
			GUILayout.Label(icon);
		}
	}
}
