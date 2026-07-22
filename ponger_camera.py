#c=1158223
import pyray as p
from entities.entity import entity
class p_camera(entity):
    def __init__(self):
        self.me = p.Camera2D(p.Vector2(p.get_screen_width()/2, p.get_screen_height()/2), p.Vector2(0, 0), 0, 1)
        self.follow_obj = None
    
    def update(self, dt):
        if not self.follow_obj is None:
            self.move_to(self.follow_obj.get_location().x, self.follow_obj.get_location().y)
    
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

    def reset(self):
        self.me.offset = p.vector2_zero() 
        self.me.target = p.vector2_zero()
        self.me.rotation = 0.0
        self.me.zoom = 1.0
        