import pyray as p
from entities.entity import entity
from doers.doer import doer
from entities.laser import laser

class laser_spinner(doer):
    def __init__(self, speed:float = 0.1):
        super().__init__()
        self.speed = speed

    def do(self, entity:laser, dt:float):
        howmuch = self.speed * dt
        entity.direction = p.vector2_rotate(entity.direction, howmuch)

   
