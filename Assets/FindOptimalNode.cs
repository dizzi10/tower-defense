using System.Collections;
using System.Collections.Generic;
using IronPython.Hosting;
using UnityEngine;

namespace TowerDefense.Nodes
{ 
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
        /// Calls 
        /// </summary>
        /// <param name="linkedNodes">List of linked nodes</param>
        /// <returns>returns the optimal node</returns>
        public Node GetOptimalNode(List<Node> linkedNodes)
        {
            var result = optimalNodeFinder.getOptimalNode(linkedNodes);
            Debug.Log(result[1]);
            return result[0];
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}
