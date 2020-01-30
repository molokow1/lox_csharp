using System;
using System.Collections.Generic;

namespace lox_csharp_interpreter
{

    class Scanner
    {
        private string source;
        private List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        private Dictionary<string, TokenType> identifierKeywords;
        public Scanner(string source)
        {
            this.source = source;
            identifierKeywords = new Dictionary<string, TokenType>
            {
                {"and", TokenType.AND},
                {"class", TokenType.CLASS},
                {"else", TokenType.ELSE},
                {"false", TokenType.FALSE},
                {"for", TokenType.FOR},
                {"fun", TokenType.FUN},
                {"if", TokenType.IF},
                {"nil", TokenType.NIL},
                {"or", TokenType.OR},
                {"print", TokenType.PRINT},
                {"return", TokenType.RETURN},
                {"super", TokenType.SUPER},
                {"this", TokenType.THIS},
                {"true", TokenType.TRUE},
                {"var", TokenType.VAR},
                {"while", TokenType.WHILE},
            };
        }

        public List<Token> scanTokens()
        {
            while (!isAtEnd())
            {
                start = current;
                scanToken();
            }
            tokens.Add(new Token(TokenType.EOF, "", null, line));

            return tokens;
        }

        private void scanToken()
        {
            char c = advanceChar();
            switch (c)
            {
                case '(': addToken(TokenType.LEFT_PAREN); break;
                case ')': addToken(TokenType.RIGHT_PAREN); break;
                case '{': addToken(TokenType.LEFT_BRACE); break;
                case '}': addToken(TokenType.RIGHT_BRACE); break;
                case ',': addToken(TokenType.COMMA); break;
                case '.': addToken(TokenType.DOT); break;
                case '-': addToken(TokenType.MINUS); break;
                case '+': addToken(TokenType.PLUS); break;
                case ';': addToken(TokenType.SEMICOLON); break;
                case '*': addToken(TokenType.START); break;
                case '!': addToken(matchNextChar('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '=': addToken(matchNextChar('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<': addToken(matchNextChar('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': addToken(matchNextChar('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER_EQUAL); break;

                case '/':
                    if (matchNextChar('/'))
                    {
                        //comment
                        while (peekChar() != '\n' && !isAtEnd())
                        {
                            advanceChar();
                        }
                    }
                    else
                    {
                        addToken(TokenType.SLASH);
                    }
                    break;

                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.                      
                    break;

                case '\n':
                    line++;
                    break;

                case '"': scanString(); break;
                default:
                    if (Char.IsDigit(c))
                    {
                        scanNumber();
                    }
                    else if (Char.IsLetter(c))
                    {
                        scanIdentifier();
                    }
                    else
                    {
                        Lox.error(line, "Unexpected character.");
                    }
                    break;
            }

        }

        private char advanceChar()
        {
            current++;
            return source[current - 1];
        }

        private bool matchNextChar(char expected)
        {
            if (isAtEnd())
            {
                return false;
            }

            if (source[current] != expected)
            {
                return false;
            }

            current++;
            return true;
        }

        private char peekChar()
        {
            if (isAtEnd()) return '\0';

            return source[current];
        }

        private char peekNextChar()
        {
            if (current + 1 >= source.Length)
            {
                return '\0';
            }

            return source[current + 1];
        }

        private void scanString()
        {
            while (peekChar() != '"' && !isAtEnd())
            {
                if (peekChar() == '\n')
                {
                    line++;
                }
                advanceChar();
            }

            if (isAtEnd())
            {
                Lox.error(line, "Unterminated string.");
                return;
            }

            advanceChar();

            string value = source.Substring(start + 1, current - start - 2);
            addToken(TokenType.STRING, value);
        }

        private void scanNumber()
        {
            while (Char.IsDigit(peekChar()))
            {
                advanceChar();
            }

            if (peekChar() == '.' && Char.IsDigit(peekNextChar()))
            {
                advanceChar();
                while (Char.IsDigit(peekChar()))
                {
                    advanceChar();
                }
            }

            addToken(TokenType.NUMBER, Double.Parse(source.Substring(start, current - start)));
        }

        private void scanIdentifier()
        {
            while (Char.IsLetterOrDigit(peekChar()))
            {
                advanceChar();
            }

            string text = source.Substring(start, current - start);
            TokenType val;
            if (identifierKeywords.TryGetValue(text, out val))
            {
                addToken(val);
            }
            else
            {
                addToken(TokenType.IDENTIFIER);
            }
        }

        private void addToken(TokenType type)
        {
            addToken(type, null);
        }

        private void addToken(TokenType type, Object literal)
        {
            string text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

        private bool isAtEnd()
        {
            return current >= source.Length;
        }
    }
}