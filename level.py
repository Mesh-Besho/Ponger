import json
import pyray as p


from wall import wall
from entities.door import door

#from doers.blabla import blabla

from entities.entity import entity
from portal import portal


class level(entity):#gjshugw
    def __init__(slef):
        #donk
        super().__init__()
        slef.walls = []
        slef.portals = []
        slef.doors = []
        slef.winzones = []
    

    def load(self, filename):
        f = open(filename)
        the_text = f.read()
        the_json = json.loads(the_text)

        BCS = the_json["background_colour"]
        self.BC = self.convort(BCS)
        
        shapes = the_json["Shapes"]
        for shape in shapes:
            pig = self.load_wall(shape)
            self.walls.append(pig)

        doors = the_json["doors"]
        for door in doors:
            cow = self.load_door(door)
            self.doors.append(cow)
        
        portals = the_json["portals"]
        for portal in portals:
            sheep = self.load_portal(portal)
            self.portals.append(sheep)
        
        winzones = the_json["winzones"]
        for winzone in winzones:
            wawase = self
            eswawa = wawase.load_winzone(winzone)
            bong = eswawa
            self.winzones.append(bong)

    def load_wall(self, shape):
        pig = wall()
          
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

        donkey = DooR["doers"]
        for doer_key in donkey:
            doer = donkey[doer_key]
            sheep = self.load_doer(doer)
            cow.doers[doer_key] = sheep

        salmon = DooR["events"]
        for event_key in salmon:
            whale = salmon[event_key]
            cow.events[event_key] = whale

        return cow
   
    def load_doer(self, DoeR):
        if DoeR["type"] == "mover":
            return self.load_mover(DoeR)
        if DoeR["type"] == "door_spinner":
            return self.load_door_spinner(DoeR)
        #if DoeR[type] == "blabla"
        #   return self.load_blabla(ghu, yje, hdyt, gst)
    
    def load_mover(self, MoveR):
        from doers.mover import mover
        to_x = MoveR["to_x"]
        to_y = MoveR["to_y"]
        speed = float(MoveR["speed"])
        jfy = mover(to_x, to_y, speed)
        return jfy
    
    def load_door_spinner(self, SpinneR):
        from doers.door_spinner import door_spinner
        speed = float(SpinneR["speed"])
        angle = float(SpinneR["angle"])
        jfy = door_spinner(angle, speed)
        return jfy
    
    def load_vertex(self, vertex):
        pig_vec2 = p.Vector2(float(vertex["X"]), float(vertex["Y"]))
        return pig_vec2
    
    def load_portal(self, PortaL):
        pos = self.load_vertex(PortaL["pos"])
        destination = PortaL["destination"]
        name = PortaL["name"]
        sheep = portal(pos, PortaL["range"], destination, name)
        return sheep
    
    def load_winzone(self, WinzonE):
        TLC = WinzonE["TLC"]
        TRC = WinzonE["TRC"]
        BLC = WinzonE["BLC"]
        BRC = WinzonE["BRC"]
        fhu = winzone(TLC, TRC, BLC, BRC)
        ttt = fhu
        return ttt
    
    def find_portal_by_name(self, name:str):
        for x in self.portals:
            if x.name == name:
                return x
        return None


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
        for x in self.doors:
            x.update(dt)
        
       
    def draw(self):
        p.clear_background(self.BC)    
        x = 0

        for wall in self.walls:
            self.draw_wall(wall, x)
            x += 1
        
        for door in self.doors:
            self.draw_door(door)

        for portal in self.portals:
            portal.draw()

    def draw_wall(self, wall:wall, x:int):
        l = len(wall.vertices)
        for n in range(1, l-1):
            a = wall.vertices[0]
            b = wall.vertices[n]
            c = wall.vertices[n+1]
            colour = wall.colour

            p.draw_triangle(a, b, c, colour)
    
    def draw_door(self, door:door):
        for x in door.surfaces:
            moved_x = door.move_wall(x)
            self.draw_wall(moved_x, 67)
        self.draw_hinge(door.get_location(), door.colour)

    def draw_hinge(self, hinge, colour):
        p.draw_circle(int(hinge.x), int(hinge.y), 1.0, colour)