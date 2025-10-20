import pyray as p
import math
from entities.entity import entity
from doers.doer import doer
from entities.door import door

class door_spinner(doer):
    def __init__(self, angle:float, speed:float):
        super().__init__()
        self.speed = speed
        self.target_angle = angle

    def reset(self):
        self.now_angle = 0.0
        super().reset()

    def do(self, entity:entity, dt:float):
        self.turn_door(entity, dt)

    def turn_door(self, door:door, dt:float):
        for s in door.surfaces:
            for v in s.vertices:
                nv = self.actually_turn_door(v, door, dt)
                v.x = nv.x
                v.y = nv.y
        self.now_angle += self.speed * dt
        
        diff = math.fabs(self.now_angle - self.target_angle)

        if diff <= 0.1:
            self.done = True
                            
    def actually_turn_door(self, v, d:door, dt:float):
        nv = p.vector2_rotate(v, self.speed * dt)
        return nv
        
