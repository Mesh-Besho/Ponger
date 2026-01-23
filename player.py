class player:
    def __init__(self):
        self.items = []

    def collect_item(self, item):
        self.items.append(item)

    def lose_item(self, item):
        if item in self.items:
            self.items.remove(item)

    def update(self, dt):
        # Could use this for things like power-up durations
        pass
    