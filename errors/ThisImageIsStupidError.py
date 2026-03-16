class ThisImageIsStupidError(Exception):
    def __init__(self, type=":( SORRY I CAN'T LOAD THIS IMAGE :("):
        self.message = f"This image is stupid: {type}"
        super().__init__(self.message)
    
    def __str__(self):
        return self.message