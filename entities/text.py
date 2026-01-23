from entities.entity import entity
import pyray as p
class text(entity):
    def __init__(self, text:str, X:int, Y:int, size_pt:int, color:p.Color):
        super().__init__()
        self.text = text
        self.X = X
        self.Y = Y
        self.size = size_pt
        self.colour = color

    def draw(self):
        super().draw()
        p.draw_text(self.text, int(self.X), int(self.Y), self.size, self.colour)

        