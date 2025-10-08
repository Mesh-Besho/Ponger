import pyray as p
from entities.entity import entity
from doers.doer import doer
from entities.door import door

class mover(doer):
    def __init__(self, to_x:int, to_y:int, speed:float = 0.1):
        super().__init__()
        self.speed = speed
        self.to_x = to_x
        self.to_y = to_y

    def do(self, entity:entity, dt:float):
        p = entity.get_location()

        dx = self.to_x - p.x
        dy = self.to_y - p.y

        D_ALLOWED = 1.0

        x_done = False
        y_done = False

        if dx > D_ALLOWED:
            p.x += self.speed * dt

        elif dx < -D_ALLOWED:
            p.x -= self.speed * dt

        else:
            x_done = True

        if dy > D_ALLOWED:
            p.y += self.speed * dt

        elif dy < -D_ALLOWED:
            p.y -= self.speed * dt

        else:
            y_done = True

        entity.set_location(p)

        if x_done and y_done:
            self.done = True

   
