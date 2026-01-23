import pyray as p
from entities.entity import entity
from doers.doer import doer
from entities.sprite import sprite
import random

class text_boinger(doer):
    def __init__(self):
        super().__init__()
        self.speed_y = -200.0
        self.speed_x = random.uniform(-50.0, 50.0)

    def do(self, entity:sprite, dt:float):
        self.speed_y += 400.0 * dt
        entity.Y += self.speed_y * dt

        entity.X += self.speed_x * dt

        if entity.Y > 1000:
            self.done = True

   
