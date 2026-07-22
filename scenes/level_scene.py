#/scenes/menu_scene.py
from doers.timer import timer
import pyray as p
from entities import door, key
import scenes.scene as scene
from entities.text import text
import chatgpt as bounce_code

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

        self.mus = p.load_music_stream(self.level.song)
        p.play_music_stream(self.mus)
        self.won = False

        self.my_ball = ball(self)
        self.my_ball.set_location(self.level.ball_spawn)
        self.my_ball.id = "mainball"
        self.entities.add(self.my_ball)
        self.level_num = level_num
        for mikey in self.level.objects:
            self.entities.add(mikey)
            #sudo scene
        for my_laser in self.level.lasers:
            self.entities.add(my_laser)
            my_laser.scene = self
        for my_trigger in self.level.triggers:
            self.entities.add(my_trigger)
            my_trigger.scene = self
        
        
    def get_lines(self):#get_lines
        ls = []
        walls = list(self.level.walls)
        for door in self.level.doors:
            for surface in door.surfaces:
                moved_wall = door.move_wall(surface)
                walls.append(moved_wall)
        for blocker in walls:
            l = blocker.get_lines()
            ls.extend(l)
        return ls
        #\function

    def get_closest_hit(self, start:p.Vector2, direction:p.Vector2, length:float=1000.0):
        hits = []
        ls = self.get_lines()
        for line in ls:
            #self, start, direction, length, p, line => hit to add to hits
            my_donkey_or_spider_i_dont_know = p.Vector2(0.0, 0.0)
            other_line_end = p.vector2_add(p.vector2_scale(direction, length), start)
            other_line = bounce_code.Line(start, other_line_end)
            hit = p.check_collision_lines(line.a, line.b, other_line.a, other_line.b, my_donkey_or_spider_i_dont_know)
            if hit:
                hits.append((line, my_donkey_or_spider_i_dont_know))
        
        if len(hits) == 0:
            return None
        else:
            closest_hit = hits[0]
            closest_d = length + 999.9990
            for hit in hits:
                d = p.vector2_distance(start, hit[1])
                if d < closest_d:
                    closest_d = d
                    closest_hit = hit

            return closest_hit
        
    def game_over(self):
        self.entities.add(text("GAME OVER", 100, 101, 28, p.RED))
        self.do_something_soon(timer(5.0, self.game_over_part_b))
   
    def game_over_part_b(self):
        self.entities.clear()
        self.game.restart_game()

    def bye_ball(self):
        self.entities.add(text("You lost a ball!", 100, 101, 28, p.RED))
        self.do_something_soon(timer(5.0, self.game.restart_level))
    

    #from entities.sprite import sprite
        #test_mouse = sprite(p.load_texture("mesh_besho.png"))
        #test_mouse.W = 64
        #test_mouse.H = 64
        #game.entities.add(test_mouse)

        #from doers.sprite_spinner import sprite_spinner
        #my_ball.current_do = sprite_spinner(360)

    def win(self, lev_num="a"):
        if lev_num == "a":
            lev_num = self.level_num + 1
        self.entities.add(text("YOU WIN!", 100, 101, 28, p.BLUE))
        self.do_something_soon(timer(5.32, self.win_part_b))
        self.won = True
        self.destination_level = lev_num

        
            
    def win_part_b(self):
        self.game.switch_scene(level_scene(self.destination_level, self.game))

    def collect_obj(self, obj):
        #if isinstance(obj, key.key): 
        #    self.level.keys.remove(obj)
        #self.level.objects.remove(obj)
        self.entities.remove(obj)
        obj.when_collected(self)
        self.game.player.collect_item(obj)

    def update(self, dt):
        super().update(dt)
        p.update_music_stream(self.mus)

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
                self.game.switch_scene(menu_scene(self.game))

    def debug_on(self):
        super().debug_on()
        self.camera.follow(None)

    def debug_off(self):
        super().debug_off()
        self.camera.follow(self.my_ball)