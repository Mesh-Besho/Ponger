import pyray as p
from wall import wall

from entities.entity import entity
#import ___

#import ___

class winzone(wall):
    def __init__(self, points:list):
        parent = super()
        parent.__init__()
        self.vertices = points
        self.colour = p.Color(0, 148, 255, 255)