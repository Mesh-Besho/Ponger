from player import player
from scenes.level_scene import level_scene
from scenes.menu_scene import menu_scene
import pyray as p
class game:

    def __init__(self):
        self.debug = False
        self.switch_scene(menu_scene(self))

        #self.current_scene = level_scene(3, self)
        self.player = player()
        self.run()

    def run(self):
        while not p.window_should_close():
            dt = p.get_frame_time()
            the_frames_scene = self.current_scene
            the_frames_scene.update(dt)
            self.player.update(dt)
            the_frames_scene.draw()

    def start_game(self, level_num:int):
        self.player = player()
        self.switch_scene(level_scene(level_num, self))

    def restart_game(self):
        self.switch_scene(menu_scene(self))

    def restart_level(self):
        if isinstance(self.current_scene, level_scene):
            level_num = self.current_scene.level_num
            self.switch_scene(level_scene(level_num, self))

    def switch_scene(self, new_scene):
        if hasattr(self, "current_scene"):
            self.current_scene.die()

        self.current_scene = new_scene

        if self.debug:
            self.current_scene.debug_on()
        else:
            self.current_scene.debug_off()

    

