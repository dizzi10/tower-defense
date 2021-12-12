import math
class FindOptimalNode():
    """
    Finds the optimal node with A* algorithm using a heuristic function h(n)
    """



    def __init__(self):
        # these are all the useful atttributes of the nodes you will ned for the heuristic
        # there are two types of agents calculate optimal node once for flying agent and once for attacking agent,

        self.childNodes = startNode.selector.linkedNodes  # shows how to get list of children
        self.aChildNode = startNode.selector.linkedNodes[0]  # shows to get a child node
        self.g = 0
        self.h = 0
        self.f = 0

        return (
            childNodes,
            aChildNode,
            len(aChildNode.nearbyTowers),  # use for heuristic
            aChildNode.cumulativeHP,  # use for heuristic
            aChildNode.cumulativeDPS,  # use for heuristic
            aChildNode.enemyPref,
            # use for heuristic, it either "AntiAir" or "AntiGround", run A* twice once for each type of enemy pref
            aChildNode.transform.position)  # position of node

    # def f_AntiAir(self):
    #     h = math.abs(aChildNode.cumulativeDPS - aChildNode.cumulativeHP)
    #     return h + len(aChildNode.nearbyTowers) + aChildNode.enemyPref.AntiAir

    # def f_AntiGround(self):
    #     h = math.abs(aChildNode.cumulativeDPS - aChildNode.cumulativeHP)
    #     return h + len(aChildNode.nearbyTowers) + aChildNode.enemyPref.AntiGround

    def getOptimalNode_AntiAir(self, linkedNodes):
        """Returns a list of tuples as a path """

        start_node = linkedNodes[0]
        start_node.g = start_node.h = start_node.f = 0
        end_node = linkedNodes[-1]
        end_node.g = end_node.h = end_node.f = 0

        # Initialize both open and closed list
        open_list = []
        closed_list = []

        # Add the start node
        open_list.append(start_node)

        # Loop until you find the end
        while len(open_list) > 0:

            # Get the current node
            current_node = open_list[0]
            current_index = 0
            for index, item in enumerate(open_list):
                if item.f < current_node.f:
                    current_node = item
                    current_index = index

            # Pop current off open list, add to closed list
            open_list.pop(current_index)
            closed_list.append(current_node)

            # Found the goal
            if current_node == end_node:
                path = []
                current = current_node
                while current is not None:
                    path.append(current.position)
                    current = current.parent
                return path[::-1]  # Return reversed path

            # Generate children
            children = self.childNodes

            # Loop through children
            for child in children:

                # Child is on the closed list
                for closed_child in closed_list:
                    if child == closed_child:
                        continue

                # Create the f, g, and h values for heuristics
                child.g = len(self.aChildNode.nearbyTowers) + self.aChildNode.enemyPref.AntiAir
                child.h = math.fabs(self.aChildNode.cumulativeDPS - self.aChildNode.cumulativeHP)
                child.f = child.g + child.h

                # Child is already in the open list
                for open_node in open_list:
                    if child == open_node and child.g > open_node.g:
                        continue

                # Add the child to the open list
                open_list.append(child)

    def getOptimalNode_AntiGround(self, linkedNodes):
        """Returns a list of tuples as a path """

        start_node = linkedNodes[0]
        start_node.g = start_node.h = start_node.f = 0
        end_node = linkedNodes[-1]
        end_node.g = end_node.h = end_node.f = 0

        # Initialize both open and closed list
        open_list = []
        closed_list = []

        # Add the start node
        open_list.append(start_node)

        # Loop until you find the end
        while len(open_list) > 0:

            # Get the current node
            current_node = open_list[0]
            current_index = 0
            for index, item in enumerate(open_list):
                if item.f < current_node.f:
                    current_node = item
                    current_index = index

            # Pop current off open list, add to closed list
            open_list.pop(current_index)
            closed_list.append(current_node)

            # Found the goal
            if current_node == end_node:
                path = []
                current = current_node
                while current is not None:
                    path.append(current.position)
                    current = current.parent
                return path[::-1]  # Return reversed path

            # Generate children
            children = self.childNodes

            # Loop through children
            for child in children:

                # Child is on the closed list
                for closed_child in closed_list:
                    if child == closed_child:
                        continue

                # Create the f, g, and h values for heuristics
                child.g = len(self.aChildNode.nearbyTowers) + self.aChildNode.enemyPref.AntiGround
                child.h = math.fabs(self.aChildNode.cumulativeDPS - self.aChildNode.cumulativeHP)
                child.f = child.g + child.h

                # Child is already in the open list
                for open_node in open_list:
                    if child == open_node and child.g > open_node.g:
                        continue

                # Add the child to the open list
                open_list.append(child)


