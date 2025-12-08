#NF NM
import pyray as p
import math
import maths
from entities.entity import entity
from wall import wall
import controls

HINGE_CLICK_RADIUS = 10

class door(entity):
    def __init__(self):
        super().__init__()
        self.surfaces = []
        self.colour = p.WHITE
        #5 speeds in LPS
        self.VFS = 0.5
        self.FS = 0.25
        self.NS = 0.013
        self.SS = 0.0113
        self.VSS = 0.00000000000132

        #turn them into RPF(normal speed)
        
        spin_speed = ((2*math.pi) / 30.0) * self.NS

        from doers.door_slider import door_slider
        from doers.door_spinner import door_spinner
        from doers.mover import mover
        #self.current_do = door_spinner(spin_speed)
        #self.current_do = door_slider()
        #self.current_do = mover(-70, -70, 30.0)
        #self.next_do = mover(300, 100, 50.0)

        #self.current_do.repeat = True
        #self.next_do.repeat = True
    
    def move_wall(self, x:wall):
        donkey = wall()
        donkey.colour = x.colour
        
        for v in x.vertices:
            new_v = p.vector2_add(v, self.get_location())
            donkey.vertices.append(new_v)

        return donkey
    
    def update(self, dt):
        #self = door > position
        #p = pyray > mouse position & mouse buttons
        #door position V
        #mouse position V
        #click flag V
        c = self.get_location()
        r = HINGE_CLICK_RADIUS
        m = controls.good_mouse_position

        if maths.is_in_circle(c, r, m):
            if p.is_mouse_button_pressed(p.MouseButton.MOUSE_BUTTON_LEFT):
                self.do_event("on_left_click")

            if p.is_mouse_button_pressed(p.MouseButton.MOUSE_BUTTON_RIGHT):
                self.do_event("on_right_click")
      
        


        super().update(dt)

#p: step 4+


    