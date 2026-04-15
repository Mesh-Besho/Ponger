from multiprocessing.managers import rebuild_as_list
from typing import Any

from . import YourCodeIsStupidError
from .script import Script
from .tokens import *

class ScriptState:
    def __init__(self):
        self.variables = {}

    def set_variable(self, name:str, value:Any) -> None:
        self.variables[name] = value

    def get_variable(self, name:str) -> Any:
        return self.variables[name]

class ScriptRunner:
    def __init__(self, script:Script):
        self.script = script
        self.state = ScriptState()

    def run(self):
        for statement in self.script.statements:
            self.run_statement(statement)

    def run_statement(self, statement:StatementToken):
        if statement.command.upper() == "SET":
            return self.run_command_set(statement.args)

        elif statement.command.upper() == "PRINT":
            return self.run_command_print(statement.args)

        elif statement.command.upper() == "PRINTLN":
            return self.run_command_print(statement.args + [StringConstantToken("\n")])

        elif statement.command.upper() == "ADD":
            return self.run_command_add(statement.args)

        else:
            raise YourCodeIsStupidError(f"What nonsense command is '{statement.command}'? Never heard of it!")

    def run_command_set(self, args:list):
        if not len(args) == 2:
            raise YourCodeIsStupidError("SET command needs 2 arguments!")

        if not isinstance(args[0], VariableToken):
            raise YourCodeIsStupidError("SET command first argument must be a variable!")

        value = self.get_value(args[1])
        self.state.set_variable(args[0].name, value)

        # Return is the value we assigned
        return value

    def run_command_print(self, args):
        line = str.join("", (str(self.get_value(a)) for a in args))
        print(line, end="")

        # Return is the formatted string
        return line

    def run_command_add(self, args):
        if not len(args) == 2:
            raise YourCodeIsStupidError("ADD command needs 2 arguments!")

        value1 = self.get_value(args[0])
        value2 = self.get_value(args[1])

        if not type(value1) == type(value2):
            raise YourCodeIsStupidError("Types of arguments for ADD don't match!")

        return value1 + value2

    def get_value(self, token:Token):
        if isinstance(token, VariableToken):
            return self.state.get_variable(token.name)

        if isinstance(token, NumericConstantToken):
            return token.value

        if isinstance(token, StringConstantToken):
            return token.value

        if isinstance(token, StatementToken):
            return self.run_statement(token)

        raise YourCodeIsStupidError(f"Can't get a value from {token}, what is that supposed to be?")
