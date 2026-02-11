#/entities/powerup.py
from entities.sprite import sprite
class powerup(sprite):
    #/entities/powerup.py[class{"powerup", inherit{entity}}]
    def __init__(self, pos, t_name):
        #/entities/powerup.py[class{"powerup"}(function{"__init__", paramaters{self, pos}})]
        super().__init__(t_name)
        self.set_location(pos)
        self.set_size_fits(16, 16)
        self.set_origin_center()
        #\function
    #/class
#\file