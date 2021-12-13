# Reference for code used: https://github.com/MilanPecov/15-Puzzle-Solvers
# commented in my understanding of the program, added further simplifications,
# heavily modified their interpretation of the A* algorithm to
# more closely match the textbook's version

import heapq

class FindOptimalNode():
    """
    Finds the optimal node with A* algorithm using a heuristic function h(n)
    """

    def __init__(self):
        pass

    def getOptimalNode(self, startNode):

        # these are all the useful atttributes of the nodes you will need for the heuristic function
        # there are two types of agents calculate optimal node once for flying agent and once for attacking agent,

        # shows to get a child node
        # aChildNode = startNode.selector.linkedNodes[0] 

        # shows how to get list of children
        # - childNodes = startNode.selector.linkedNodes     

        # use for heuristic
        # - aChildNode.cumulativeDPS
        # - aChildNode.cumulativeHP
        # - len(aChildNode.nearbyTowers)

        # use for heuristic, it is either "AntiAir" or "AntiGround", run A* twice once for each type of agent 
        # - aChildNode.enemyPref 

        # position of node 
        # -aChildNode.transform.position)
     
        optimalAttackingNode = self.a_star(startNode, "ATTACKING_AGENT")
        optimalFlyingNode = self.a_star(startNode, "FLYING_AGENT")

        return (optimalAttackingNode, optimalFlyingNode)

           
    def a_star(self, startNode, AGENT_TYPE):

       
        pq = []

        #  to keep track of each node's path cost, heuristic cost and parent
        #  also to check if visited
        #  index 0 is g(n), index 1 is h(n), index 2 is parent node 
        node_dict = {startNode: [0, 0, None]}

        # initialize start node with h(n) of 0 as it is not necessary to calculate
        heapq.heappush(pq, (0, startNode))

         
        while len(pq) > 0:
            current_node = heapq.heappop(pq)[1]

            # not sure why this is needed
            if current_node not in node_dict:
                node_dict[current_node] = [0, 0, None]

            #  checks if we reached the end, which is when theres no node selector to get next node
            #  notice it will only consider the found goal
            #  if it has the lowest cost in the priority queue
            if current_node.selector == None:
                break

            #  check all of the node's children
            for child_node in current_node.selector.linkedNodes:
                if child_node not in node_dict:
                    node_dict[child_node] = [0, 0, None]

                    node_dict[child_node][0] += 1  # increment the g(n) value

                    node_dict[child_node][1] = self.get_heuristic(child_node, AGENT_TYPE)
                    
                    # f(n) = g(n) + h(n)
                    total_cost = node_dict[child_node][0] + node_dict[child_node][1]

                    # checking if node is already in pq
                    in_pq = False
                    for i in range(0, len(pq)):
                        if child_node == pq[i][1]: # pq[i][1] is the node entry
                            in_pq = True
                            # checking if it  has worse fitness than current_node
                            if pq[i][0] > total_cost:
                                #set parent
                                node_dict[child_node][2] = current_node
                                pq[i] = (total_cost, child_node)
                                heapq.heapify(pq)

                    if not in_pq:
                        node_dict[child_node][2] = current_node # set parent
                        heapq.heappush(pq, (total_cost, child_node))

        return self.traverse_nodes(current_node, node_dict, startNode) #todo return optimal node that follows the starting node
        

    
    def traverse_nodes(self, current_node, node_dict, startNode):
        
        if node_dict[current_node][2] == startNode: # at the node to be returned
            return current_node
        else:    
            return self.traverse_nodes(node_dict[current_node][2], node_dict, startNode)

    

    def get_heuristic(self, node, AGENT_TYPE):

        heuristic_value = node.cumulativeDPS + node.cumulativeHP + float(len(node.nearbyTowers))

        if AGENT_TYPE == "FLYING_AGENT":

            if node.enemyPref == node.EnemyPref.AntiAir:
                heuristic_value *= 2
            else:
                heuristic_value /= 2

        else:

            if node.enemyPref == node.EnemyPref.AntiGround:
                heuristic_value *= 2
            else:
                heuristic_value /= 2

        return heuristic_value
    