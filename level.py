"""
== START OF PLAN ==
#1 lasers
1. we will load their start point and angle. [START:0, ANGLE:0]
2. we will also load their properties [PROPERTIES:1]
3. we will have to draw them []
4. we will detect when we hit them and deal damage when we do []
[1. PROPERTIES]
1. damage
2. colour
#2 health
1. things can hurt you and it will show how much you can take [THINGS:1]
2. if you run out of health you will die []
[1. THINGS]
1. !lasers
== END OF PLAN ==
"""
from entities.powerup import powerup
from errors import WheresTheMusicError
from errors.UnknownObjectTypeError import UnknownObjectTypeError
from entities.laser import laser
import json
import pyray as p


from entities.key import key
from wall import wall
from entities.door import door
from winzone import winzone
from portal import portal
from entities.mouse_magnet_powerup import mouse_magnet_powerup
from entity_manager import entity_manager
import info
import errors.DonkeyDonutAteTheSpawnPointError as DDATSPerror
from entities.trigger import trigger
#from doers.blabla import blabla

from entities.entity import entity


class level(entity):#gjshugw
    def __init__(self):
        #donk
        super().__init__()
        self.walls = []
        self.portals = []
        self.doors = []
        self.winzones = []
        self.objects = []
        self.lasers = []
        self.triggers = []

    

    def load(self, filename):
        f = open(filename)
        the_text = f.read()
        the_json = json.loads(the_text)

        BCS = the_json.get("background_colour", "ghehujGUK")
        self.BC = self.convort(BCS)

        self.ball_spawn = self.load_vertex(the_json.get("spawn_point", DDATSPerror.DonkeyDonutAteTheSpawnPointError(filename[7:])))

        self.song = the_json.get("song", "Please crash")
#        if self.song == "Please crash":
#            raise WheresTheMusicError.WheresTheMusicError(filename[8])
        
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

        lasers = the_json.get("lasers", [])
        for laser in lasers:
            woof = self.load_laser(laser)
            self.lasers.append(woof)
        triggers = the_json.get("triggers", [])
        for trigger in triggers:
            adhd = self.load_trigger(trigger)
            self.triggers.append(adhd)

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

        donkey = DooR.get("doers", [])
        for doer_key in donkey:
            doer = donkey[doer_key]
            sheep = self.load_doer(doer)
            cow.doers[doer_key] = sheep

        salmon = DooR.get("events", {})
        for event_key in salmon:
            whale = salmon[event_key]
            cow.events[event_key] = whale
        
        

        cow.locked = DooR.get("locked", None)

        return cow
   
    def load_doer(self, DoeR):
        if DoeR["type"] == "mover":
            return self.load_mover(DoeR)
        if DoeR["type"] == "door_spinner":
            return self.load_door_spinner(DoeR)
        if DoeR["type"] == "door_wobbler":
            return self.load_door_wobbler(DoeR)
        else:
            return "Please crash"
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
    
    def load_door_wobbler(self, WobbleR):
        from doers.door_wobbler import door_wobbler
        jfy = door_wobbler()
        return jfy
    
    def load_vertex(self, vertex):
        if isinstance(vertex, DDATSPerror.DonkeyDonutAteTheSpawnPointError):
            raise vertex
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
        sheep = key(pos, TEXTure)
        return sheep

    def load_object(self, ObjecT):
        key_id = ObjecT["obj_id"]
        type = ObjecT["type"]
        if type == "key":
            sheep = self.load_key(ObjecT)
        elif type == "mouseMagnetPowerup":
            sheep = self.load_mouse_magnet_powerup(ObjecT)
        else:
            raise UnknownObjectTypeError(type)
        sheep.obj_id = key_id

        return sheep

    def load_mouse_magnet_powerup(self, ObjecT):
        pos = self.load_vertex(ObjecT["pos"])
        sheep = mouse_magnet_powerup(pos)
        return sheep
    
    def load_laser(self, LaseR):
        start = self.load_vertex(LaseR["start"]) 
        direction = self.load_vertex(LaseR["direction"])
        damage = float(LaseR.get("damage", 99))
        colour = self.convort(LaseR.get("colour", "RED"))
        jfy = laser(start, direction, damage, colour)
        return jfy
    
    def load_trigger(self, TriggeR):#Well Done
        vertices = []
        for vertex in TriggeR["vertices"]:
            pig_vec2 = self.load_vertex(vertex)
            vertices.append(pig_vec2)
        enter_script = TriggeR.get("enter", "")
        exit_script = TriggeR.get("exit", "")
        scripts = self.sort_out_triggers(enter_script, exit_script)
        adhd = trigger(vertices, scripts)
        return adhd

    def sort_out_triggers(self, enter, exit):
        return [f"levels/scripts/{enter}.ppy", f"levels/scripts/{exit}.ppy"]    

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

        for trigger in self.triggers:
            self.draw_polygon(trigger.vertices, p.ORANGE)

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