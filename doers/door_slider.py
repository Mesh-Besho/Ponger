import pyray as p
from entities.entity import entity
from doers.doer import doer
from entities.door import door

class door_slider(doer):
    def __init__(self, speed:float = 0.1):
        super().__init__()
        self.speed = speed

    def do(self, entity:entity, dt:float):
        self.move_door(entity)

    def move_door(self, door:door):
        p = door.get_location()
        p.y += self.speed
        door.set_location(p)
                          

        
