import pyray as p
from entities.ball import ball 
from level import level
from ponger_camera import p_camera
import controls
from game import game
import info

from scenes.scene import scene
from scenes.menu_scene import menu_scene
from scenes.level_scene import level_scene



p.init_window(info.SCREEN_WIDTH, info.SCREEN_HEIGHT, b"Ponger")
p.init_audio_device()
#p.toggle_fullscreen()
p.set_target_fps(60)

gos = game()
