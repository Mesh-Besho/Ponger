import pyray as p
from wall import wall
from chatgpt import Line

from entities.entity import entity
#import ___

#import ___

class winzone(wall):
    def __init__(self, points:list):
        parent = super()
        parent.__init__()
        self.vertices = points
        self.colour = p.Color(0, 148, 255, 255)

    def collides_with(self, d, l)->bool:
        ls = self.get_lines()
        better_l = p.vector2_add(d, l)
        ball_line = Line(l, better_l)

        for l in ls:
            if p.check_collision_lines(l.a, l.b, ball_line.a, ball_line.b, None):
                return True
            
        return False
            