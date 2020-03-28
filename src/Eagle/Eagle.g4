grammar Eagle;

unit: (function | struct)*;

scriptUnit: (function | struct | stmt)*;

struct: 'struct' Identifier '{' structMember* '}';
structMember: field | property | method;

field: Identifier (':' type)? ('=' expr);
property: Identifier (':' type)? propertyBody;
propertyBody: propertyBlockBody | propertyExprBody;
propertyBlockBody: '{' (getClause | setClause)* '}';
propertyExprBody: exprBody;

getClause: 'get' body;
setClause: 'set' body;

method: Identifier parameterList body;

body: blockBody | exprBody;

blockBody: block;

block: '{' stmt* '}';

stmt:
	ifStmt
	| loopStmt
	| whileStmt
	| forStmt
	| breakStmt
	| continueStmt
	| block
	| exprStmt;

ifStmt: 'if' expr block elseClause?;
elseClause: 'else' (ifStmt | block);

loopStmt: 'loop' block;
whileStmt: 'while' expr block;
forStmt: 'for' Identifier '=' expr 'to' expr block;
breakStmt: 'break';
continueStmt: 'continue';

exprStmt: expr;

exprBody: '=>' expr;

function: 'func' Identifier parameterList body;

parameterList: '(' parameter (',' parameter)* ')';

parameter: Identifier ':' type;

name: Identifier ('.' Identifier)*;

type: name | predefinedType;

predefinedType:
	I8Keyword
	| I16Keyword
	| I32Keyword
	| I64Keyword
	| I128Keyword
	| U8Keyword
	| U16Keyword
	| U32Keyword
	| U64Keyword
	| U128Keyword
	| BoolKeyword
	| StringKeyword;

expr:
	literal
	| identifier
	| expr '.' identifier
	| expr '(' argumentList? ')'
	| ('+' | '-' | '!' | '~') expr
	| expr ('*' | '/' | '%') expr
	| expr ('+' | '-') expr
	| expr ('<' | '<=' | '>' | '>=') expr
	| expr ('==' | '!=') expr
	| expr '&' expr
	| expr '^' expr
	| expr '|' expr
	| expr '&&' expr
	| expr '||' expr
	| <assoc = right> expr '=' expr;

identifier: Identifier;

literal: Number;

argumentList: expr (',' expr)*;

Hat: '^';
Less: '<';
LessEquals: '<=';
Greater: '>';
GreaterEquals: '>=';
EqualsGreater: '=>';
Ampersand: '&';
Pipe: '|';
AmpersandAmpersand: '&&';
PipePipe: '||';
Dot: '.';
Comma: ',';
Underscore: '_';
Colon: ':';
Semicolon: ';';
Plus: '+';
Minus: '-';
Star: '*';
Slash: '/';
Percent: '%';
Tilde: '~';
OpenBrace: '{';
CloseBrace: '}';
OpenParanthesis: '(';
CloseParanthesis: ')';
OpenBracket: '[';
CloseBracket: ']';
Equals: '=';
EqualsEquals: '==';
Bang: '!';
BangEquals: '!=';

TrueKeyword: 'true';
FalseKeyword: 'false';
IfKeyword: 'if';
ElseKeyword: 'else';
StructKeyword: 'struct';
ValueKeyword: 'value';
GetKeyword: 'get';
SetKeyword: 'set';
LoopKeyword: 'loop';
WhileKeyword: 'while';
ForKeyword: 'for';
ToKeyword: 'to';
BreakKeyword: 'break';
ContinueKeyword: 'continue';
FuncKeyword: 'func';

I8Keyword: 'i8';
I16Keyword: 'i16';
I32Keyword: 'i32';
I64Keyword: 'i64';
I128Keyword: 'i128';
U8Keyword: 'u8';
U16Keyword: 'u16';
U32Keyword: 'u32';
U64Keyword: 'u64';
U128Keyword: 'u128';
BoolKeyword: 'bool';
StringKeyword: 'string';

Identifier: Letter ( Letter | Digit)*;
Number: Digit Digit*;

fragment Letter: [_A-Za-z];
fragment Digit: [0-9];

BlockComment: '/*' .*? '*/' -> skip;
LineComment: '//' ~[\r\n]* -> skip;
Whitespace: [ \r\t\n]+ -> skip;