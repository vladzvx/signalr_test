using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace IASK.Cases.JsonStorage.Utils
{
    public class JsonProcessor
    {
        private Regex permitRegex = new Regex("\"Permit\":{\"authKey\":\"(\\w+)\",\"service\":\"(\\w+)\"}");
        private Regex IdRegex = new Regex("\"JsonId\":(\\d+)");
        private Regex DataRegex = new Regex("\"Data\":");
        public JsonProcessor()
        {

        }
        public string GetDataJson(string input)
        {
            char startSymbol = ' ';
            char endSymbol = ' ';
            int index = 0;
            for (int i=0;i< input.Length; i++)
            {
                if (input[i]=='{')
                {
                    startSymbol = input[i];
                    endSymbol = '}';
                    index = i;
                    break;
                }
                if (input[i] == '[')
                {
                    startSymbol = input[i];
                    endSymbol = ']';
                    index = i;
                    break;
                }
            }
            if (startSymbol == ' ') throw new ArgumentException("Input data don't contains json");
            int openCount = 0;
            int closeCount = 0;
            int closeIndex = 0;
            for (int i = index; i < input.Length; i++)
            {
                if (input[i] == startSymbol)
                {
                    openCount++;
                }
                else if (input[i] == endSymbol)
                {
                    closeCount++;
                }
                if (openCount== closeCount)
                {
                    closeIndex = i;
                    break;
                }
            }
            char b1 = input[index];
            char b2 = input[closeIndex];
            return input.Substring(index, closeIndex-index+1);
        }


        public string GetData(string input)
        {
            Match matchData = DataRegex.Match(input);
            if (matchData.Success)
            {
                return GetDataJson(input.Substring(matchData.Index));
            }
            else throw new ArgumentException("Json doesn't content Data!"); ;
        }

        public (string AuthKey,string Service) GetAuthData(string input)
        {
            var match = permitRegex.Match(input);
            if (match.Success)
            {
                return (match.Groups[1].Value, match.Groups[2].Value);
            }
            else throw new ArgumentException("Json doesn't content Permit!");
        }

        public bool TryGetId(string input, out long Id)
        {
            Id = 0;
            var match = IdRegex.Match(input);
            if (match.Success && long.TryParse(match.Groups[1].Value, out Id))
            {
                return true;
            }
            else return false;
        }
    }
}
