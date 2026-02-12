import pyray as p
from entities.entity import entity
import chatgpt as bounce_code
class wall(entity):
    def __init__(self, owner=None):
        self.vertices = []
        self.colour = p.BLACK
        self.owner = owner

    
    #def become_a_rectangle(self):
    #    TLcorner = p.Vector2(0, 0)
    #    TRcorner = p.Vector2(180, 0)
    #    BLcorner = p.Vector2(0, 200)
    #    BRcorner = p.Vector2(180, 200)
    #    self.vertices = [TLcorner, BLcorner, BRcorner, TRcorner]

    def get_lines(self):
        ls = []

        for x in range(len(self.vertices)):
            s = self.vertices[x]
            e = self.vertices[(x + 1) % len(self.vertices)]

            #bounce_code
            l = bounce_code.Line(s, e, self.owner)
            ls.append(l)
        
        return ls
            