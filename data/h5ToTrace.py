import glm 
import h5py
import argparse
import os
import sys
from enum import Enum

class NodeType(Enum):
    SOMA = 1
    NEURITES = 2

class Node:
    def __init__(self, pos: glm.vec3, radius: float, type : int = 2, parentId : int = -1):
        self.pos = pos
        self.radius = radius
        if (type == 1):
            self.type = NodeType.SOMA
        else: 
            self.type = NodeType.NEURITES
        self.parentId = parentId
        self.parent : Node | None = None
        self.children : list[Node] = []


def loadH5(file:str) -> list[list[Node]]:
    sections = []
    with h5py.File(file, 'r') as f:        
        points = f['points']
        numPoints = len(points)
        secs = f['structure']
        numSecs = len(secs)

        nodes = []
        for pt in points:
            p = glm.vec3(pt[0], pt[1], pt[2])
            r = pt[3]*0.5
            nodes.append(Node(p,r))
                
        def getLastNode(secId: int):
            if secId == numSecs-1:
                return numPoints-1
            return secs[secId + 1][0]-1
         
        for i, sec in enumerate(secs):
            if sec[1] > 2:
                firstNode = sec[0]
                lastNode = getLastNode(i)
                
                section = []
                for i in range(firstNode, lastNode+1):
                    section.append(nodes[i])
                sections.append(section)                    
    return sections


def loadSWC(file:str) -> list[list[Node]]:
    sections = []

    nodes : list[Node]= []
    with open(file, 'r') as f:
         for line in f.readlines():
            params = line.split()
            if (len(params) < 7):
                continue
            if (params[0][0] == '#'):
                continue
            type = int(params[1])
            x = float(params[2])
            y = float(params[3])
            z = float(params[4])
            r = float(params[5])
            parent = int(params[6])
            nodes.append(Node(glm.vec3(x,y,z), r, type, parent))

    for n in nodes:
        if n.parentId > 0:
            parentNode = nodes[n.parentId-1]
            if parentNode.type == NodeType.NEURITES:
                n.parent = parentNode
                parentNode.children.append(n)

    beginnings : list[Node] = []
    for n in nodes:
        if n.type == NodeType.NEURITES:
            if n.parent == None or len(n.children) > 1:
                beginnings.append(n)


    for start in beginnings:
        for child in start.children:
            section = [start]
            while(1):
                section.append(child)
                if len(child.children) == 1:
                    child = child.children[0]
                else:
                    sections.append(section)
                    break

    return sections


def writeTrace(file:str, sections: list[list[Node]]):
    sectionsIds = []
    nodes = []

    for section in sections:
        firstNode = len(nodes)
        for n in section:
            nodes.append(n)
        lastNode = len(nodes)-1
        sectionsIds.append((firstNode, lastNode))

    with open(file, 'w') as f:
        lines = []
        for n in nodes:
            lines.append(f'n {n.pos.x} {n.pos.y} {n.pos.z} {n.radius}\n')

        for sec in sectionsIds:
            lines.append(f's {sec[0]} {sec[1]}\n')
        f.writelines(lines)

    
if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("infiles", metavar="path", nargs="+", help="path to neuron moprholgy file")
    parser.add_argument("-d", type=str, default=".")
    args = parser.parse_args()


    folder = os.path.abspath(args.d)
    if not os.path.isdir(folder):
        folder = os.path.abspath(".")
    folder += "/"


    for infile in args.infiles:
        if not os.path.isfile(infile):
            print(f"ERROR: File {infile} doesn't exist")
            continue
        basename = os.path.basename(infile)

        sections = []
        if (basename[-3:] == ".h5"):
            basename = basename[:-3]
            sections = loadH5(infile)
        elif (basename[-4:] == ".swc"):
            basename = basename[:-4]
            sections = loadSWC(infile)
        else:
            print(f"ERROR: File {infile} is not morphology file[.swc|.h5]")
            continue
        outfile = folder + basename + ".trace"
        writeTrace(outfile, sections)
        print(f"File saved as {outfile}")

