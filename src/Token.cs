using System;

namespace lox_csharp_interpreter
{
    enum TokenType
    {
        // Single-char tokens,
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
        COMMA, DOT, PLUS, MINUS, SEMICOLON, SLASH, START,

        // One or two char tokens
        BANG, BANG_EQUAL, EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL, LESS, LESS_EQUAL,

        // Literals
        IDENTIFIER, STRING, NUMBER,

        // Keywords,
        AND, CLASS, ELSE, FALSE,
        FUN, FOR, IF, NIL, OR,
        PRINT, RETURN, SUPER,
        THIS, TRUE, VAR, WHILE,

        EOF
    }

    class Token
    {
        private readonly TokenType type;
        private readonly string lexeme;
        private readonly Object literal;
        private readonly int line;

        public Token(TokenType type, string lexeme, Object literal, int line)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        public override string ToString()
        {
            return type + " " + lexeme + " " + literal;
        }
    }
}