from entities.entity import entity
from scenes.level_scene import level_scene
import info
import pyray as p
from entities.ball import ball

class hud(entity):
    def __init__(self, level_scene:level_scene):
        super().__init__()
        self.scene = level_scene

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
        
    
    def do_health_bar(self, health, max_health):
        percentage = health / max_health * 100
        health_in_bar = percentage / 100 * info.HEALTH_BAR_WIDTH
        p.draw_rectangle(10, 10, int(health_in_bar), 20, p.BLUE)
        p.draw_rectangle_lines(10, 10, info.HEALTH_BAR_WIDTH, 20, p.BLACK)
        p.draw_text(f"{percentage:.1f}%", info.HEALTH_BAR_WIDTH + 20, 10, 28, p.BLACK)