#/entities/trigger.py
from entities.entity import entity
import pyray as p
import info
import pongthon as ppy
from wall import wall
from chatgpt import Line

class trigger(wall):
    #/entities/trigger.py[class{name:trigger inherrit{entity}}]
    def __init__(self, verticies, scripts):
        #/entities/trigger.py[class{name:trigger inherrit{entity}}[def{name:__init__ args:{verticies, scripts}}]]
        super().__init__()
        self.vertices = verticies
        self.scripts = scripts
        self.player_inside = False
        #\function

    def collides_with(self, d, l)->bool:
        ls = self.get_lines()
        better_l = p.vector2_add(d, l)
        ball_line = Line(l, better_l)

        for l in ls:
            if p.check_collision_lines(l.a, l.b, ball_line.a, ball_line.b, None):
                return True
            
        return False

    def on_enter(self):
        #/entities/trigger.py[class{name:trigger inherrit{entity}}[def{name:on_enter args:{player}}]]
        name = self.scripts[0]
        my_script = ppy.Script(open(name).read())
        ppy.ScriptRunner(my_script).run()
        self.player_inside = True
        #\function

    def on_exit(self):
        #/entities/trigger.py[class{name:trigger inherrit{entity}}[def{name:on_exit args:{player}}]]
        name = self.scripts[1]
        my_script = ppy.Script(open(name).read())
        ppy.ScriptRunner(my_script).run()
        self.player_inside = False
        #\function
    #\class
#\file