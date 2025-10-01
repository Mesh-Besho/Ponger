
class entity_manager:
    from entities.entity import entity
    #cheer up class
    def __init__(self):
        self.entities = []

    def add(self, thing:entity):
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
        
