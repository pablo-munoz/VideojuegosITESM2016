using UnityEngine;
using System;
using System.Collections.Generic;

public class AutomataNode {

	private string name;
	private Dictionary<Symbol, AutomataNode> transitions;
	private Type behaviour;

	public string Name {
		get { 
			return name;
		}
	}

	public Type Behaviour{
		get { return behaviour; }
	}

	public AutomataNode(string name, Type behaviour) {
		this.name = name;
		this.behaviour = behaviour;
		transitions = new Dictionary<Symbol, AutomataNode> ();
	}


	public void AddTransition(Symbol symbol, AutomataNode node) {

		transitions.Add (symbol, node);
	}

	public AutomataNode ApplySymbol(Symbol symbol){

		if (!transitions.ContainsKey (symbol))
			return null;

		return transitions[symbol];
	}

}