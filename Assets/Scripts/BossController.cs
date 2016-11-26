using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour {

	private AutomataNode current;
	private Symbol cast;
	private MonoBehaviour currentBehaviour;

	// Use this for initialization
	void Start () {
		AutomataNode resting = new AutomataNode ("idle", typeof(BossIdleBehaviour));
		AutomataNode casting = new AutomataNode ("casting", typeof(BossCastingBehaviour));

		cast = new Symbol ("cast");

		resting.AddTransition (cast, casting);

		current = resting;
		currentBehaviour = (MonoBehaviour)gameObject.AddComponent (current.Behaviour);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void ChangeState(Symbol symbol){
		current = current.ApplySymbol (symbol);
		Destroy (currentBehaviour);
		currentBehaviour = (MonoBehaviour)gameObject.AddComponent (current.Behaviour);
	}
}
