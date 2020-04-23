// how to write a program in a language that does not yet exist ?
// we do not have a base class library nor a standard library
// we dont even have a string type

// a simple lexer without any fluff

object Token {

}

object Lexer {

    _position: int;
    _text: string;
//
    Current : char => _text[_position];
//
    Next() {
        _index = _index + 1;
    }
//
    ctor(text: string) {
        _text = text;
    }
//
    Lex() : Token {
        let current = Current;
        Next();

//        if IsIdentifierStart(current) {

//            while IsLetterOrDigit(Current) {
//                Next();
//            }
//        }
//        else if current == '"' {
            // read string
//        }

        return Token();
    }
//
    IsIdentifierStart(c: char) {
        return c == '_' ||
          ( c >= 'A' && c <= 'Z') ||
          (c >= 'a' && c <= 'z');
    }
//
    IsIdentifierFollow(c: char) {
        return IsIdentifierStart(c) ||
        (c >= '0' && c <= '9');
    }
}

let lexer = Lexer("hello");
let token = lexer.Lex();

extern Print(s: string);

Print("Hello World!");