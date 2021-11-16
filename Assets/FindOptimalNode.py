class FindOptimalNode():
    """
    Finds the optimal node with A* algorithm using a heuristic function h(n)
    """

    def __init__(self):
        pass
    def getOptimalNode(self, linkedNodes):
        return (linkedNodes[0], linkedNodes[0].weight)