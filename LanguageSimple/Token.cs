using System;
using System.Diagnostics;
using System.Linq;
using SPA.Utility;

namespace SPA;

public enum TokenType
{
    Identifier,
    Number,
    
    KeywordFunction,
    KeywordIf,
    KeywordWhile,
    KeywordElse,
    KeywordLet,
    KeywordReturn,
    
    MathAdd,
    MathSub,
    MathMul,
    MathDiv,
    MathMod,
    MathAnd,
    MathOr,
    MathLt,
    MathGt,
    MathLeq,
    MathGeq,
    MathEq,
    MathNeq,
    
    CtrlSep,
    CtrlAssign,
    CtrlLeftParen,
    CtrlRightParen,
    CtrlLeftBrace,
    CtrlRightBrace,
    CtrlComma,
}

public class Token
{
    public static readonly IBiMap<TokenType, string> TokenMap = new Func<BiMap<TokenType, string>>(() =>
    {
        var biMap = new BiMap<TokenType, string>();
        biMap.AddPair(TokenType.KeywordFunction, "function");
        biMap.AddPair(TokenType.KeywordWhile, "while");
        biMap.AddPair(TokenType.KeywordElse, "else");
        biMap.AddPair(TokenType.KeywordIf, "if");
        biMap.AddPair(TokenType.KeywordLet, "let");
        biMap.AddPair(TokenType.KeywordReturn, "return");
        biMap.AddPair(TokenType.MathAdd, "+");
        biMap.AddPair(TokenType.MathSub, "-");
        biMap.AddPair(TokenType.MathMul, "*");
        biMap.AddPair(TokenType.MathDiv, "/");
        biMap.AddPair(TokenType.MathMod, "%");
        biMap.AddPair(TokenType.MathAnd, "&");
        biMap.AddPair(TokenType.MathOr, "|");
        biMap.AddPair(TokenType.MathLt, "<");
        biMap.AddPair(TokenType.MathLeq, "<=");
        biMap.AddPair(TokenType.MathGt, ">");
        biMap.AddPair(TokenType.MathGeq, ">=");
        biMap.AddPair(TokenType.MathEq, "==");
        biMap.AddPair(TokenType.MathNeq, "!=");
        biMap.AddPair(TokenType.CtrlLeftBrace, "{");
        biMap.AddPair(TokenType.CtrlRightBrace, "}");
        biMap.AddPair(TokenType.CtrlLeftParen, "(");
        biMap.AddPair(TokenType.CtrlRightParen, ")");
        biMap.AddPair(TokenType.CtrlSep, ";");
        biMap.AddPair(TokenType.CtrlAssign, "=");
        biMap.AddPair(TokenType.CtrlComma, ",");
        return biMap;
    }).Invoke();
    
    public string CharSeq { get; }
    public TokenType Type { get; }

    private Token(string charSeq, TokenType type)
    {
        CharSeq = charSeq;
        Type = type;
    }

    public static Token Of(string token)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(token));
        if (TokenMap.TryGetRightToLeft(token, out var tokenType))
        {
            return new Token(token, tokenType);
        }
        if ((char.IsLetter(token[0]) || token[0] == '_') && token.Skip(1).All(x => char.IsLetterOrDigit(x) || x == '_'))
        {
            return new Token(token, TokenType.Identifier);
        }
        if (token.Length == 1 && token[0] == '0')
        {
            return new Token(token, TokenType.Number);
        }
        if (char.IsBetween(token[0], '1', '9') && token.Skip(1).All(char.IsDigit))
        {
            return new Token(token, TokenType.Number);
        }
        throw new ArgumentException($"unrecognized token \"{token}\"");
    }

    public bool Is(string charSeq)
    {
        return CharSeq == charSeq;
    }

    public bool Is(TokenType type)
    {
        return Type == type;
    }

    public override string ToString()
    {
        return $"Token({Type}, \"{CharSeq}\")";
    }
}