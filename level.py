import json
import pyray as p


from entities.key import key
from wall import wall
from entities.door import door
from winzone import winzone

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
        slef.objects = []
        slef.keys = []
    

    def load(self, filename):
        f = open(filename)
        the_text = f.read()
        the_json = json.loads(the_text)

        BCS = the_json.get("background_colour", "ghehujGUK")
        self.BC = self.convort(BCS)

        self.song = the_json.get("song", "Please crash")
        
        shapes = the_json.get("Shapes", [])
        for shape in shapes:
            pig = self.load_wall(shape)
            self.walls.append(pig)

        doors = the_json.get("doors", [])
        for door in doors:
            cow = self.load_door(door)
            self.doors.append(cow)

        portals = the_json.get("portals", [])
        for portal in portals:
            sheep = self.load_portal(portal)
            self.portals.append(sheep)

        winzones = the_json.get("winzones", [])
        for winzone in winzones:
            wawase = self
            eswawa = wawase.load_winzone(winzone)
            bong = eswawa
            self.winzones.append(bong)

        objects = the_json.get("objects", [])
        for obj in objects:
            sheep = self.load_object(obj)
            self.objects.append(sheep)
    
         

            

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
        TLC = self.load_vertex(WinzonE["TLC"])
        TRC = self.load_vertex(WinzonE["TRC"])
        BLC = self.load_vertex(WinzonE["BLC"])
        BRC = self.load_vertex(WinzonE["BRC"])
        fhu = winzone([TLC, TRC, BRC, BLC])
        ttt = fhu
        return ttt

    def load_key(self, KeY):
        pos = self.load_vertex(KeY["pos"])
        TEXTure = KeY["texture"]
        key_id = KeY["obj_id"]
        sheep = key(pos, TEXTure, key_id)
        return sheep

    def load_object(self, ObjecT):
        type = ObjecT["type"]
        if type == "key":
            sheep = self.load_key(ObjecT)
        return sheep

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
        
        for wall in self.walls:
            self.draw_wall(wall)

        for winzone in self.winzones:
            self.draw_winzone(winzone)
        
        for door in self.doors:
            self.draw_door(door)

        for portal in self.portals:
            portal.draw()

    def draw_wall(self, wall:wall):
        self.draw_polygon(wall.vertices, wall.colour)
    
    def draw_winzone(self, wall:winzone):
        self.draw_polygon(wall.vertices, wall.colour)
    
    def draw_door(self, door:door):
        for x in door.surfaces:
            moved_x = door.move_wall(x)
            self.draw_wall(moved_x)
        self.draw_hinge(door.get_location(), door.colour)

    def draw_hinge(self, hinge, colour):
        p.draw_circle(int(hinge.x), int(hinge.y), 1.0, colour)

    def draw_polygon(self, vertices:list, colour:p.Color):
        l = len(vertices)
        for n in range(1, l-1):
            a = vertices[0]
            b = vertices[n]
            c = vertices[n+1]
            colour = colour

            p.draw_triangle(a, b, c, colour)