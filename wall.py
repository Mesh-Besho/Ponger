import pyray as p
from entities.entity import entity
class wall(entity):
    def __init__(self):
        self.vertices = []
        self.colour = p.BLACK
    
    #def become_a_rectangle(self):
    #    TLcorner = p.Vector2(0, 0)
    #    TRcorner = p.Vector2(180, 0)
    #    BLcorner = p.Vector2(0, 200)
    #    BRcorner = p.Vector2(180, 200)
    #    self.vertices = [TLcorner, BLcorner, BRcorner, TRcorner]