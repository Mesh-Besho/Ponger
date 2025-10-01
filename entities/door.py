#NF NM
import pyray as p
import math
from entities.entity import entity
from wall import wall

class door(entity):
    def __init__(self):
        super().__init__(self)
        self.surfaces = []
        self.colour = p.WHITE
        #5 speeds in LPS
        self.VFS = 0.5
        self.FS = 0.25
        self.NS = 0.013
        self.SS = 0.0113
        self.VSS = 0.00000000000132
        #turn them into RPF(normal speed)
        
        spin_speed = ((2*math.pi) / 30.0) * self.NS

        from doers.door_spinner import door_spinner
        self.current_do = door_spinner(spin_speed)
    
    def move_wall(self, x:wall):
        donkey = wall(None)
        donkey.colour = x.colour
        
        for v in x.vertices:
            new_v = p.vector2_add(v, self.get_location())
            donkey.vertices.append(new_v)

        return donkey
#p: step 4+


    