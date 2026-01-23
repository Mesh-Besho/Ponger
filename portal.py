#/portal.py

import pyray as p

from entities.sprite import sprite

class portal(sprite):
    #/portal.py[class portal]
    def __init__(self, pos, rang, dest, name):
        #/portal.py[class portal.function __init__]
        self.radius = rang
        self.set_location(pos)
        t = "ball.png"
        super().__init__(t)
        self.W = 10
        self.H = 10
        self.destination = dest
        self.name = name
        #function\
    #class\
#file\