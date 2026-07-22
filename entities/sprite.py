#i@:
import pyray as p#donk
from errors.ThisImageIsStupidError import ThisImageIsStupidError


from entities.entity import entity#donk
class sprite(entity):
    def __init__(self, png:str):
        super().__init__()
        self.colour = p.WHITE
        self.t = p.load_texture(png)
        if self.t.width == 0 or self.t.height == 0:
            raise ThisImageIsStupidError(png)
        self.W = self.t.width
        self.origin = p.Vector2(0, 0)
        self.H = self.t.height
        self.R = 0.0

    def set_size_fits(self, max_width, max_height):
        aspect_ratio = self.t.width / self.t.height
        if self.t.width > max_width:
            self.W = max_width
            self.H = max_width / aspect_ratio
        if self.H > max_height:
            self.H = max_height
            self.W = max_height * aspect_ratio

    def set_origin_center(self):
        self.origin = p.Vector2(self.W / 2, self.H / 2)

    def draw(self):
        source = p.Rectangle(0, 0, self.t.width, self.t.height)
        destination = p.Rectangle(self.X, self.Y, self.W, self.H)
        p.draw_texture_pro(self.t, source, destination, self.origin, self.R, self.colour)

    def am_I_touching(self, what):
        if isinstance(what, sprite):
            if p.check_collision_recs(p.Rectangle(self.X, self.Y, self.W, self.H),p.Rectangle(what.X, what.Y, what.W, what.H)):
                return True
        return False

class collectable(sprite):
#-----------------^^^^^^-------- FIX - Your problem, you fix. Ashley@meshbesho.games. Ashley@meshbesho.games refused. Alex@meshbesho.games did it. £0.50 of wages lost.
    def __init__(self, png:str):
        super().__init__(png)

    def when_collected(self, scene):
        pass
        