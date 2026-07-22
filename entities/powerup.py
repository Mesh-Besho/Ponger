#/entities/powerup.py
from entities.sprite import collectable, sprite
class powerup(collectable):
    #/entities/powerup.py[class{"powerup", inherit{entity}}]
    def __init__(self, pos, t_name, duration=0.0):
        #/entities/powerup.py[class{"powerup"}(function{"__init__", paramaters{self, pos}})]
        super().__init__(t_name)
        self.set_location(pos)
        self.set_size_fits(16, 16)
        self.set_origin_center()
        self.collected = False
        self.timer = duration

        if duration > 0.0:
            self.duration = duration
            
        else:
            self.duration = self.get_default_duration()

        #\function

    def get_default_duration(self):
        #/entities/powerup.py[class{"powerup"}(function{"get_default_duration", paramaters{self}})]
        return 5.0
        #\function

    def when_collected(self, scene):
        self.collected = True
        self.player = scene.game.player

    def update(self, dt:float):
        if self.collected:
            self.duration -= dt
            if self.duration <= 0.0:
                self.player.lose_item(self)
            self.timer -= dt

    #/class
#\file