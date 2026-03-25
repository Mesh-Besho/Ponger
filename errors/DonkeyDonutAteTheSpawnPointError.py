class DonkeyDonutAteTheSpawnPointError(Exception):
    def __init__(self, filename):
        self.filename = filename

    def __str__(self):
        return f"Donkey Donut ate the spawn point in {self.filename}! Please make it sick it up and try again."