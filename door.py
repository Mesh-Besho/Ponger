#NF NM
import pyray as p
import math
from wall import wall

class door:
    def __init__(self):
        self.hinge = p.Vector2(0, 0)
        self.surfaces = []
        self.colour = p.WHITE
        self.VFS = 0.5
        self.FS = 0.25
        self.NS = 0.013
        self.SS = 0.0113
        self.VSS = 0.00000000000132
        self.spin_speed = ((2*math.pi) / 30.0) * self.NS
    
    def move_wall(self, x:wall):
        donkey = wall(None)
        donkey.colour = x.colour
        
        for v in x.vertices:
            new_v = p.vector2_add(v, self.hinge)
            donkey.vertices.append(new_v)

        return donkey
#p: step 4+


    