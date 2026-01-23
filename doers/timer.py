from doers.doer import doer
from entities.entity import entity

class timer(doer):
    def __init__(self, duration:float, callback:callable):
        super().__init__()
        self.duration = duration
        self.callback = callback
        self.elapsed = 0.0
        self.finished = False

    def do(self, entity:entity, dt:float):
        if self.finished:
            return
        self.elapsed += dt
        if self.elapsed >= self.duration:
            self.finished = True
            self.callback()
            self.die()

    def die(self):
        self.finished = True
