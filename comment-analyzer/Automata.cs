using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comment_analyzer
{
    public class Automata
    {
        public bool isSplitter(char symbol)
        {
            switch (symbol)
            {
                case '.': return true;
                case ',': return true;
                case '!': return true;
                case '?': return true;
                case '(': return true;
                case ')': return true;
                case '{': return true;
                case '}': return true;
                case '[': return true;
                case ']': return true;
                case '+': return true;
                case '-': return true;
                case '=': return true;
                case '\\': return true;
                case '/': return true;
                case '<': return true;
                case '>': return true;
                default: return false;
            }
        }

        public bool isFromGrammar(char symbol)
        {
            if (char.IsLetterOrDigit(symbol) || isSplitter(symbol) || symbol == '*')
                return true;
            return false;
        }

        public int analyzeLine(string input, ref List<string> errors)
        {
            int i = 0;
            int state = 0;
            bool wrongSymbolIsFound = false;

            errors.Clear();

            for (; i < input.Length; i++)
            {
                if (input[i] == '*' && state == 0)
                {
                    state = 1;
                }
                else if (state == 0 && (char.IsLetterOrDigit(input[i]) || isSplitter(input[i])))
                {
                    state = 3;
                }
                else if (!isFromGrammar(input[i]))
                {
                    wrongSymbolIsFound = true;
                    int j = 1;
                    while (input.Length > i + j && !isFromGrammar(input[i+j]))
                        j++;
                    errors.Add(input.Substring(i, j));
                    i += j - 1;
                }
            }

            if (!wrongSymbolIsFound)
            {       //в строке все символы были из алфавита
                if (state == 1)
                {
                    return 0;   //правильная строка
                }
                else if (i == 0)
                {
                    return 1;   //пустая строка
                }
                else if (state == 3)
                {
                    return 2;   //строка началась не со *
                }
            }
            else
            {       //в строке попались символы не из алфавита
                if (state == 1)
                {
                    return 3;   //правильная строка
                }
                else if (i == 0)
                {
                    return 4;   //пустая строка
                }
                else if (state == 3)
                {
                    return 5;   //строка началась не со *
                }
            }

            return 0;
        }
    }
}
