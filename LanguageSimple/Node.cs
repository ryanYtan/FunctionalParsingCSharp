using SPA.Utility;

namespace SPA;

public abstract class Node
{
    public void Accept(IVisitor<Node> visitor)
    {
        visitor.Visit(this);
    }
}

public class Program
{
    public Program(List<Function> functions)
    {
        Functions = functions;
    }

    public List<Function> Functions { get; }
}

public class Function : Node
{
    public Function(FunctionDecl declaration, List<Statement> statements)
    {
        Declaration = declaration;
        Statements = statements;
    }

    public FunctionDecl Declaration { get; }
    public List<Statement> Statements { get; }
}

public class FunctionDecl : Node
{
    public FunctionDecl(Token functionName, ArgumentList argList)
    {
        FunctionName = functionName;
        ArgList = argList;
    }

    public Token FunctionName { get; }
    public ArgumentList ArgList { get; }
}

public class ArgumentList : Node
{
    public ArgumentList(List<Token> arguments)
    {
        Arguments = arguments;
    }

    public List<Token> Arguments { get; }
}

public abstract class Statement : Node
{
    protected Statement(int statementNumber)
    {
        StatementNumber = statementNumber;
    }

    public int StatementNumber { get; }
}

public class ConditionedStatements : Node
{
    public ConditionedStatements(Expression condition, List<Statement> statements)
    {
        Condition = condition;
        Statements = statements;
    }

    public Expression Condition { get; }
    public List<Statement> Statements { get; }
}

public class If : Statement
{
    public If(int statementNumber, ConditionedStatements ifStatements, List<ConditionedStatements> elseIfStatements, List<Statement> elseStatements) : base(statementNumber)
    {
        IfStatements = ifStatements;
        ElseIfStatements = elseIfStatements;
        ElseStatements = elseStatements;
    }

    public ConditionedStatements IfStatements { get; }
    public List<ConditionedStatements> ElseIfStatements { get; }
    public List<Statement> ElseStatements { get; }
}

public class While : Statement
{
    public While(int statementNumber, ConditionedStatements block) : base(statementNumber)
    {
        Block = block;
    }

    public ConditionedStatements Block { get; }
}

public class Assignment : Statement
{
    public Assignment(int statementNumber, Token leftHandSide, Expression rightHandSide) : base(statementNumber)
    {
        LeftHandSide = leftHandSide;
        RightHandSide = rightHandSide;
    }

    public Token LeftHandSide { get; }
    public Expression RightHandSide { get; }
}

public class Return : Statement
{
    public Return(int statementNumber, Expression returnExpression) : base(statementNumber)
    {
        ReturnExpression = returnExpression;
    }

    public Expression ReturnExpression { get; }
}

public class Expression : Node
{
}

public class BinaryOperator : Expression
{
    public BinaryOperator(Token @operator, Expression leftHandSide, Expression rightHandSide)
    {
        Operator = @operator;
        LeftHandSide = leftHandSide;
        RightHandSide = rightHandSide;
    }

    public Token Operator { get; }
    public Expression LeftHandSide { get; }
    public Expression RightHandSide { get; }
}

public class UnaryOperator : Expression
{
    public UnaryOperator(Token @operator, Expression operand)
    {
        Operator = @operator;
        Operand = operand;
    }

    public Token Operator { get; }
    public Expression Operand { get; }
}

public class FunctionCall : Expression
{
    public FunctionCall(Token functionName, ArgumentList argList)
    {
        FunctionName = functionName;
        ArgList = argList;
    }

    public Token FunctionName { get; }
    public ArgumentList ArgList { get; }
}

public class Number : Expression
{
    public Number(Token charSequence)
    {
        CharSequence = charSequence;
    }

    public Token CharSequence { get; }
}

public class Name : Expression
{
    public Name(Token charSequence)
    {
        CharSequence = charSequence;
    }

    public Token CharSequence { get; }
}