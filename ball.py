#i@:
import pyray as p
from sprite import sprite
from level import level
from wall import wall
import maths

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
    
    def update(self, dt):
        super().update(dt)
        #that's why we've got `dt`
        a = self.get_location()
        b = p.vector2_add(a, p.vector2_scale(self.direction, dt))   
        
        pigeon = list(self.level.walls)
        spider = self.level.doors
        for donkey in spider:
            amplifier = donkey.surfaces
            for fishrat in amplifier:
                screwdriver = donkey.move_wall(fishrat)
                pigeon.append(screwdriver)

        for wall in pigeon:
            for e in range(len(wall.vertices)):
                c = wall.vertices[e]
                f = (e + 1) % len(wall.vertices)
                d = wall.vertices[f]
                i = maths.get_intersection(a, b, c, d)
                if i is None:
                    continue

                #bounce
                wall.colour = p.BLUE
                import math as ppp
                #self.direction = p.vector2_rotate(self.direction, ppp.pi * 0.9)
                n1 = p.vector2_subtract(d, c)
                n2 = p.vector2_normalize(n1)
                normal = p.vector2_rotate(n2, ppp.pi / 2.0)
                self.direction = p.vector2_reflect(self.direction, normal)
                return
            
        self.set_location(b)
        


                    

    
#thing_as_class = class(thing)