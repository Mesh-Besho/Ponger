#/doers/door_wobbler.py
import math
import pyray as p

from doers.doer import doer
from entities.door import door

TOTAL_TIME = 0.75
STEPS = 6.0
SLICE_TIME = TOTAL_TIME / STEPS

class door_wobbler(doer):
    #
    def __init__(self):
        super().__init__()
        self.reset()

    def reset(self):
        super().reset()
        self.speed = (math.pi / 180.0) * 3.25
        self.step_num = 0
        self.step_time_left = 0.0

    def do(self, door, dt):
        self.step_time_left -= dt

        if self.step_time_left > 0.0:
            return
        
        self.step_num += 1
        self.step_time_left = SLICE_TIME

        if self.step_num == 1:
            self.turn_door(door, -self.speed / 2.0)
        elif self.step_num == 2:
            self.turn_door(door, self.speed)
        elif self.step_num == 3:
            self.turn_door(door, -self.speed)
        elif self.step_num == 4:
            self.turn_door(door, self.speed)
        elif self.step_num == 5:
            self.turn_door(door, -self.speed)
        else:
            self.turn_door(door, self.speed / 2.0)
            self.done = True    
    
    def turn_door(self, door:door, angle):
        for s in door.surfaces:
            for v in s.vertices:
                nv = self.actually_turn_door(v, angle)
                v.x = nv.x
                v.y = nv.y
                            
    def actually_turn_door(self, v, angle):
        nv = p.vector2_rotate(v, angle)
        return nv
        
    
