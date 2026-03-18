class player:
    def __init__(self, balls_left=2):
        self.items = []
        self.balls_left = balls_left

    def collect_item(self, item):
        self.items.append(item)

    def lose_item(self, item):
        if item in self.items:
            self.items.remove(item)

    def find_item(self, item:str):
        for X in self.items:
            if X.obj_id == item:
                return X
        return None
    
    def lose_spare_ball(self):
        x = True
        if self.balls_left > 0:
            self.balls_left -= 1
            x = False
        return x

    def update(self, dt):
        # Could use this for things like power-up durations
        pass
    