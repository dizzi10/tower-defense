class Node:
    def __init__(self, position, parent):
        self.position = position
        self.parent = parent
        self.gCost = 0  # distance to start
        self.hCost = 0  # distance to goal
        self.fCost = 0  # total cost

    # equal nodes
    def __eq__(self, other):
        return self.position == other.position

    # less than nodes
    def __lt__(self, other):
         return self.f < other.f

class FindOptimalNode():
    """
    Finds the optimal node with A* algorithm using a heuristic function h(n)
    """

    def getOptimalNode(grid, start, end):
        open = []  # list of nodes to search
        closed = []  # nodes that were already processed

        startNode = Node(start, None)
        endNode = Node(end, None)

        open.append(startNode)

        while len(open) > 0:
            open.sort()
            # node with lowest cost
            currentNode = open.pop(0)
            closed.append(currentNode)

            # check if goal was reached; if so return the reversed path
            if currentNode == endNode:
                path = []
                while currentNode != startNode:
                    path.append(currentNode.position)
                    currentNode = currentNode.parent
                return path[::1]

            # get neighboring nodes
            (x, y) = currentNode.position
            neighbors = [(x-1, y), (x+1, y), (x, y-1), (x, y+1)]

            for i in neighbors:
                neighbor = Node(next, currentNode)

                #manhattan distance
                neighbor.g = abs(neighbor.position[0] - startNode.position[0]) + abs(neighbor.position[1] - startNode.position[1])
                neighbor.h = abs(neighbor.position[0] - endNode.position[0]) + abs(neighbor.position[1] - endNode.position[1])
                neighbor.f = neighbor.g + neighbor.h