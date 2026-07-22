
from errors.UnknownEntityError import UnknownEntityError


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
        interesting = filter(lambda v: isinstance(v, _class), self.entities)
        return list(interesting)
        
    def get_by_name(self, name):
        #please help me with this one, i have no idea how to do it
        for e in self.entities:
            if hasattr(e, "id") and e.id == name:
                return e
        raise UnknownEntityError(name)