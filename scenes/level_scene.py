#/scenes/menu_scene.py
from doers.timer import timer
import pyray as p
import scenes.scene as scene
from entities.text import text

class level_scene(scene.scene):
    #/scenes/menu_scenes.py[class{"menu", inherit{scene}}]
    def __init__(self, level_num:int, game):
        super().__init__(game)
        self.huds = []
        from level import level
        from entities.ball import ball
        from entities.hud import hud

        #/scenes/menu_scene.py[class{menu}(funtion{"__init__", paramaters{self}})]
        
        self.hud = hud(self)
        self.huds.append(self.hud)

        self.level = level()
        self.level.load(f"levels/l{level_num}.ponger")
        self.entities.add(self.level)
        self.won = False

        my_ball = ball(self)
        my_ball.set_location(p.Vector2(50.0, 50.0))
        self.entities.add(my_ball)
        self.level_num = level_num
        for mikey in self.level.objects:
            self.entities.add(mikey)


        #from entities.sprite import sprite
        #test_mouse = sprite(p.load_texture("mesh_besho.png"))
        #test_mouse.W = 64
        #test_mouse.H = 64
        #game.entities.add(test_mouse)

        #from doers.sprite_spinner import sprite_spinner
        #my_ball.current_do = sprite_spinner(360)

    def win(self):
        if not self.won:
            self.entities.add(text("YOU WIN!", 100, 101, 28, p.BLUE))
            self.do_something_soon(timer(5.32, self.win_part_b))
            self.won = True
    def win_part_b(self):
        self.game.switch_scene(level_scene(self.level_num + 1, self.game))

    def collect_obj(self, obj):
        self.level.keys.remove(obj)
        self.level.objects.remove(obj)
        self.entities.remove(obj)
        obj.when_collected(self)
        self.game.player.collect_item(obj)

    def update(self, dt):
        super().update(dt)

        if p.is_key_down(p.KeyboardKey.KEY_UP):
            self.camera.move_by(0.0, -2.0)
        if p.is_key_down(p.KeyboardKey.KEY_DOWN):
            self.camera.move_by(0.0, 2.0)
        if p.is_key_down(p.KeyboardKey.KEY_LEFT):
            self.camera.move_by(-2.0, 0.0)
        if p.is_key_down(p.KeyboardKey.KEY_RIGHT):
            self.camera.move_by(2.0, 0.0)

    #    if p.is_key_down(p.KeyboardKey.KEY_LEFT_CONTROL):
    #        camera.rotation += 0.7
    #    if p.is_key_down(p.KeyboardKey.KEY_RIGHT_CONTROL):
    #        if p.is_key_down(p.KeyboardKey.KEY_LEFT_SHIFT):
    #            if p.is_key_down(p.KeyboardKey.KEY_F7):
    #                if p.is_key_down(p.KeyboardKey.KEY_ZERO):
    #                    exit("shhhhhhh")

        if p.is_key_down(p.KeyboardKey.KEY_LEFT_CONTROL or p.is_key_down(p.KeyboardKey.KEY_RIGHT_CONTROL)):
            if p.is_key_down(p.KeyboardKey.KEY_Q):
                from scenes.menu_scene import menu_scene
                self.game.current_scene = menu_scene(self.game)

    