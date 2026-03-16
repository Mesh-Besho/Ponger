#Helper AI thing, Are you ready?
class WheresTheMusicError(Exception):
    def __init__(self, level_num:int=1):
        self.message = f"Level {level_num} doesn't have music and it needs some, sorry"
        super().__init__(self.message)
    
    def __str__(self):
        return self.message