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
            Debug.Log(@$"Possible nodes from start {string.Join(",", result[0])} Chosen node is {result[1].name} with nearby towers: {result[2]}
            total hp: {result[3]}, total dps: {result[4]}, enemy preference: {result[5]},
            node pos: {result[6]}");

            bestAttackingAgentNode = result[0][0];
            bestFlyingAgentNode = result[0][1];
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
