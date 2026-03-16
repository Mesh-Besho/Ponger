#/entities/laser.py
import pyray as p
from entities.sprite import sprite
class fedrgr:
    def __init__(self):
        self.entities = "abc"
class laser(sprite):
    #/entities/laser.py[class{name:laser, inherit:entity}]
    def __init__(self, start:p.Vector2, direction:p.Vector2, damage:float=1.0, colour:p.Color=p.RED):
        #/entities/laser.py[class{name:laser, inherit:entity[funct {name:__init__, paramaters:L(start, direction, damage, colour)}]}]
        super().__init__("laser_emitter.png")
        self.direction = direction
        self.damage = damage
        self.colour = colour
        self.set_location(start)
        self.set_size_fits(16, 16)
        self.set_origin_center()
        self.scene = fedrgr()#It is a fedgr because I don't want to import level_scene and cause circular imports. It will be replaced with the actual level_scene instance when the laser is added to the scene.
        #\function

        from doers.laser_spinner import laser_spinner
        self.do_something_soon(laser_spinner(1.1234567898765432123456789000987654321))
    
        #\function
    
    def update(self, dt):
        #/entities/laser.py[class{name:laser, inherit:entity[funct {name:update, paramaters:L(dt)}]}]
        #PART 1
        super().update(dt)
        s = self.get_location()
        self.R = p.vector2_line_angle(p.vector2_zero(), self.direction) * (-180.0 / p.PI)
        e1 = self.scene.get_closest_hit(s, self.direction)
        if e1 is None:
            self.e = p.vector2_add(s, p.vector2_scale(self.direction, 1000.0))
        else:
            self.e = e1[1]
        
        #PART 2

        from entities.ball import ball
        balls = self.scene.entities.get_by_class(ball)
        ls = self.scene.get_lines()
        for _ball in balls:
            c = _ball.get_location()
            r = _ball.radius
            if p.check_collision_circle_line(c, r, s, self.e):

        #PART 3

                damage_per_frame = self.damage * dt
                _ball.lose_health(damage_per_frame)

        #\function
    
    def draw(self):
        #/entities/laser.py[class{name:laser, inherit:entity[funct {name:draw, Nparamaters}]}]
        super().draw()
        s = self.get_location()
        p.draw_line_v(s, self.e, self.colour)
         
        #\function
    #\class
#\file