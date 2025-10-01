import json
import pyray as p


from wall import wall
from entities.door import door

from entities.entity import entity



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
        BCS = the_json["background_colour"]
        self.BC = self.convort(BCS)
        doors = the_json["doors"]

        for shape in shapes:
            pig = self.load_wall(shape)
            self.walls.append(pig)

        for door in doors:
            cow = self.load_door(door)
            self.doors.append(cow)

    def load_wall(self, shape):
        pig = wall(self.game)
          
        for vertex in shape["Vertices"]:
            pig_vec2 = self.load_vertex(vertex)
            pig.vertices.append(pig_vec2)

        colour = shape["Color"]
        _colour = self.convort(colour)
        pig.colour = _colour  

        return pig
    
    def load_door(self, DooR):
        cow = door()

        cow.set_location(self.load_vertex(DooR["hinge"]))
        
        for banana in DooR["walls"]:
            surface = self.load_wall(banana)
            cow.surfaces.append(surface)

        return cow

    def load_vertex(self, vertex):
        pig_vec2 = p.Vector2(float(vertex["X"]), float(vertex["Y"]))
        return pig_vec2

    def convort(self, str:str):
        if str == "RED":
            return p.RED
        elif str == "BROWN":
            return p.BROWN
        elif str == "WHITE":
            return p.WHITE
        elif str == "BLACK":
            return p.BLACK
        elif str == "GOLD":
            return p.GOLD        
        else:
            return p.LIME

    def update(self, dt):
        for x  in self.doors:
            x.update(dt)
       
   
    def draw(self):
        p.clear_background(self.BC)    
        x = 0

        for wall in self.walls:
            self.draw_wall(wall, x)
            x += 1
        
        for door in self.doors:
            self.draw_door(door)
        
        


    def draw_wall(self, wall:wall, x:int):
        l = len(wall.vertices)
        for n in range(1, l-1):
            #colours = [p.RED, p.BLUE, p.BEIGE, p.YELLOW, p.BLACK, p.BROWN, p.GREEN, p.GOLD, p.GRAY]
            a = wall.vertices[0]
            b = wall.vertices[n]
            c = wall.vertices[n+1]
            colour = wall.colour

            #colour = colours[x % len(colours)]
            p.draw_triangle(a, b, c, colour)
    
    def draw_door(self, door:door):
        for x in door.surfaces:
            moved_x = door.move_wall(x)
            self.draw_wall(moved_x, 67)
        self.draw_hinge(door.get_location(), door.colour)

    def draw_hinge(self, hinge, colour):
        p.draw_circle(int(hinge.x), int(hinge.y), 1.0, colour)