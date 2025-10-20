from __future__ import annotations
import pyray as p


class doer:
    def __init__(self):
        self.reset()

    def reset(self):
        self.done = False
        self.repeat = False

    def do(self, entity:entity, dt:float):
        pass