#c=1158223
from game import game
import pyray as p
from entities.entity import entity
class p_camera(entity):
    def __init__(self, level):
        self.level = level
        self.me = p.Camera2D(p.Vector2(p.get_screen_width()/2, p.get_screen_height()/2), p.Vector2(0, 0), 0, 1)
        self.follow_obj = None
    
    def update(self, dt):
        if not self.follow_obj is None:
            self.move_to(self.follow_obj.get_location())
    
    def follow(self, follow_obj:entity|None):
        self.follow_obj = follow_obj

    def move_by(self, x, y):
        change = p.Vector2(x, y)
        old = self.get_location()
        new = p.vector2_add(old, change)
        self.move_to(new.x, new.y)

    def move_to(self, x, y):
        new = p.Vector2(x, y)
        self.set_location(new)
        self.me.target = new