class player:
    def __init__(self):
        self.items = []

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
        
    def update(self, dt):
        # Could use this for things like power-up durations
        pass
    