from entities.entity import entity
from scenes.level_scene import level_scene
import info
import pyray as p
from entities.ball import ball
import random


class hud(entity):
    def __init__(self, level_scene:level_scene):
        super().__init__()
        self.scene = level_scene
        self.png = p.load_texture("box.png")
        self.ball_png = p.load_texture("ball.png")
        self.co_ordinates_cache = []

    def draw(self):
        super().draw()
        p.draw_rectangle(0, 0, info.SCREEN_WIDTH, 40, p.color_alpha(p.YELLOW, 0.3))


        total_health = 0
        total_max_health = 0
        for _ball in self.scene.entities.get_by_class(ball):
            total_health += _ball.health
            total_max_health += _ball.max_health
        if total_max_health > 0:
            self.do_health_bar(total_health, total_max_health)

        thing_x = 10 + info.HEALTH_BAR_WIDTH + 100
        for thing in self.scene.game.player.items:
            thing.set_location(p.Vector2(thing_x, 10))
            thing.draw()
            thing_x += thing.W + 20

        self.ball_box()

    def ball_box(self):
        tex = self.png
        sou = p.Rectangle(0, 0, self.png.width, self.png.height)
        des = p.Rectangle(info.BLC.x, info.BLC.y - 100, 100, 100)
        ori = p.Vector2(0, 0)
        rot = 0
        tin = p.WHITE
        p.draw_texture_pro(tex, sou, des, ori, rot, tin)
        for N in range(self.scene.game.player.balls_left):#heard the name?
            if N > len(self.co_ordinates_cache) - 1:
                X = random.randrange(int(des.x), int(des.x + des.width - 20))
                Y = random.randrange(int(des.y + 35), int(des.y + des.height - 10))
                self.co_ordinates_cache.append(p.Vector2(X, Y))
            p.draw_texture_ex(self.ball_png, self.co_ordinates_cache[N], 0.0, 0.05001, p.WHITE)

        
    
    def do_health_bar(self, health, max_health):
        percentage = health / max_health * 100
        health_in_bar = percentage / 100 * info.HEALTH_BAR_WIDTH
        p.draw_rectangle(10, 10, int(health_in_bar), 20, p.BLUE)
        p.draw_rectangle_lines(10, 10, info.HEALTH_BAR_WIDTH, 20, p.BLACK)
        p.draw_text(f"{percentage:.1f}%", info.HEALTH_BAR_WIDTH + 20, 10, 28, p.BLACK)