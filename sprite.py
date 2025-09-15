#i@:
import pyray as p#donk
from entity import entity#donk

#c@:
class sprite(entity):
    def __init__(self, png:p.Texture2D):
        self.colour = p.WHITE
        self.png = png
        self.W = png.width
        self.H = png.height

    def draw(self):
        source = p.Rectangle(0, 0, self.png.width, self.png.height)
        destination = p.Rectangle(self.X, self.Y, self.W, self.H)
        p.draw_texture_pro(self.png, source, destination, p.vector2_zero(), 0.0, self.colour)
        