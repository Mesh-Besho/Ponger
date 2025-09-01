import pyray as p

from game import game
from level import level

p.init_window(800, 600, b"Ponger")
p.init_audio_device()
#p.toggle_fullscreen()
p.set_target_fps(60)

game = game()
level = level(game)
level.load("levels/l1.ponger")
game.entities.add(level)

camera = p.Camera2D(p.Vector2(), p.Vector2(), 0.0, 1.0)

while not p.window_should_close():
    dt = p.get_frame_time()

    p.begin_drawing()
    p.begin_mode_2d(camera)
    
    #game.update(dt)

    for e in game.entities.get_all():
        e.update(dt)
        e.draw()    
    

    if p.is_key_down(p.KeyboardKey.KEY_LEFT_ALT) or p.is_key_down(p.KeyboardKey.KEY_RIGHT_ALT):
        if p.is_key_down(p.KeyboardKey.KEY_PERIOD):
            camera.zoom += 0.001
        if p.is_key_down(p.KeyboardKey.KEY_COMMA):
            camera.zoom -= 0.001
    if p.is_key_down(p.KeyboardKey.KEY_UP):
        camera.target = p.vector2_subtract(camera.target, p.Vector2(0.0, 2.0))
    if p.is_key_down(p.KeyboardKey.KEY_DOWN):
        camera.target = p.vector2_add(camera.target, p.Vector2(0.0, 2.0))
    if p.is_key_down(p.KeyboardKey.KEY_LEFT):

        camera.target = p.vector2_subtract(camera.target, p.Vector2(2.0, 0.0))

    if p.is_key_down(p.KeyboardKey.KEY_RIGHT):
        camera.target = p.vector2_add(camera.target, p.Vector2(2.0, 0.0))
    if p.is_key_down(p.KeyboardKey.KEY_LEFT_CONTROL):
        camera.rotation += 0.7
    if p.is_key_down(p.KeyboardKey.KEY_RIGHT_CONTROL):
        if p.is_key_down(p.KeyboardKey.KEY_LEFT_SHIFT):
            if p.is_key_down(p.KeyboardKey.KEY_F7):
                if p.is_key_down(p.KeyboardKey.KEY_RIGHT_BRACKET):
                    pass

    p.end_mode_2d()
    p.end_drawing()
    #~~:@3517''""[VBS]ALEX