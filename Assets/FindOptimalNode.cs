using System.Collections;
using System.Collections.Generic;
using IronPython.Hosting;
using TowerDefense.Agents;
using UnityEngine;

namespace TowerDefense.Nodes
{
    /// <summary>
    /// Configures python script to find optimal node
    /// </summary>
    public class FindOptimalNode : MonoBehaviour
    {
        /// <summary>
        /// python file using Ironpython
        /// </summary>
        private dynamic py;

        /// <summary>
        /// class from python file
        /// </summary>
        private dynamic optimalNodeFinder;

        /// <summary>
        /// Optimal node for attacking agent updated on path update
        /// </summary>
        public Node bestAttackingAgentNode  { get; private set; }

        /// <summary>
        /// Optimal node for flying agent updated on path update
        /// </summary>
        public Node bestFlyingAgentNode { get; private set; }

        /// <summary>
        /// Calls 
        /// </summary>
        /// <param name="linkedNodes">List of linked nodes</param>
        /// <returns>returns the optimal node</returns>
        public Node GetOptimalNode(Agent enemyAgent)
        {
            if(enemyAgent is AttackingAgent) 
            { 
                return bestAttackingAgentNode; 
            }
            else 
            {
                return bestFlyingAgentNode; 
            }
        }

        /// <summary>
        /// Calls python file with A* to get optimal nodes
        /// </summary>
        /// <param name="linkedNodes">List of linked nodes</param>
        public void SetOptimalNodes(Node startNode)
        {
            var result = optimalNodeFinder.getOptimalNode(startNode);

            bestAttackingAgentNode = result[0];
            bestFlyingAgentNode = result[1];

            Debug.Log(@$"Possible nodes from start {string.Join(",", startNode.selector.linkedNodes)}");

            Debug.Log(@$"Chosen nodes are {bestAttackingAgentNode.name} for attacking agents with nearby towers: {bestAttackingAgentNode.nearbyTowers.Count} and 
            {bestFlyingAgentNode.name} for flying agents with nearby towers: {bestFlyingAgentNode.nearbyTowers.Count}");

            Debug.Log(@$"{bestAttackingAgentNode.name} stats: total hp: {bestAttackingAgentNode.cumulativeHP}, 
            total dps: {bestAttackingAgentNode.cumulativeDPS}, enemy preference: {bestAttackingAgentNode.enemyPref},
            node pos: {bestAttackingAgentNode.transform.position}");

            Debug.Log(@$" {bestFlyingAgentNode.name} stats: total hp: {bestFlyingAgentNode.cumulativeHP}, 
            total dps: {bestFlyingAgentNode.cumulativeDPS}, enemy preference: {bestFlyingAgentNode.enemyPref},
            node pos: {bestFlyingAgentNode.transform.position}");
        }

        /// <summary>
        /// Sets up Ironpython integration, where the optimal node will be given from FindOptimalNode.py
        /// </summary>
        void Awake()
        {
            var engine = Python.CreateEngine();

            ICollection<string> searchPaths = engine.GetSearchPaths();

            //Path to the folder of FindOptimalNode.py
            searchPaths.Add(Application.dataPath);
            //Path to the Python standard library
            searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
            engine.SetSearchPaths(searchPaths);

            py = engine.ExecuteFile(Application.dataPath + @"\FindOptimalNode.py");
            optimalNodeFinder = py.FindOptimalNode();
            Debug.Log(optimalNodeFinder);

        }

    }
}
