import pyray as p

from game import game
from level import level

p.init_window(800, 600, b"Ponger")
p.init_audio_device()
#p.toggle_fullscreen()
p.set_target_fps(30)

game = game()
level = level(game)
level.load("levels/l1.ponger")
game.entities.add(level)

while not p.window_should_close():
    dt = p.get_frame_time()

    p.begin_drawing()
    p.clear_background(p.WHITE)
    
    #game.update(dt)

    for e in game.entities.get_all():
        e.update(dt)
        e.draw()    
    
    p.end_drawing()
    