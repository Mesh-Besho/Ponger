from entities.entity import entity
from scenes.level_scene import level_scene
import info
import pyray as p

class hud(entity):
    def __init__(self, level_scene:level_scene):
        super().__init__()
        self.scene = level_scene

    def draw(self):
        super().draw()
        p.draw_rectangle(0, 0, info.SCREEN_WIDTH, 40, p.color_alpha(p.YELLOW, 0.3))

        thing_x = 10

        for thing in self.scene.game.player.items:
            thing.set_location(p.Vector2(thing_x, 10))
            thing.draw()
            thing_x += thing.W + 20