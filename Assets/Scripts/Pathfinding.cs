using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour {

	public static List<GameObject> AStar(int originX, int originY, int destX, int destY){ 
		return null;
	}
//
//		List<Node> visited = new List<Node> ();
//		List<Node> work = new List<Node> ();
//
//		visited.Add (start);
//		work.Add (start);
//
//		start.history = new List<Node> ();
//		start.g = 0;
//		start.f = start.g + 
//			Vector3.Distance (start.transform.position, end.transform.position);
//
//		while (work.Count > 0) {
//
//			// get the best one (the smallest f)
//
//			int smallest = 0;
//
//			for(int i = 0; i < work.Count; i++) {
//				if(work[i].f < work[smallest].f){
//					smallest = i;
//				}
//			}
//
//			Node smallestNode = work[smallest];
//
//			// remove from working list
//			work.Remove(smallestNode);
//
//			if (smallestNode == end) {
//				// found	
//				List<Node> result = new List<Node>(smallestNode.history);
//				result.Add (smallestNode);
//				return result;
//
//			} else {
//
//				// not found
//				for(int i = 0; i < smallestNode.neighbors.Length; i++){
//					Node currentChild = smallestNode.neighbors[i];
//
//					if (!visited.Contains (currentChild)) {
//
//						visited.Add (currentChild);
//						work.Add (currentChild);
//
//						// f, g, h
//						currentChild.g = smallestNode.g +
//							Vector3.Distance (currentChild.transform.position, 
//								smallestNode.transform.position);
//
//						float h = Vector3.Distance (currentChild.transform.position,
//							end.transform.position);
//
//						currentChild.f = currentChild.g + h;
//
//						// update history
//						currentChild.history = new List<Node>(smallestNode.history);
//						currentChild.history.Add (smallestNode);
//					}
//				}
//			}
//		}
//
//		return null; 
//	}

}
