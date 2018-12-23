
# have loop state variable keywords
first : bool -> true in first iteration
last : bool -> true in last iteration # Possible for iterators ?

Only emit the constructs for them if the keywords have been used. Maybe they should also support named loops
-> frist name 

# have a concept to break the outer loop
loop name {
	break;
	continue;
	break name;
	continue name;
}

for let item in items name {
    break;
	continue;
    break name;
    continue name;
}

while true name {
    break;
	continue;
    break name;
    continue name;
}


x = int?

a = x ? b : c;
a = x != null ? b : null;

a = x ? b; -> typeof(b)?;

a = x? .a?.b; -> typeof(b)?

a = x ?? 4;
a = x ?? throw new E();
a = x ?? return 5;

a = x ?? 4; -> int

if let a = x {
	a -> int
}

if ( x != null) {
	Something();
}

throw = () => !

# statements

stmt 
	VariableDeclStmt
	If
	BreakStmt
	ExprStmt
	LoopStmt
	WhileStmt
	ContinueStmt
	
loop 
    'loop' Block

if
    'if' Cond Block 'else' If | Block 

WhileStmt
    'while' Cond Block

BreakStmt
    'break'

ContinueStmt
    'continue'
	   
Cond 
	expr

# expression
expr 
    '+' | '-' | '!' expr 
    expr '+' | '-' expr
	expr '*' | '/' expr
	expr '==' | '!=' expr
	expr '&&' expr
	expr '||' expr
	'(' expr ')'
	BlockExpr
	identifier
	literal

	# todo
	expr '??' expr     # coalces
	expr '?'           # check expr - left assoc -> expr == nil ? nil : expr
	expr '?' expr      # xor expr - can this be done ?
	expr '?' expr ':'  # cond expr

BlockExpr
    '{' (stmt ('\n\r' stmt)*)? '}'
	
Literal
    NumberLiteral | BooleanLiteral | StringLiteral

StringLiteral
    '"' [^'"']* '"' 

NumberLiteral 
    Digit (Digit)*

BooleanLiteral
    TrueKeyword
	FalseKeyword

# token

Identifier
    Letter (LetterDigit)*

LetKeyword       'let' # could be var ?
WhileKeyword     'while'
LoopKeyword      'loop'
ContinueKeyword  'continue'
BreakKeyword     'break'
IfKeyword        'if'
ElseKeyword      'else'
TrueKeyword      'true'
FalseKeyword     'false'
OpenParenthesis  '('
CloseParenthesis ')'

Equals       '='
Bang         '!'
EqualsEquals '=='
BangEquals   '!='

AmpersandAmpersand '&&'
PipePipe           '||'


# these could be keywords of the syntax definition language
letterOrDigit
    letter | digit

letter 
    _ | 'A'..'Z' | 'a'..'z'

digit
    '0'..'9'

whitespace
    ' ' | '\t' | '\n' | '\r'