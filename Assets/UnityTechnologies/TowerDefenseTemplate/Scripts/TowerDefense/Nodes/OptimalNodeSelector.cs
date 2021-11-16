using Core.Extensions;
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
		/// Script used to get the optimal node
		/// </summary>
		private FindOptimalNode findOptimalNode;

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
			return findOptimalNode.GetOptimalNode(linkedNodes);
		}

		/// <summary>
		/// Constructor for OptimalNodeSelector class
		/// Sets up the reference to the findOptimalNode script
		/// </summary>
		private void Awake()
		{
			findOptimalNode = GetComponent<FindOptimalNode>();
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