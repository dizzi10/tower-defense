class FindOptimalNode():
    """
    Finds the optimal node with A* algorithm using a heuristic function h(n)
    """

    def __init__(self):
        pass
    def getOptimalNode(self, startNode):

        # these are all the useful atttributes of the nodes you will ned for the heuristic
        # there are two types of agents calculate optimal node once for flying agent and once for attacking agent,
        
        childNodes = startNode.selector.linkedNodes # shows how to get list of children
        aChildNode = startNode.selector.linkedNodes[0]  # shows to get a child node

        return (
        childNodes,
        aChildNode, 
        len(aChildNode.nearbyTowers), # use for heuristic
        aChildNode.cumulativeHP, #  use for heuristic
        aChildNode.cumulativeDPS,  #  use for heuristic
        aChildNode.enemyPref,  #  use for heuristic, it either "AntiAir" or "AntiGround", run A* twice once for each type of enemy pref
        aChildNode.transform.position)  # position of node
        