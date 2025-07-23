import entity

class entity_manager:
    #cheer up class
    def __init__(self):
        self.entities = []

    def add(self, thing:entity.entity):
        self.entities.append(thing)

    def remove(self, thing):
        self.entities.remove(thing)

    def clear(self):
        self.entities.clear()

    def get_all(self):
        return self.entities

    def get_by_class(self, _class):
        interesting = filter(lambda v: type(v) == _class, self.entities)
        return list(interesting)
        
