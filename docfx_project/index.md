# Tower Defense Game
Group members: David Martinez, Harsh Patel, Alexander Djordjevic
Due: November 4th 2021

## Abstract: 
To use informed search strategies, specifically A* search to create an AI that can defeat or challenge a player in a Tower Defense game. This type of game requires a player to place towers along multiple routes that defend the home base. The computer sends out waves of enemy agents whose goal is to destroy the player’s home base. By using an existing game template we can modify the enemy agent’s behavior to choose the optimal path to beat the player.

#Goal: To implement a tower defense game with AI.  

## Objectives: 
*For each agent, utilize a heuristic value based on several factors not just path length
*Currently the template either linearly or randomly selects the next node for the agent to traverse.
*User friendly
*Executable file that can be run instead of cloning the git repository. 
*Implement the tower defense game using unity
*Implement the A* algorithm using IronPython
*Use sample templates to create and redesign the interface as needed

## Contribution Plan: 
*David Martinez - Setting up TD template. Configuring IronPython to enable the A* algorithm to be written in Python. Creating the OptimalNodeSelector class in C# and sending the necessary data for the A* algorithm to compute the optimal node. Updating the FlyingAgent and AttackingAgent to utilize the optimal node from the Python script containing the A* algorithm
*Harsh Patel -  Work on implementing A* search in python to find the optimal node and path required to win the game for the AI agent.
*Alexander Djordjevic - Help implementing the A* search algorithm and creating the executable file

## Class Diagram:
This is the current state of the project using the Visual Studio Class Designer. The most relevant classes for our purpose of expanding the AI and explaining the overall game logic are kept. 
Logic to be updated is within the Agent class and NodeSelector class, specifically within the MoveToNode() function. Here we can control where the agent moves next based on the existing nodes defined within the environment. We will also need to create a new class called OptimalNodeSelector that inherits from the NodeSelector class to feed in the optimal node to the MoveToNode() method.


![supposed to be a uml diagram here](/images/TdUml.png)