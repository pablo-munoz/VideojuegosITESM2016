using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerGUI : MonoBehaviour {
	public Texture heartImage;
	public Texture2D pickaxeImage;
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

		GUILayout.BeginHorizontal ();
		GUILayout.Label (pickaxeImage);
		GUILayout.Label ("x" + this.player.getNumPickaxes());

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
