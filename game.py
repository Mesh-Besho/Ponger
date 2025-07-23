class game:
    def __init__(self):
        from entity_manager import entity_manager
        from level import level
        self.entities = entity_manager()
    