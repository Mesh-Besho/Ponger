#
class UnknownObjectTypeError(Exception):
    def __init__(self, type:str):
        self.type = type

    def __str__(self):
        return "Unknown object type: " + self.type