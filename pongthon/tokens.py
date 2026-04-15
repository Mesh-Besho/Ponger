class Token:
    pass

class VariableToken(Token):
    def __init__(self, name:str):
        self.name = name

    def __str__(self):
        return f"Variable({self.name})"

class ConstantToken(Token):
    pass

class NumericConstantToken(ConstantToken):
    def __init__(self, value:float|int):
        self.value = value

    def __str__(self):
        return f"NumericConstant({self.value})"

class StringConstantToken(ConstantToken):
    def __init__(self, value:str):
        self.value = value

    def __str__(self):
        return f"StringConstant(\"{self.value}\")"

class StatementToken(Token):
    def __init__(self, command:str, args:list):
        self.command = command
        self.args = args

    def __str__(self):
        args_str = " ".join(str(a) for a in self.args)
        return f"Statement({self.command}: {args_str})"

