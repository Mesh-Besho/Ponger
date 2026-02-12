import pyray as p
from entities.sprite import sprite
class key(sprite):
    def __init__(self, pos, texture:str):
        super().__init__(texture)
        self.set_size_fits(15, 15)
        self.set_origin_center()
        self.colour = p.YELLOW
        self.set_location(pos)
        self.obj_id = ""
    
    def when_collected(self, scene):
        pass