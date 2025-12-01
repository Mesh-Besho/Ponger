#i@:
import pyray as p

from entities.sprite import sprite
from level import level
from wall import wall
import chatgpt as bounce_code
import controls


import math
import maths

MOUSE_MAGNET_RANGE = 200.1
MOUSE_MAGNET_POWER = 5.1
CIRC_RADIUS = 10



#c@:
class ball(sprite):
    #c':
    def __init__(self, level:level):
        t = p.load_texture("ball.png")
        super().__init__(t)
        self.W = 10
        self.H = 10
        self.direction = p.Vector2(111, 110)
        self.level = level
        self.can_teleport = True
        
    
    def update(self, dt):
        #dt = dt / 10.0

        super().update(dt)
        #that's why we've got `dt`

        if p.is_mouse_button_down(p.MouseButton.MOUSE_BUTTON_LEFT):
            self.apply_magnet(dt, MOUSE_MAGNET_RANGE, MOUSE_MAGNET_POWER, controls.good_mouse_position, 1)

        elif p.is_mouse_button_down(p.MouseButton.MOUSE_BUTTON_RIGHT):
            self.apply_magnet(dt, MOUSE_MAGNET_RANGE, MOUSE_MAGNET_POWER, controls.good_mouse_position, -1)        

        
        self.move_and_bounce(dt)
        did_touch = False

        for x in self.level.portals:
            if p.check_collision_circles(self.get_location(), CIRC_RADIUS, x.get_location(), x.radius):
                did_touch = True
                if self.can_teleport:
                    self.teleport(x.destination)
                    self.can_teleport = False
        if not did_touch:
            self.can_teleport = True
                    
                
          
    def teleport(self, to_portal_string):
        to_portal = self.level.find_portal_by_name(to_portal_string)
        self.set_location(to_portal.get_location())


  
    def move_and_bounce(self, dt):
        #self.direction
        #dt
        #circle: Circle, velocity: pyray.Vector2, walls: list[Line], dt: float
        ls = []
        #ls

        walls = list(self.level.walls)
        for door in self.level.doors:
            for surface in door.surfaces:
                moved_wall = door.move_wall(surface)
                walls.append(moved_wall)

        for blocker in walls:
            for x in range(len(blocker.vertices)):
                s = blocker.vertices[x]
                e = blocker.vertices[(x + 1) % len(blocker.vertices)]
                #bounce_code
                l = bounce_code.Line(s, e)
                ls.append(l)
        circ = bounce_code.Circle(self.get_location(), CIRC_RADIUS)
        #circ

        #4/4
        self.direction = bounce_code.update_circle(circ, self.direction, ls, dt)
        self.set_location(circ.center)

        
    def apply_magnet(self, dt:float, range:float, power:float, position:p.Vector2, polarity:float):
        magnetism = self.calculate_magnetism(range, power, position, polarity)
        magnetism_per_frame = p.vector2_scale(magnetism, dt)
        self.direction = p.vector2_add(magnetism_per_frame, self.direction)

    def calculate_magnetism(self, range:float, power:float, position:p.Vector2, polarity:float):
        #self.direction
        location = self.get_location()
        v = p.Vector2(position.x - location.x, position.y - location.y)
        distance = p.vector2_length(v)

        actual_power = power * (range - distance)
        actual_power = self.minimise(actual_power, 0)
        direction = p.vector2_normalize(v)
        magnetism = p.vector2_scale(direction, polarity)
        magnetism = p.vector2_scale(magnetism, actual_power)
        return magnetism


    def minimise(self, var, minimum):
        if var < minimum:
            var = minimum
        return var        
    def maximise(self, var, maximum):
        if var < maximum:
            var = maximum
        return var        
        

    def draw(self):
        #super().draw()
        
        p.draw_circle_lines(int(self.X), int(self.Y), CIRC_RADIUS, p.BLACK)

        a = self.get_location()
        b = p.vector2_add(a, p.vector2_scale(self.direction, 10))
        p.draw_line(int(self.X), int(self.Y), int(b.x), int(b.y), p.GREEN)
                    

    
#thing_as_class = class(thing)