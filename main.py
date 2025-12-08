import pyray as p
from entities.ball import ball 
from level import level
from ponger_camera import p_camera
import controls

from scenes.scene import scene
from scenes.menu_scene import menu_scene
from scenes.level_scene import level_scene

p.init_window(800, 600, b"Ponger")
p.init_audio_device()
#p.toggle_fullscreen()
p.set_target_fps(60)

current_scene = level_scene(1)

while not p.window_should_close():
    dt = p.get_frame_time()
    current_scene.update(dt)
    current_scene.draw()
