#/scenes/scene.py
import pyray as p
import controls
from entities.entity import entity
from entity_manager import entity_manager
from ponger_camera import p_camera

class scene(entity):
    #/scenes/scene.py[class{"scene", inherit{entity}}]
    def __init__(self, game):
        super().__init__()
        self.entities = entity_manager()
        self.camera = p_camera()
        self.entities.add(self.camera)
        self.game = game


    def update(self, dt:float):
        pyray_camera = self.camera.me
        controls.update_controls(pyray_camera)
        for e in self.entities.get_all():
            e.update(dt)

    def draw(self):
        pyray_camera = self.camera.me

        p.begin_drawing()
        p.begin_mode_2d(pyray_camera)

        for e in self.entities.get_all():
            e.draw()    

        p.end_mode_2d()
        p.end_drawing()

    #\class
#\file