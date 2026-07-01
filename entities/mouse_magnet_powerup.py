#/entities/mouse_magnet_powerup.py

from entities.powerup import powerup
class mouse_magnet_powerup(powerup):
    #/entities/mouse_magnet_powerup.py[class{"mouse_magnet_powerup", inherit{powerup}}]
    def __init__(self, pos):
        #/entities/mouse_magnet_powerup.py[class{"mouse_magnet_powerup"}(function{"__init__", paramaters{self, pos}})]
        super().__init__(pos, "mouse_magnet_powerup.png")
        self.pos = pos
        #\function

    def update(self, dt:float):
        #/entities/mouse_magnet_powerup.py[class{"mouse_magnet_powerup"}(function{"update", paramaters{self, dt}})]
        super().update(dt)
        #\function

    def get_default_duration(self):
        #/entities/mouse_magnet_powerup.py[class{"mouse_magnet_powerup"}(function{"get_default_duration", paramaters{self}})]
        return 7.5
        #\function
    #\class
#\file
        