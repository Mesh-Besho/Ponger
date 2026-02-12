from player import player
from scenes.level_scene import level_scene
from scenes.menu_scene import menu_scene
import pyray as p
class game:

    def __init__(self):
        self.current_scene = menu_scene(self)
        self.player = player()
        self.run()

    def run(self):
        while not p.window_should_close():
            dt = p.get_frame_time()
            self.current_scene.update(dt)
            self.current_scene.draw()

    def start_game(self, level_num:int):
        self.player = player()
        self.switch_scene(level_scene(level_num, self))

    def switch_scene(self, new_scene):
        self.current_scene.die()
        self.current_scene = new_scene

