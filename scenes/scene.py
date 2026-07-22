#/scenes/scene.py
import pyray as p
import controls
from entities.entity import entity
from entity_manager import entity_manager
from ponger_camera import p_camera
import info

class scene(entity):
    #/scenes/scene.py[class{"scene", inherit{entity}}]
    def __init__(self, game):
        super().__init__()
        self.entities = entity_manager()
        self.camera = p_camera()
        self.entities.add(self.camera)
        self.game = game
        


    def update(self, dt:float):
        myparent = super()
        myparent.update(dt)
        updaters = self.entities.get_all()
        pyray_camera = self.camera.me
        controls.update_controls(pyray_camera)
        self.check_test()
        for HUD in self.huds:
            HUD.update(dt)
        for e in updaters:
            e.update(dt)

    def check_test(self):
        if info.TESTING_ALLOWED:
            if p.is_key_down(p.KeyboardKey.KEY_RIGHT_CONTROL):
                if p.is_key_down(p.KeyboardKey.KEY_RIGHT_SHIFT):
                    if p.is_key_pressed(p.KeyboardKey.KEY_DELETE):
                        self.game.debug = not self.game.debug
                        if self.game.debug:
                            self.debug_on()
                        else:
                            self.debug_off()
    
    def debug_on(self):
        pass

    def debug_off(self):
        pass

            
    def draw(self):
        pyray_camera = self.camera.me

        p.begin_drawing()
        p.begin_mode_2d(pyray_camera)

        for e in self.entities.get_all():
            e.draw()    

        p.end_mode_2d()
        for HUD in self.huds:
            HUD.draw()
        p.end_drawing()

    def die(self):
        for e in self.entities.get_all():
            self.entities.remove(e)
        print("gdujgshud")

#\file