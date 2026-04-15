class StringEnumerator:
    def __init__(self, text:str):
        self.text = text
        self.index = 0

    @property
    def current(self) -> str:
        return self.text[self.index]

    def next(self):
        self.index += 1

    @property
    def is_finished(self) -> bool:
        return self.index >= len(self.text)

    @property
    def is_space(self) -> bool:
        """Current character is whitespace, but not newline"""
        return not self.current == '\n' and self.current.isspace()

    @property
    def is_newline(self) -> bool:
        return self.current == '\n'

    @property
    def is_numeric(self) -> bool:
        return self.current.isdigit()

class YourCodeIsStupidError(Exception):
    def __init__(self, message: str):
        self.message = message