import json
import pyray as p

from wall import wall
from entity import entity

class level(entity):#gjshugw
    def __init__(slef, game):
        #donk
        super().__init__(game)
        slef.walls = []
        slef.pushables = []
        slef.doors = []

    def load(self, filename):
        f = open(filename)
        the_text = f.read()
        the_json = json.loads(the_text)

        shapes = the_json["Shapes"]

        for shape in shapes:
            pig = wall(self.game)
            for vertex in shape["Vertices"]:
                pig_vec2 = p.Vector2(float(vertex["X"]), float(vertex["Y"]))
                pig.vertices.append(pig_vec2)
            self.walls.append(pig)






    def draw_wall(self, wall:wall, x:int):
        l = len(wall.vertices)
        for n in range(1, l-1):
            colours = [p.RED, p.BLUE, p.BEIGE, p.YELLOW, p.BLACK, p.BROWN, p.GREEN, p.GOLD, p.GRAY]
            a = wall.vertices[0]
            b = wall.vertices[n]
            c = wall.vertices[n+1]
            colour = colours[x % len(colours)]
            p.draw_triangle(a, b, c, colour)


    
    def draw(self):
        x = 0
        for wall in self.walls:
            self.draw_wall(wall, x)
            x += 1
    