﻿
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Memory;

namespace cssparser
{
    static class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllText(Directory.GetCurrentDirectory() + "/res/simple.css");

            lines = Preprocess(lines);

            /* foreach (var line in lines) {
                Console.WriteLine(line);
            }*/

            Tokenize(lines);
        }

        // See https://www.w3.org/TR/css-syntax-3/#input-preprocessing
        static string Preprocess(string input) {
            return input.Replace("\u000D\u000D\u000A", "\u000A").
                    Replace('\u000D', '\u000A').
                    Replace('\u000C', '\u000A').
                    Replace('\u0000', '\uFFFD');
        }

        // See https://www.w3.org/TR/css-syntax-3/#tokenizer-algorithms for reference
        static void Tokenize(string input) {
            var globalTokens = new List<Tokens[]>();

            var inputTokens = new List<Tokens>();

            for (int j = 0; j < input.Length; j++)
            {
                var currToken = input[j];

                if (Char.IsWhiteSpace(currToken)) {
                    inputTokens.Add(Tokens.whitespaceToken);
                    continue;
                }

                if (currToken == '\u0022') { // '\u0022' = '"'
                    
                }
            }
        }

        // See https://www.w3.org/TR/css-syntax-3/#consume-a-string-token
        static (StringToken token, int offset) TokenizeString(string line) {
            var token = new StringToken("");
            int i = 0;

            for (; i < line.Length; i++)
            {
                if (line[i] == '\n') {
                    token.token = Tokens.badStringToken;
                    break;
                }

                if (line[i] == '\\')
                {
                    if (line[i+1] == '\n')
                    {
                        i++;
                        continue;
                    }

                    if (!CheckEscape(line.Substring(i)));

                    char escapedCodePoint = TokenizeEscape(line.Substring(i));

                    if (escaped)
                    {
                        
                    }
                }
            }

            return (token, i);
        }

        // See https://www.w3.org/TR/css-syntax-3/#consume-an-escaped-code-point
        static char TokenizeEscape(string line) {

            var hexDigits = 0;
            var hexDigitsCount = 0;

            char codePoint = '\uFFFD';
            
            for (int i = 0; i < line.Length; i++)
            {
                if (IsHexDigit(line[i])) {
                    hexDigits += (int)line[i];
                    hexDigitsCount++;

                    if (hexDigitsCount == 5) {
                        codePoint = (char)hexDigits;

                        if (hexDigits == 0 ||                                    // If it's a null character escape
                            codePoint.IsBetween((int)'\ud800', (int)'\udfff') || // Or if it's a surrogate code point
                            codePoint.IsBetween(1114111, Char.MaxValue))         // Or if it's 
                        {
                            return '\uFFFD';
                        }

                        return codePoint;
                    }
                } else {
                    Console.WriteLine("Escaped character \'" + line[i] + "\' because it isn't an hex digit");
                    codePoint = line[i];
                }
            }

            return codePoint;
        }

        // See https://www.w3.org/TR/css-syntax-3/#starts-with-a-valid-escape
        static bool CheckEscape(string line) {
            if (line[0] != '\\')
            {
                return false;
            }

            if (line[1] == '\n') {
                return false;
            }

            return true;
        }

        // See https://www.w3.org/TR/css-syntax-3/#hex-digit
        static bool IsHexDigit(char codePoint) {

            return codePoint.IsBetween(65, 70)  ||  // If it's between 'A' and 'F'
                   codePoint.IsBetween(97, 102) ||  // Or if it's between 'a' and 'f'
                   Char.IsDigit(codePoint);         // Or if it's a digit
        }

        static bool IsBetween(this char codePoint, int min, int max, bool inclusive=true) {
            int cpInt = (int)codePoint;

            if (inclusive) {
                return (cpInt >= min && cpInt <= max) ? true : false;
            } else {
                return (cpInt > min && cpInt < max) ? true : false;
            }
        }
    }
}
