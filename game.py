from scenes.menu_scene import menu_scene
import pyray as p
class game:

    def __init__(self):

        self.current_scene = menu_scene(self)
        self.run()

    def run(self):
        while not p.window_should_close():
            dt = p.get_frame_time()
            self.current_scene.update(dt)
            self.current_scene.draw()

