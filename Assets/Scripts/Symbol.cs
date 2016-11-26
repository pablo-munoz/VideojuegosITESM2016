using UnityEngine;
using System.Collections;

public class Symbol {

	private string name;

	// properties are fun
	public string Name {

		private set { 
			name = value;
		}

		get { 
			return name;
		}
	}

	public Symbol(string name){
		this.Name = name;
	}
}
