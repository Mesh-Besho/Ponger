import pyray as p
from doers.doer import doer

class entity:
    from game import game
    X = 0
    Y = 0
    W = 0
    H = 0

    def __init__(self, game:game):
        self.game = game
        self.current_do = doer()
        self.next_do = doer()
        
    def hit_test(self, coordinates:p.Vector2):
        r = self.get_bounds()
        
        if p.check_collision_point_rec(coordinates, r):
            return True
        else:
            return False
        
    def get_location(self):
        XY = p.Vector2(self.X, self.Y)
        return XY
    def set_location(self, XY:p.Vector2):
        self.X = XY.x
        self.Y = XY.y

    
    def get_bounds(self):
        hello = p.Rectangle(float(self.X), float(self.Y), float(self.W), float(self.H))
        return hello
    
    def draw(self):
        pass

    def update(self, dt):
        if not self.current_do is None:
            self.current_do.do(self, dt)
        