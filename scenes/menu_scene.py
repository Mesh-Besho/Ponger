#/scenes/menu_scene.py
import scenes.scene as scene
import pyray as p
from entities.sprite import sprite

from scenes.level_scene import level_scene

class menu_scene(scene.scene):
    #/scenes/menu_scenes.py[class{"menu_scene", inherit{scene}}]
    def __init__(self, game):
        #/scenes/menu_scene.py[class{"menu_scene"}(funtion{"__init__", paramaters{self}})]
        super().__init__(game)
        t = p.load_texture("Ts.png")
        my_sprite = sprite(t)
        self.camera.reset()
        self.entities.add(my_sprite)
        self.mus = p.load_music_stream("Menu_fixed.ogg")
        p.play_music_stream(self.mus)
        self.game = game

    def update(self, dt:float):
        p.update_music_stream(self.mus)
        if p.is_key_pressed(p.KeyboardKey.KEY_P):
            self.game.current_scene = level_scene(1, self.game)
            
        

        #\function
    #\class
#\file
