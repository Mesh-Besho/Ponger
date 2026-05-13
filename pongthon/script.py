from .tokens import *
from .helper import *

class Script:
    def __init__(self, text:str):
        self.statements = self.parse(text)

    def __str__(self):
        return str.join("\n", (str(a) for a in self.statements))

    def parse(self, text):
        tokens = []
        reader = StringEnumerator(text)

        while not reader.is_finished:
            if reader.is_newline:
                reader.next()
                continue

            if reader.is_space:
                self.read_space(reader)
                continue

            if reader.current == '#':
                self.read_comment(reader)
                continue

            statement = self.read_statement(reader)
            tokens.append(statement)

            reader.next()

        return tokens

    def read_comment(self, reader:StringEnumerator):
        if not reader.current == '#':
            raise YourCodeIsStupidError("Expected a comment")

        while not reader.is_finished and not reader.current == '\n':
            reader.next()

    def read_statement(self, reader:StringEnumerator, is_sub:bool = False) -> StatementToken:
        command_name = self.read_command(reader)
        seen_sub_end = False
        args = []

        while not reader.is_finished and not reader.is_newline:
            self.read_space(reader)

            if is_sub and reader.current == ')':
                seen_sub_end = True
                break

            arg = self.read_argument(reader)
            args.append(arg)

        if is_sub and not seen_sub_end:
            raise YourCodeIsStupidError("Missing closing ')' in sub-statement")

        return StatementToken(command_name, args)

    def read_command(self, reader:StringEnumerator) -> str:
        name_chars = []

        while not reader.is_finished and not reader.current.isspace():
            if not reader.current.isalpha():
                raise YourCodeIsStupidError(f"Invalid character '{reader.current}' in statement command name")

            name_chars.append(reader.current)
            reader.next()

        if len(name_chars) == 0:
            raise YourCodeIsStupidError("Expected a command name")

        return str.join("", name_chars)

    def read_space(self, reader:StringEnumerator):
        """Consume all space, but not newline"""
        while True:
            if reader.is_finished:
                break

            if not reader.is_space:
                break

            reader.next()

    def read_argument(self, reader:StringEnumerator):
        """Reads a command argument, which is a variable, constant or sub-statement"""
        if reader.current == "(":
            reader.next()
            sub = self.read_statement(reader, True)
            # This should be guaranteed by read_statement
            if not reader.current == ")":
                raise YourCodeIsStupidError("The whole world has exploded")
            reader.next()
            return sub

        if reader.current == '"':
            return self.read_string(reader)

        if reader.is_numeric or reader.current == '-':
            return self.read_numeric(reader)

        return self.read_variable(reader)

    def read_string(self, reader:StringEnumerator) -> StringConstantToken:
        string_chars = []

        if not reader.current == '"':
            raise YourCodeIsStupidError("Expected a string")

        reader.next()

        while not reader.current == '"':
            if reader.is_finished or reader.is_newline:
                raise YourCodeIsStupidError("Unexpected end of string")

            string_chars.append(reader.current)
            reader.next()

        # Consume closing quote
        reader.next()

        return StringConstantToken(str.join("", string_chars))

    def read_numeric(self, reader:StringEnumerator) -> NumericConstantToken:
        numeric_chars = []
        had_dot = False
        had_digits = False

        # One single '-' is allowed at the start
        if reader.current == '-':
            numeric_chars.append(reader.current)
            reader.next()

        while True:
            # Valid ways to end a numeric
            if reader.is_finished or reader.is_newline or reader.is_space or reader.current == ')':
                break

            if reader.current == '.':
                if had_dot:
                    raise YourCodeIsStupidError("Two decimal points in a number?!")
                had_dot = True
                numeric_chars.append(reader.current)
                reader.next()
                continue

            if not reader.is_numeric:
                raise YourCodeIsStupidError(f"Invalid character in numeric constant: {reader.current}")

            numeric_chars.append(reader.current)
            had_digits = True
            reader.next()

        if not had_digits:
            raise YourCodeIsStupidError("No digits in numeric constant?!?!?!")

        if had_dot:
            numeric_value = float(str.join("", numeric_chars))
        else:
            numeric_value = int(str.join("", numeric_chars))

        return NumericConstantToken(numeric_value)

    def read_variable(self, reader):
        name_chars = []

        while not reader.is_finished and not reader.current.isspace() and reader.current != ')':
            if not reader.current.isalpha():
                raise YourCodeIsStupidError(f"Invalid character '{reader.current}' in variable name")

            name_chars.append(reader.current)
            reader.next()

        if len(name_chars) == 0:
            raise YourCodeIsStupidError("Expected a variable name")

        name = str.join("", name_chars)
        return VariableToken(name)