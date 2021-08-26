using ETALib.Models;
using ETALib.Requests;
using IASK.InterviewerEngine;
using IASK.InterviewerEngine.Models.Output;
using System.Collections.Generic;
using System.Linq;
using UMKBRequests.Models.API.Satellite;

namespace ETIntegration
{
    public static class DataConverters
    {
        private static InterfaceUnit ConverFromQuestionRequest(Question question, string name)
        {
            InterfaceUnit interfaceUnit = new InterfaceUnit();
            interfaceUnit.Type = (InterfaceUnit.UnitType)question.QuestionType;
            interfaceUnit.Id = question.Id.ToString();
            
            interfaceUnit.Label = string.IsNullOrEmpty(name)? question.Id.ToString():name;
            return interfaceUnit;
        }
        private static InterfaceUnit ConverFromTable(TableTab tableTab)
        {
            InterfaceUnit table = new InterfaceUnit();
            table.Type = InterfaceUnit.UnitType.TABLE;
            table.Label = tableTab.Name;
            InterfaceUnit headers = new InterfaceUnit();
            headers.Type = InterfaceUnit.UnitType.HEADERS;
            table.Units.Add(headers);
            for (int i =0; i< tableTab.Headers.Count;i++)
            {
                ColumnHeader header = tableTab.Headers[i];
                InterfaceUnit column = new InterfaceUnit();
                column.Type = InterfaceUnit.UnitType.HEADER;
                column.Label = header.Name;
                headers.Units.Add(column);
            }
            foreach (TableRow tableRow in tableTab.Rows)
            {
                InterfaceUnit row = new InterfaceUnit();
                row.Type = InterfaceUnit.UnitType.ROW;
                foreach (string cell in tableRow.Cells)
                {
                    InterfaceUnit Cell = new InterfaceUnit();
                    Cell.Type = InterfaceUnit.UnitType.CELL;
                    Cell.Label = cell;
                    Cell.Value = cell;
                    Cell.Type = InterfaceUnit.UnitType.CELL;
                    row.Units.Add(Cell);
                }
                table.Units.Add(row);
            }
            return table;
        }
        public static List<InterfaceUnit> ConvertQuestions(QuestionRequest questions, string key)
        {
            List<InterfaceUnit> result = new List<InterfaceUnit>();
            List<ulong> ids = questions.Questions.Select(item => item.Id).ToList();
            GetNamesResponse getNameseResponse =  CacheForming.GetNames(key, ids);
            int i = 0;
            foreach (Question question in questions.Questions)
            {
                List<UMKBRequests.Models.API.Satellite.Name> names = getNameseResponse.names[i];
                result.Add(ConverFromQuestionRequest(question, names.Count>0?names[0].text:string.Empty));
                i++;
            }
            return result;
        }
        public static List<InterfaceUnit> ConvertTable(SubmitTableRequest tableRequest)
        {
            List<InterfaceUnit> result = new List<InterfaceUnit>();
            foreach (var tab in tableRequest.Tabs)
            {
                result.Add(ConverFromTable(tab));
            }
            return result;
            

        }
    }
}
