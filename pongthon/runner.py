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
    def __init__(self, script:Script, scene: Any):
        self.script = script
        self.state = ScriptState()
        self.scene = scene

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

        elif statement.command.upper() == "CALL":
            return self.run_command_call(statement.args)
        
        elif statement.command.upper() == "GETENTITY":
            return self.run_command_getentity(statement.args)

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
    
    def run_command_call(self, args):
        if not len(args) >= 2:
            raise YourCodeIsStupidError("CALL command needs at least 2 arguments: owner, method, and optionally more arguments!")
        owner = self.get_value(args[0])
        method_name = self.get_value(args[1])
        if len(args) > 2:
            method_args = [self.get_value(a) for a in args[2:]]
        else:
            method_args = []

        owner.do_your_script_this_instant(method_name, method_args)

    def run_command_getentity(self, args):
        if not len(args) == 1:
            raise YourCodeIsStupidError("GETENTITY command needs exactly 1 argument!")

        entity_name = self.get_value(args[0])
        entity = self.scene.entities.get_by_name(entity_name)  # TODO: implement entity retrieval by name
        return entity

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
