# SPA (CS)
This was originally  an
[NUS Project](https://nus-cs3203.github.io/course-website/contents/basic-spa-requirements/simple-programming.html).
The task was to write a Static Program Analyzer for a toy programming language using C++.

This is a re-do of that project (at least, the Lexer/Parser part) in C# .NET 7.0,
and introduces some enhancements.

## Grammar
```text
LETTER          = [A-Z] | [a-z] ;
DIGIT           = [0-9] ;
NZDIGIT         = [1-9] ; 
NAME            = (LETTER | "_") (LETTER | DIGIT | "_")* ;

SIGN            = "-" | "+";
INTEGER         = DIGIT+ ;
FRACTION        = "." DIGIT+ ;
EXPONENT        = ("e" | "E") SIGN? INTEGER ;
NUMBER          = SIGN? INTEGER FRACTION? EXPONENT? ;

program         = function+ ;

function        = "function" function_decl "{" stmt+ "}" ;

function_decl   = NAME "(" arg_list ")" ;

arg_list        = ""
                | NAME ("," NAME)* ;
                
stmt            = if | while | assign | return ;

if              = "if" "(" cond_expr ")" "{" stmt+ "}" else_if* else? ;

else_if         = "else" "if" "(" cond_expr ")" "{" stmt+ "}"

else            = "else" "{" stmt+ "}" ;

while           = "while" "(" expr ")" "{" stmt+ "}" ;

assign          = "let" NAME "=" expr ";" ;

return          = "return" expr ;

cond_expr       = "!" "(" cond_expr ")"
                | cond_expr ("&&" | "||") cond_expr
                | rel_expr ;

rel_expr        = rel_factor ("<" | ">" | "<=" | ">=" | "==" | "!=") rel_factor

rel_factor      = expr
                | NUMBER
                | NAME ;

expr            = expr ("+" | "-") term
                | term ;

term            = term ("*" | "/" | "%") factor
                | factor ;
                
factor          = "(" expr ")"
                | NUMBER
                | NAME
                | function_call ;
                
function_call   = NAME "(" arg_list ")" ;
```