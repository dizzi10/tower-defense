using System.Collections.Generic;
using TowerDefense.Agents;
using TowerDefense.MeshCreator;
using TowerDefense.Towers;
using TowerDefense.Towers.TowerLaunchers;
using UnityEngine;

namespace TowerDefense.Nodes
{
	/// <summary>
	/// A point along the path which agents will navigate towards before recieving the next instruction from the NodeSelector
	/// Requires a collider to be added manually.
	/// </summary>
	[RequireComponent(typeof(Collider))]
	public class Node : MonoBehaviour
	{
		/// <summary>
		/// Reference to the MeshObject created by an AreaMeshCreator
		/// </summary>
		[HideInInspector]
		public AreaMeshCreator areaMesh;

		/// <summary>
		/// Selection weight of the node
		/// </summary>
		public int weight = 1;

		/// <summary>
		/// List of towers near the node
		/// </summary>
		[HideInInspector]
		public List<Tower> nearbyTowers;

		/// <summary>
		/// Total hit points of all nearby towers summed up
		/// </summary>
		public float cumulativeHP { get; private set; }

		/// <summary>
		/// Total damage per second of all nearby towers summed up
		/// </summary>
		public float cumulativeDPS { get; private set; }

		public Agent currentAgent {get; private set;}

		/// <summary>
		/// Node selector used with this node
		/// </summary>
		public NodeSelector selector;

		/// <summary>
		/// Defines whats type of enemy agents this node excels against
		/// </summary>
		public enum EnemyPref
        {
			/// <summary>
			/// Beats Flying Agents
			/// </summary>
			AntiAir,

			/// <summary>
			/// Beats Attacking Agents
			/// </summary>
			AntiGround
        }

		public EnemyPref enemyPref { get; protected set; }

	/// <summary>
	/// Gets the next node from the selector
	/// </summary>
	/// <returns>Next node, or null if this is the terminating node</returns>
	public Node GetNextNode()
		{
			
			if (selector != null)
			{
				return selector.GetNextNode();
			}
			return null;
		}

		/// <summary>
		/// Gets a random point inside the area defined by a node's meshcreator
		/// </summary>
		/// <returns>A random point within the MeshObject's area</returns>
		public Vector3 GetRandomPointInNodeArea()
		{
			// Fallback to our position if we have no mesh
			return areaMesh == null ? transform.position : areaMesh.GetRandomPointInside();
		}

		/// <summary>
		/// When agent enters the node area, get the next node
		/// </summary>
		public virtual void OnTriggerEnter(Collider other)
		{
			var agent = other.gameObject.GetComponent<Agent>();

			//need current agent to compute optimal node
			currentAgent = agent;
			if (agent != null)
			{
				agent.GetNextNode(this);
			}
		}

		public void SetNodeStats()
        {
			SetCumulativeHP();
			SetCumulativeDPS();
			SetEnemyPref();
		}


		private void SetCumulativeHP()
        {
			float hp = 0;
			for(int i=0; i < nearbyTowers.Count; i++)
            {
				hp += nearbyTowers[i].configuration.currentHealth;
            }
			cumulativeHP = hp;
        }
		private void SetCumulativeDPS() 
		{
			float dps = 0;
			for (int i = 0; i < nearbyTowers.Count; i++)
			{
				dps += nearbyTowers[i].currentTowerLevel.GetTowerDps();
			}
			cumulativeDPS = dps;
		}


		//TODO Sloppy implementation needs to be redone
		private void SetEnemyPref() 
		{
			int numAATowers = 0;
			int numAGTowers = 0;
			for (int i = 0; i < nearbyTowers.Count; i++)
			{
				//tower that uses a ballisitc launcher is anti-ground
                if(nearbyTowers[i].gameObject.GetComponentInChildren<BallisticLauncher>())
                {
					numAGTowers++;
                }
                else
                {
					numAATowers++;
                }
			}
			if(numAATowers >= numAGTowers)
            {
				enemyPref = EnemyPref.AntiAir;
			}
            else
            {
				enemyPref = EnemyPref.AntiGround;
            }
		}

        private void Awake()
        {
			selector = GetComponent<NodeSelector>();
		}


#if UNITY_EDITOR
        /// <summary>
        /// Ensure the collider is a trigger
        /// </summary>
        protected void OnValidate()
		{
			var trigger = GetComponent<Collider>();
			if (trigger != null)
			{
				trigger.isTrigger = true;
			}
			
			// Try and find AreaMeshCreator
			if (areaMesh == null)
			{
				areaMesh = GetComponentInChildren<AreaMeshCreator>();
			}
		}

		void OnDrawGizmos()
		{
			Gizmos.DrawIcon(transform.position + Vector3.up, "movement_node.png", true);
		}
#endif
	}
}