using Core.Extensions;
using System.Collections.Generic;
using TowerDefense.Towers;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace TowerDefense.Nodes
{
	/// <summary>
	/// Selects next optimal node based on A* algorithm
	/// </summary>
	public class OptimalNodeSelector : NodeSelector
	{
		/// <summary>
		/// A list of Nodes that appear in the level excluding start and end node
		/// </summary>
		public List<Node> nodes;

		/// <summary>
		/// Script used to get the optimal node
		/// </summary>
		private FindOptimalNode findOptimalNode;

		/// <summary>
		/// The starting node this node selector chooses from
		/// </summary>
		Node parentNode;

		/// <summary>
		/// Selects the next node using <see cref="findOptimalNode" />
		/// </summary>
		/// <returns>The optimal node, or null if there are no valid nodes</returns>
		public override Node GetNextNode()
		{

			if (linkedNodes == null)
			{
				return null;
			}
			return findOptimalNode.GetOptimalNode(parentNode.currentAgent);
		}

		void UpdatePath(Tower tower)
        {
			Node closest = GetClosestNodeToTower(tower);
			Debug.Log($"Tower {tower.name} is closest to {closest.name} node");

			if (tower.isDead)
            {
				closest.nearbyTowers.Remove(tower);
				Debug.Log($"Tower {tower.name} is destroyed/sold updating path");
			}
			else
            {	
				closest.nearbyTowers.Add(tower);
				Debug.Log($"Tower {tower.name} has been created updating path");

			}
			// updates the stats of the node that was affected by the tower addition/removal
			closest.SetNodeStats();

			findOptimalNode.SetOptimalNodes(parentNode);

		}

		/// <summary>
		/// Gets the closest node to the position of the input tower
		/// </summary>
		/// <param name="tower">Node that is closest to the tower</param>
		/// <returns></returns>
		Node GetClosestNodeToTower(Tower tower)
		{
			Node best = null;
			float closestDistanceSqr = Mathf.Infinity;
			Vector3 towerPosition = tower.transform.position;
			
			for (int i = 0; i<nodes.Count; i++)
			{
				Node current = nodes[i];
				Vector3 directionToTarget = current.transform.position - towerPosition;
				float dSqrToTarget = directionToTarget.sqrMagnitude;
				if (dSqrToTarget < closestDistanceSqr)
				{
					closestDistanceSqr = dSqrToTarget;
					best = current;
				}
			}
			return best;
		}

		/// <summary>				
		/// Constructor for OptimalNodeSelector class
		/// Sets up the reference to the findOptimalNode script
		/// </summary>
		private void Awake()
		{
			findOptimalNode = GetComponent<FindOptimalNode>();
			parentNode = GetComponent<Node>();
		}

		private void Start()
        {	
			Tower.towerModified += UpdatePath;
		}

        private void OnDestroy()
        {
			Tower.towerModified -= UpdatePath;
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			base.OnDrawGizmos();
		}
#endif
	}
}