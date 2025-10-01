import pyray as p
from entities.entity import entity
from doers.doer import doer
from entities.door import door

class door_spinner(doer):
    def __init__(self, speed:float = 0.01):
        super().__init__()
        self.speed = speed

    def do(self, entity:entity, dt:float):
        self.turn_door(entity)

    def turn_door(self, door:door):
        for s in door.surfaces:
            for v in s.vertices:
                nv = self.actually_turn_door(v, door)
                v.x = nv.x
                v.y = nv.y
                            
    def actually_turn_door(self, v, d:door):
        nv = p.vector2_rotate(v, self.speed)
        return nv
        
