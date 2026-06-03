class UnknownEntityError(Exception):
    def __init__(self, entity_name):
        self.message = f"Unknown entity with name '{entity_name}'"
    def __str__(self):
        return self.message