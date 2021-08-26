using IASK.Common.Services;
using IASK.InterviewerEngine;
using IASK.InterviewerEngine.Models.Input;
using IASK.InterviewerEngine.Models.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UMKBRequests;

namespace IASK.InterviewerEngineTests
{

    public partial class InterviewerTests
    {
        #region Common test methods

        [TestMethod]
        public static void CheckPatientsPage(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));
        }

        [TestMethod]
        public static Report[] CheckResults(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ������������", out Report report1));
            Report[] result = new Report[1] { report1 };
            return result;
        }


        [TestMethod]
        public static Report[] CheckAdditionalFactors(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ��������� � ��� �������������� �������", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������", out Report report2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� ��������������������� ��� ������ ����������������� (������������ � �.�.)", out Report report3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���������� ������� � ������� ��������� (������ 65 ���)", out Report report4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report5));
            Report[] result = new Report[5] { report1, report2, report3, report4, report5 };
            return result;
        }


        [TestMethod]
        public static Report[] CheckChronicles(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� ����������� ����������� �� ����������������� ������� � ���?", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������������� ������� (��� ���������� ������������ ��������)", out Report report2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����������� ����������� ������", out Report report3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ������������������� �������", out Report report4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report5));
            Report[] result = new Report[5] { report1, report2, report3, report4, report5 };
            return result;
        }

        [TestMethod]
        public static Report[] CheckChronicles2(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ����������� �����������", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����������� ������������� ������� ������ (����)", out Report report2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� �����", out Report report3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������� ������", out Report report4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report5));
            Report[] result = new Report[5] { report1, report2, report3, report4, report5 };
            return result;
        }

        [TestMethod]
        public static Report[] CheckSimpLast2Week(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���������� �������� � ����� (��������)", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� � ������", out Report report2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� � �����", out Report report3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������� ����������� ���� �� ", out Report report4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������� ����������� ���� ���� ", out Report report5));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������, ����� ��� � ��������� ����������� �������", out Report report6));
            Report[] result = new Report[6] { report1, report2, report3, report4, report5, report6 };
            return result;

        }
        [TestMethod]
        public static Report[] CheckCurrentSimptoms(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� �������� �� ����������������� ����������� � ��� �� ������ ������?", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� (�������� ������ ���������������� �������� ������)", out Report report2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������", out Report report3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ������������ � ������� ������", out Report report4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ (������� �������� �������, ������ ��� ��������� ������� � �����)", out Report report5));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������, ��������", out Report report6));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������� ����������� ���� ���� 38.5", out Report report7));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report8));
            Report[] result = new Report[8] { report1, report2, report3, report4, report5, report6, report7, report8 };
            return result;
        }

        [TestMethod]
        public static Report[] CheckAge(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� �������", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "36 - 60", out Report report2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "61 - 74", out Report report3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ 90", out Report report4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "75 - 90", out Report report5));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "17 - 23", out Report report6));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "24 - 35", out Report report7));
            Report[] result = new Report[7] { report1, report2, report3, report4, report5, report6, report7 };
            return result;
        }

        [TestMethod]
        public static Report[] CheckAdditional2(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� � ��������� ��� ����", out Report report2));
            Report[] result = new Report[2] { report1, report2 };
            return result;
        }

        [TestMethod]
        public static Report[] CheckBlood(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ ������ ����� I (0)", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ ������ ����� II (�)", out Report report2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ ������ ����� III (�)", out Report report3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������� ������ ����� IV (AB)", out Report report4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�� ����", out Report report5));
            Report[] result = new Report[5] { report1, report2, report3, report4, report5 };
            return result;
        }

        [TestMethod]
        public static Report[] CheckEvents(InterfaceUnit tempUnit)
        {

            Assert.IsTrue(ReqursiveFinder(tempUnit, null,
                "��� ������� � ������, �������� �������� �������� � ���� (��������� �� ����, ������, ������� � ��.)", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������� � ������� � �������������� ��������� COVID-19", out Report report2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report3));
            Report[] result = new Report[3] { report1, report2, report3 };
            return result;

        }

        [TestMethod]
        public static Report[] CheckWork(InterfaceUnit tempUnit)
        {

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ����� ����� ������", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�� �������", out Report report3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ��������", out Report report4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ ��� ��������� (�� �����)", out Report report5));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ����� ������", out Report report6));
            Report[] result = new Report[6] { report1, report2, report3, report4, report5, report6 };
            return result;

        }

        [TestMethod]
        public static Report[] CheckMoving(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������ ���������", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������", out Report report2));
            Report[] result = new Report[2] { report1, report2 };
            return result;
        }

        [TestMethod]
        public static Report[] CheckPrevention(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� � ��������� ��� ����", out Report report2));
            Report[] result = new Report[2] { report1, report2 };
            return result;

        }

        [TestMethod]
        public static Report[] CheckAdditional(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��� ������� � ������, �������� �������� �������� � ���� (��������� �� ����, ������, ������� � ��.)", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������� � ������� � �������������� ��������� COVID-19", out Report report2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report3));
            Report[] result = new Report[3] { report1, report2, report3 };
            return result;
        }

        [TestMethod]
        public static Report[] CheckSex(InterfaceUnit tempUnit)
        {
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������", out Report report1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������", out Report report2));
            Report[] result = new Report[2] { report1, report2 };
            return result;
        }
        #endregion

        #region Assistance static methods

        public static bool ReqursiveFinder(InterfaceUnit interfaceUnit, string parent_id, string text, out Report report)
        {
            report = new Report
            {
                label = null,
                value = null,
                idb = null,
                id = null
            };
            if (interfaceUnit.Label != null && interfaceUnit.Label.StartsWith(text))
            {
                report.value = interfaceUnit.Id;
                report.label = interfaceUnit.Label;
                report.idb = interfaceUnit.Idb;
                report.id = interfaceUnit.Id;
                //id = interfaceUnit.id;
                return true;
            }
            else
            {
                bool res = false;
                if (interfaceUnit.Units != null)
                {
                    foreach (InterfaceUnit iu in interfaceUnit.Units)
                    {
                        if (ReqursiveFinder(iu, interfaceUnit.Idb, text, out report))
                        {
                            return true;
                        }
                    }
                    return res;
                }
                else return false;

            }
        }

        public static bool ReqursiveFinder(InterfaceUnit interfaceUnit, string text, out InterfaceUnit report)
        {
            report = null;
            if (interfaceUnit.Label != null && interfaceUnit.Label.StartsWith(text))
            {
                report = interfaceUnit;
                return true;
            }
            else
            {
                bool res = false;
                if (interfaceUnit.Units != null)
                {
                    foreach (InterfaceUnit iu in interfaceUnit.Units)
                    {
                        if (ReqursiveFinder(iu, text, out report))
                        {
                            return true;
                        }
                    }
                    return res;
                }
                else return false;
            }
        }

        internal static Answer CreateAnswer(string TriggerId, params Report[] report)
        {
            List<InputStruct> buffer = new List<InputStruct>();
            foreach (Report rep in report)
            {
                buffer.Add(new InputStruct()
                {
                    Id = rep.id,
                    Idb = rep.idb,
                    Value = rep.value,
                });
            }
            Answer ans = new Answer()
            {
                TriggerId = TriggerId,
                CheckerResponse = buffer.ToArray()
            };
            return ans;
        }


        internal static Answer SetAnsver(Interviewer eTSession, InterviewerState state, string TriggerId, params Report[] report)
        {
            List<InputStruct> buffer = new List<InputStruct>();
            foreach (Report rep in report)
            {
                buffer.Add(new InputStruct()
                {
                    Id = rep.id,
                    Idb = rep.idb,
                    Value = rep.value,
                });
            }
            Answer ans = new Answer()
            {
                TriggerId = TriggerId,

                CheckerResponse = buffer.ToArray()
            };
            eTSession.SetAnswer(state, ans);
            return ans;
        }

        internal static Answer SetAnsver(Interviewer eTSession, InterviewerState state, Report report)
        {
            Answer ans = new Answer()
            {
                TriggerId = state.CurrentQuestionTriggerID.ToString(),
                CheckerResponse = new InputStruct[1]
                {
                    new InputStruct()
                    {
                        Id = report.id,
                        Idb = report.idb,
                        Value = report.value,
                        //Labels= new string[1] { report.label }
                    }
                }
            };
            eTSession.SetAnswer(state, ans);
            return ans;
        }

        #endregion
    }

    [TestClass]
    public partial class InterviewerTests
    {
        static Interviewer EMC1;
        static Interviewer CheckerTemplate3;
        static Interviewer Reception;
        static Interviewer DoctorQuestioning;
        static Interviewer OnePage;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            KeyProvider keyProvider = new KeyProvider();
            Interviewer.Factory Factory = new Interviewer.Factory(new KeyProvider()) ;
            Assert.IsTrue(Factory.TryGetInterviewer(IdParser.GetNewBigId(81, 8), out EMC1));

            OnePage = Factory.Create(IdParser.ParseBigIdToNewBigId(860007076), keyProvider.SemanticKey, keyProvider.DescriptionsListKey, keyProvider.VarcharListKey, keyProvider.BoolSattliteKey, keyProvider.GetNamesKey);
            Reception = Factory.Create(IdParser.ParseBigIdToNewBigId(860006910), keyProvider.SemanticKey, keyProvider.DescriptionsListKey, keyProvider.VarcharListKey, keyProvider.BoolSattliteKey, keyProvider.GetNamesKey);
            CheckerTemplate3 = Factory.Create(IdParser.ParseBigIdToNewBigId(860000506), keyProvider.SemanticKey, keyProvider.DescriptionsListKey, keyProvider.VarcharListKey, keyProvider.BoolSattliteKey, keyProvider.GetNamesKey);
            DoctorQuestioning = Factory.Create(IdParser.ParseBigIdToNewBigId(860006408), keyProvider.SemanticKey, keyProvider.DescriptionsListKey, keyProvider.VarcharListKey, keyProvider.BoolSattliteKey, keyProvider.GetNamesKey);

        }


        [TestMethod]
        public void Test1_by_Coronastop24()
        {
            DateTime dt1 = DateTime.UtcNow;
            Interviewer temp = CheckerTemplate3;//.Copy();
            DateTime dt2 = DateTime.UtcNow;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.dialog_patient;
            temp.SetAnswer(state);
            temp.GetResponse(state);
            Answer answer = new Answer();
            answer.TriggerId = state.CurrentQuestionTriggerID.ToString();
            temp.SetAnswer(state, answer);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckAge(tempUnit)[4]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckSex(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckBlood(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null,
                "��� ������� � ������, �������� �������� �������� � ���� (��������� �� ����, ������, ������� � ��.)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������� � ������� � �������������� ��������� COVID-19", out Report report4_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));


            SetAnsver(temp, state, report4_2);

            //  List<InterfaceUnit> response5 = temp.GetResponse(state);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report5_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� � ��������� ��� ����", out _));

            SetAnsver(temp, state, report5_1);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���������� �������� � ����� (��������)", out Report report6_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� � ������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� � �����", out _));

            SetAnsver(temp, state, report6_1);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� �������� �� ����������������� ����������� � ��� �� ������ ������?", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� (�������� ������ ���������������� �������� ������)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ������������ � ������� ������", out Report report7_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));

            SetAnsver(temp, state, report7_4);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� ����������� ����������� �� ����������������� ������� � ���?", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������������� ������� (��� ���������� ������������ ��������)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����������� ����������� ������", out Report report8_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ������������������� �������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));

            SetAnsver(temp, state, report8_3);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ����������� �����������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����������� ������������� ������� ������ (����)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� �����", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������� ������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));


            DateTime dt3 = DateTime.UtcNow;
            _ = dt3.Subtract(dt3).TotalMilliseconds;
        }

        /// <summary>
        /// ������ �������� � ������ ����������� ������������
        /// </summary>
        [TestMethod]
        public void Test2_by_Coronastop24()
        {
            Interviewer temp = CheckerTemplate3;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.dialog_patient;
            temp.SetAnswer(state);
            List<InterfaceUnit> response0 = temp.GetResponse(state);
            string TriggerId0 = state.CurrentQuestionTriggerID.ToString(); ;
            Answer answer = new Answer();
            answer.TriggerId = TriggerId0;
            temp.SetAnswer(state, answer);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckAge(tempUnit)[4]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckSex(tempUnit)[1]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckBlood(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null,
                "��� ������� � ������, �������� �������� �������� � ���� (��������� �� ����, ������, ������� � ��.)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������� � ������� � �������������� ��������� COVID-19", out Report report4_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));

            SetAnsver(temp, state, report4_2);

            // List<InterfaceUnit> response5 = temp.GetResponse(state);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report5_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� � ��������� ��� ����", out _));

            SetAnsver(temp, state, report5_1);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���������� �������� � ����� (��������)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ��������, ������� �� ��������� � ���� �� ��������� ��� ������", out Report report6_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� � ������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� � �����", out _));

            SetAnsver(temp, state, report6_1);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };


            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� �������� �� ����������������� ����������� � ��� �� ������ ������?", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� (�������� ������ ���������������� �������� ������)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ������������ � ������� ������", out Report report7_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));

            SetAnsver(temp, state, report7_4);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� ����������� ����������� �� ����������������� ������� � ���?", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������������� ������� (��� ���������� ������������ ��������)", out Report report8_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����������� ����������� ������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ������������������� �������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));

            SetAnsver(temp, state, report8_2);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ��������� � ��� �������������� �������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� ��������������������� ��� ������ ����������������� (������������ � �.�.)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���������� ������� � ������� ��������� (������ 65 ���)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));
        }

        /// <summary>
        /// ������ �������� �������
        /// </summary>
        [TestMethod]
        public void Test3_by_Coronastop24()
        {
            Interviewer temp = CheckerTemplate3;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.dialog_patient;
            temp.SetAnswer(state);
            string TriggerId0 = state.CurrentQuestionTriggerID.ToString();
            Answer answer = new Answer();
            answer.TriggerId = TriggerId0;
            temp.SetAnswer(state, answer);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckAge(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckSex(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckBlood(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null,
                "��� ������� � ������, �������� �������� �������� � ���� (��������� �� ����, ������, ������� � ��.)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������� � ������� � �������������� ��������� COVID-19", out Report report4_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));

            SetAnsver(temp, state, report4_2);

            //  List<InterfaceUnit> response5 = temp.GetResponse(state);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report5_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� � ��������� ��� ����", out _));

            SetAnsver(temp, state, report5_1);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���������� �������� � ����� (��������)", out Report report6_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� � ������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� � �����", out _));

            SetAnsver(temp, state, report6_1);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� �������� �� ����������������� ����������� � ��� �� ������ ������?", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� (�������� ������ ���������������� �������� ������)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ������������ � ������� ������", out Report report7_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));

            SetAnsver(temp, state, report7_4);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� ����������� ����������� �� ����������������� ������� � ���?", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������������� ������� (��� ���������� ������������ ��������)", out Report report8_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����������� ����������� ������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ������������������� �������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));

            SetAnsver(temp, state, report8_2);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ��������� � ��� �������������� �������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� ��������������������� ��� ������ ����������������� (������������ � �.�.)", out Report report9_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���������� ������� � ������� ��������� (������ 65 ���)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));
        }


        [TestMethod]
        public void Test4_by_Coronastop24()
        {
            Interviewer temp = CheckerTemplate3;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.dialog_patient;
            temp.SetAnswer(state);
            temp.GetResponse(state);
            string TriggerId0 = state.CurrentQuestionTriggerID.ToString(); ;
            Answer answer = new Answer();
            answer.TriggerId = TriggerId0;
            temp.SetAnswer(state, answer);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckAge(tempUnit)[5]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckSex(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckBlood(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null,
                "��� ������� � ������, �������� �������� �������� � ���� (��������� �� ����, ������, ������� � ��.)", out Report report4_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������� � ������� � �������������� ��������� COVID-19", out Report report4_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report4_3));

            SetAnsver(temp, state, report4_3);

            List<InterfaceUnit> response5 = temp.GetResponse(state);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report5_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� � ��������� ��� ����", out Report report5_2));

            SetAnsver(temp, state, report5_2);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ����� ����� ������", out Report report6_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report6_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�� �������", out Report report6_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ��������", out Report report6_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ ��� ��������� (�� �����)", out Report report6_5));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ����� ������", out Report report6_6));

            SetAnsver(temp, state, report6_4);


            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���������� �������� � ����� (��������)", out Report report_6_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ��������, ������� �� ��������� � ���� �� ��������� ��� ������", out Report report_6_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� � ������", out Report report_6_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� � �����", out Report report_6_3));

            SetAnsver(temp, state, report_6_4);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };


            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� �������� �� ����������������� ����������� � ��� �� ������ ������?", out Report report7_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� (�������� ������ ���������������� �������� ������)", out Report report7_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������", out Report report7_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ������������ � ������� ������", out Report report7_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report7_5));

            SetAnsver(temp, state, report7_5);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� ����������� ����������� �� ����������������� ������� � ���?", out Report report8_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������������� ������� (��� ���������� ������������ ��������)", out Report report8_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����������� ����������� ������", out Report report8_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ������������������� �������", out Report report8_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report8_5));

            SetAnsver(temp, state, report8_5);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ��������� � ��� �������������� �������", out Report report9_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� ��������������������� ��� ������ ����������������� (������������ � �.�.)", out Report report9_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���������� ������� � ������� ��������� (������ 65 ���)", out Report report9_3));
            //Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������", out Report report9_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report9_5));
        }

        [TestMethod]
        public void Test5_by_Coronastop24()
        {
            Interviewer temp = CheckerTemplate3;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.dialog_patient;
            temp.SetAnswer(state);
            List<InterfaceUnit> response0 = temp.GetResponse(state);
            string TriggerId0 = state.CurrentQuestionTriggerID.ToString(); ;
            Answer answer = new Answer();
            answer.TriggerId = TriggerId0;
            temp.SetAnswer(state, answer);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckAge(tempUnit)[5]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckSex(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckBlood(tempUnit)[4]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null,
                "��� ������� � ������, �������� �������� �������� � ���� (��������� �� ����, ������, ������� � ��.)", out Report report4_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������� � ������� � �������������� ��������� COVID-19", out Report report4_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report4_3));

            SetAnsver(temp, state, report4_3);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report5_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� � ��������� ��� ����", out Report report5_2));

            SetAnsver(temp, state, report5_2);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ����� ����� ������", out Report report6_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report6_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�� �������", out Report report6_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ��������", out Report report6_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ ��� ��������� (�� �����)", out Report report6_5));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ����� ������", out Report report6_6));

            SetAnsver(temp, state, report6_6);


            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };



            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������ ���������", out Report report_6_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������", out Report report_6_1));



            SetAnsver(temp, state, report_6_4);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };


            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ��������, ������� �� ��������� � ���� �� ��������� ��� ������", out Report report7_1));
            //Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� (�������� ������ ���������������� �������� ������)", out Report report7_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� � ������", out Report report7_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ (�����, ������ ����)", out Report report7_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������, ������������ ����", out Report report7_5));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����������� ��������", out Report report7_6));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report7_7));

            SetAnsver(temp, state, report7_5);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� �������� �� ����������������� ����������� � ��� �� ������ ������?", out Report report8_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� (�������� ������ ���������������� �������� ������)", out Report report8_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������", out Report report8_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ������������ � ������� ������", out Report report8_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report8_5));

            SetAnsver(temp, state, report8_4);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� ����������� ����������� �� ����������������� ������� � ���?", out Report report9_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������������� ������� (��� ���������� ������������ ��������)", out Report report9_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����������� ����������� ������", out Report report9_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ������������������� �������", out Report report9_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report9_5));

            SetAnsver(temp, state, report9_3);

            //tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            //Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ��������� � ��� �������������� �������", out Report report10_1));
            //Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� ��������������������� ��� ������ ����������������� (������������ � �.�.)", out Report report10_2));
            //Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���������� ������� � ������� ��������� (������ 65 ���)", out Report report10_3));
            ////Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������", out Report report10_4));
            //Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report10_5));
        }

        [TestMethod]
        public void Test6_by_Coronastop24()
        {
            Interviewer temp = CheckerTemplate3;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.dialog_patient;
            temp.SetAnswer(state);
            List<InterfaceUnit> response0 = temp.GetResponse(state);
            string TriggerId0 = state.CurrentQuestionTriggerID.ToString(); ;
            Answer answer = new Answer();
            answer.TriggerId = TriggerId0;
            temp.SetAnswer(state, answer);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckAge(tempUnit)[5]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckSex(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckBlood(tempUnit)[4]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null,
                "��� ������� � ������, �������� �������� �������� � ���� (��������� �� ����, ������, ������� � ��.)", out Report report4_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������� � ������� � �������������� ��������� COVID-19", out Report report4_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report4_3));

            SetAnsver(temp, state, report4_3);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report5_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� � ��������� ��� ����", out Report report5_2));

            SetAnsver(temp, state, report5_2);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ����� ����� ������", out Report report6_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report6_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�� �������", out Report report6_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ��������", out Report report6_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ ��� ��������� (�� �����)", out Report report6_5));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ����� ������", out Report report6_6));

            //answers.Add(SetAnsver(temp, state, report6_6));
            SetAnsver(temp, state, report6_6);


            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };



            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������ ���������", out Report report_6_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������", out Report report_6_1));

            SetAnsver(temp, state, report_6_4);



        }

        [TestMethod]
        public void Test7_by_Coronastop24()
        {
            Interviewer temp = CheckerTemplate3;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.dialog_patient;
            temp.SetAnswer(state);
            List<InterfaceUnit> response0 = temp.GetResponse(state);
            string TriggerId0 = state.CurrentQuestionTriggerID.ToString(); ;
            Answer answer = new Answer();
            answer.TriggerId = TriggerId0;
            temp.SetAnswer(state, answer);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckAge(tempUnit)[5]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckSex(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckBlood(tempUnit)[4]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null,
                "��� ������� � ������, �������� �������� �������� � ���� (��������� �� ����, ������, ������� � ��.)", out Report report4_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������� � ������� � �������������� ��������� COVID-19", out Report report4_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report4_3));

            SetAnsver(temp, state, report4_3);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� � ��������� ��� ����", out Report report5_2));

            SetAnsver(temp, state, report5_2);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ����� ����� ������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�� �������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ��������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ ��� ��������� (�� �����)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ����� ������", out Report report6_6));

            SetAnsver(temp, state, report6_6);


            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };



            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������ ���������", out Report report_6_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������", out _));



            SetAnsver(temp, state, report_6_4);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };


            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ��������, ������� �� ��������� � ���� �� ��������� ��� ������", out Report report7_1));
            //Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� (�������� ������ ���������������� �������� ������)", out Report report7_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� � ������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ (�����, ������ ����)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������, ������������ ����", out Report report7_5));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����������� ��������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));

            SetAnsver(temp, state, report7_5);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� �������� �� ����������������� ����������� � ��� �� ������ ������?", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� (�������� ������ ���������������� �������� ������)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ������������ � ������� ������", out Report report8_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));

            SetAnsver(temp, state, report8_4);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� ����������� ����������� �� ����������������� ������� � ���?", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������������� ������� (��� ���������� ������������ ��������)", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����������� ����������� ������", out Report report9_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ������������������� �������", out _));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out _));

            SetAnsver(temp, state, report9_3);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };


            SetAnsver(temp, state, CheckChronicles2(tempUnit)[3]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, CheckAdditionalFactors(tempUnit)[1]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            //tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            //Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ��������� � ��� �������������� �������", out Report report10_1));
            //Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� ��������������������� ��� ������ ����������������� (������������ � �.�.)", out Report report10_2));
            //Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���������� ������� � ������� ��������� (������ 65 ���)", out Report report10_3));
            ////Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������", out Report report10_4));
            //Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report10_5));
        }


        [TestMethod]
        public void Test8_by_Coronastop24()
        {
            Interviewer temp = CheckerTemplate3;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.dialog_patient;
            temp.SetAnswer(state);
            List<InterfaceUnit> response0 = temp.GetResponse(state);
            string TriggerId0 = state.CurrentQuestionTriggerID.ToString();
            Answer answer = new Answer();
            answer.TriggerId = TriggerId0;
            temp.SetAnswer(state, answer);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckAge(tempUnit)[5]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckSex(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckBlood(tempUnit)[4]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
            Assert.IsTrue(ReqursiveFinder(tempUnit, null,
                "��� ������� � ������, �������� �������� �������� � ���� (��������� �� ����, ������, ������� � ��.)", out Report report4_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������� � ������� � �������������� ��������� COVID-19", out Report report4_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report4_3));

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), report4_3);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report5_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� � ��������� ��� ����", out Report report5_2));

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), report5_2);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ����� ����� ������", out Report report6_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report6_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�� �������", out Report report6_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ��������", out Report report6_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ ��� ��������� (�� �����)", out Report report6_5));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ����� ������", out Report report6_6));

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), report6_6);


            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };



            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������������ ���������", out Report report_6_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������", out Report report_6_1));



            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), report_6_4);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };


            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ��������, ������� �� ��������� � ���� �� ��������� ��� ������", out Report report7_1));
            //Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� (�������� ������ ���������������� �������� ������)", out Report report7_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "���� � ������", out Report report7_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������ (�����, ������ ����)", out Report report7_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������, ������������ ����", out Report report7_5));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����������� ��������", out Report report7_6));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report7_7));

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), report7_5);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� �������� �� ����������������� ����������� � ��� �� ������ ������?", out Report report8_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� (�������� ������ ���������������� �������� ������)", out Report report8_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������", out Report report8_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�������� ������������ � ������� ������", out Report report8_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report8_5));

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), report8_4);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����� ����������� ����������� �� ����������������� ������� � ���?", out Report report9_1));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "��������������� ������� (��� ���������� ������������ ��������)", out Report report9_2));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "����������� ����������� ������", out Report report9_3));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "������� ������������������� �������", out Report report9_4));
            Assert.IsTrue(ReqursiveFinder(tempUnit, null, "�����", out Report report9_5));

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), report9_3);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };


            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckChronicles2(tempUnit)[3]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };


            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckAdditionalFactors(tempUnit)[1]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };
        }


        [TestMethod]
        public void Test9_by_Coronastop24()
        {
            Interviewer temp = CheckerTemplate3;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.dialog_patient;
            temp.SetAnswer(state);
            List<InterfaceUnit> response0 = temp.GetResponse(state);
            string TriggerId0 = state.CurrentQuestionTriggerID.ToString();
            Answer answer = new Answer();
            answer.TriggerId = TriggerId0;
            temp.SetAnswer(state, answer);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckAge(tempUnit)[2]);//61-74

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckSex(tempUnit)[1]);//�������

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckBlood(tempUnit)[0]);//I

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckEvents(tempUnit)[1]);//�������� � �����

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckPrevention(tempUnit)[0]);//�����

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckWork(tempUnit)[5]);//������� �����

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckMoving(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            var symptoms1 = CheckSimpLast2Week(tempUnit);

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), symptoms1[0], symptoms1[3], symptoms1[5]);//�������� � ����, ��������� ����������� ���� �� 38�C, ������, ����� ��� � ��������� ����������� �������

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            var symptoms2 = CheckCurrentSimptoms(tempUnit);

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), symptoms2[4], symptoms2[5], symptoms2[6], symptoms2[3]);//������, ��������, �����������, ������������ � �� ������

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckChronicles(tempUnit)[4]);//�����

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckAdditionalFactors(tempUnit)[4]);//������

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckResults(tempUnit)[0]);

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

        }

        [TestMethod]
        public void DoubledAnswersTest1_by_Coronastop24()
        {
            Interviewer temp = CheckerTemplate3;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.dialog_patient;
            temp.SetAnswer(state);
            List<InterfaceUnit> response0 = temp.GetResponse(state);
            string TriggerId0 = state.CurrentQuestionTriggerID.ToString();
            Answer answer = new Answer();
            answer.TriggerId = TriggerId0;
            temp.SetAnswer(state, answer);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckAge(tempUnit)[2]);//61-74

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            var CheckSexResult = CheckSex(tempUnit);
            try
            {
                SetAnsver(temp, state, "11", CheckSexResult[1]);//������� � �������
            }
            catch (IASK.InterviewerEngine.Exceptions.UncorrectAnsverException ex)
            {
                Assert.IsTrue(ex.Message.Equals("�������������� ����������� ID �������� ����������! �������� ��������� ������������������ �������."));
            }
            
        }

        [TestMethod]
        public void DoubledAnswersTest2_by_Coronastop24()
        {
            Interviewer temp = CheckerTemplate3;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.dialog_patient;
            temp.SetAnswer(state);
            List<InterfaceUnit> response0 = temp.GetResponse(state);
            string TriggerId0 = state.CurrentQuestionTriggerID.ToString();
            Answer answer = new Answer();
            answer.TriggerId = TriggerId0;
            temp.SetAnswer(state, answer);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckAge(tempUnit)[2]);//61-74

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            var CheckSexResult = CheckSex(tempUnit);
            try
            {
                SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckSexResult[1], CheckSexResult[0]);//������� � �������
            }
            catch (IASK.InterviewerEngine.Exceptions.UncorrectAnsverException ex)
            {
                Assert.IsTrue(ex.Message.Equals("Multiple answers in OR group!"));
            }

        }

        [TestMethod]
        public void DoubledAnswersTest3_by_Coronastop24()
        {
            Interviewer temp = CheckerTemplate3;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.dialog_patient;
            temp.SetAnswer(state);
            List<InterfaceUnit> response0 = temp.GetResponse(state);
            string TriggerId0 = state.CurrentQuestionTriggerID.ToString();
            Answer answer = new Answer();
            answer.TriggerId = TriggerId0;
            temp.SetAnswer(state, answer);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            SetAnsver(temp, state, state.CurrentQuestionTriggerID.ToString(), CheckAge(tempUnit)[2]);//61-74

            tempUnit = new InterfaceUnit() { Units = temp.GetResponse(state) };

            var CheckSexResult = CheckSex(tempUnit);
            try
            {
                SetAnsver(temp, state, "���", CheckSexResult[1]);//������� � �������
            }
            catch (InvalidCastException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Cannot parse Trigger id: "));
            }
        }


        [TestMethod]
        public void DoctorTest1()
        {
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.doctor;
            DoctorQuestioning.SetAnswer(state);

            List<InterfaceUnit> unit1 = DoctorQuestioning.GetResponse(state);

            Report ans1 = new Report() { idb = "3030081", id = "64200086", value = "qqq" };
            Report ans2 = new Report() { idb = "3040081", id = "64210086", value = "qqq" };
            Report ans3 = new Report() { idb = "3050081", id = "64220086", value = "1111" };
            Report ans4 = new Report() { idb = "1510081", id = "64100086", value = "111" };
            Report ans5 = new Report() { idb = "1560081", id = "64110086", value = "1111" };
            Report ans6 = new Report() { idb = "3020081", id = "64130086", value = "1111" };

            SetAnsver(DoctorQuestioning, state, state.CurrentQuestionTriggerID.ToString(),ans1,ans2,ans3,ans4,ans5,ans6);

            unit1 = DoctorQuestioning.GetResponse(state);

            ans1 = new Report() { idb = "5780912", id = "67620086", value = "qqq" };
            ans2 = new Report() { idb = "5790912", id = "67630086", value = "qqq" };
            ans3 = new Report() { idb = "3480121", id = "67550086", value = "qqq" };
            ans4 = new Report() { idb = "20165", id = "67540086", value = "qqq" };

            SetAnsver(DoctorQuestioning, state, state.CurrentQuestionTriggerID.ToString(), ans1, ans2, ans3, ans4);

            unit1 = DoctorQuestioning.GetResponse(state);

            ans1 = new Report() { idb = "1210910", id = "68260086", value = "qqq" };
            ans2 = new Report() { idb = "5780912", id = "68270086", value = "qqq" };
            ans3 = new Report() { idb = "5790912", id = "68280086", value = "qqq" };
            ans4 = new Report() { idb = "5800912", id = "68290086", value = "qqq" };


            SetAnsver(DoctorQuestioning, state, state.CurrentQuestionTriggerID.ToString(), ans1, ans2, ans3, ans4);

            unit1 = DoctorQuestioning.GetResponse(state);

            ans1 = new Report() { idb = "1210910", id = "68140086", value = "qqq" };
            ans2 = new Report() { idb = "2290123", id = "69970086", value = "qqq" };
            ans3 = new Report() { idb = "5780912", id = "68150086", value = "qqq" };
            ans4 = new Report() { idb = "5790912", id = "68160086", value = "qqq" };

            SetAnsver(DoctorQuestioning, state, state.CurrentQuestionTriggerID.ToString(), ans1, ans2, ans3, ans4);

            unit1 = DoctorQuestioning.GetResponse(state);

            ans1 = new Report() { idb = "1210910", id = "68380086", value = "qqq" };
            ans2 = new Report() { idb = "5780912", id = "68390086", value = "qqq" };
            ans3 = new Report() { idb = "2720123", id = "68700086", value = "1111" };
            ans4 = new Report() { idb = "5800912", id = "68410086", value = "111" };
            ans5 = new Report() { idb = "5790912", id = "68400086", value = "1111" };

            SetAnsver(DoctorQuestioning, state, state.CurrentQuestionTriggerID.ToString(), ans1, ans2, ans3, ans4, ans5);

            unit1 = DoctorQuestioning.GetResponse(state);
            Assert.IsTrue(ReqursiveFinder(unit1[0], null, "������� ������������", out Report r));
            //SetAnsver(DoctorQuestioning, state, state.CurrentQuestionTriggerID.ToString());

            //unit1 = DoctorQuestioning.GetResponse(state);

            //SetAnsver(DoctorQuestioning, state, state.CurrentQuestionTriggerID.ToString());

            //unit1 = DoctorQuestioning.GetResponse(state);
        }

        [TestMethod]
        public void DoctorTest2()
        {
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.doctor;
            DoctorQuestioning.SetAnswer(state);

            List<InterfaceUnit> unit1 = DoctorQuestioning.GetResponse(state);

            Report ans1 = new Report() { idb = "3030081", id = "64200086", value = "qqq" };
            Report ans2 = new Report() { idb = "3040081", id = "64210086", value = "qqq" };
            Report ans3 = new Report() { idb = "3050081", id = "64220086", value = "1111" };
            Report ans4 = new Report() { idb = "1510081", id = "64100086", value = "111" };
            Report ans5 = new Report() { idb = "1560081", id = "64110086", value = "1111" };
            Report ans6 = new Report() { idb = "3020081", id = "64130086", value = "1111" };

            SetAnsver(DoctorQuestioning, state, state.CurrentQuestionTriggerID.ToString(),ans1,ans2,ans3,ans4,ans5,ans6);

            unit1 = DoctorQuestioning.GetResponse(state);

            ans1 = new Report() { idb = "5780912", id = "67620086", value = "qqq" };
            ans2 = new Report() { idb = "5790912", id = "67630086", value = "qqq" };
            ans3 = new Report() { idb = "3480121", id = "67550086", value = "qqq" };
            ans4 = new Report() { idb = "20165", id = "67540086", value = "qqq" };

            SetAnsver(DoctorQuestioning, state, state.CurrentQuestionTriggerID.ToString(), ans1, ans2, ans3, ans4);

            unit1 = DoctorQuestioning.GetResponse(state);

            ans1 = new Report() { idb = "1210910", id = "68260086", value = "qqq" };
            ans2 = new Report() { idb = "5780912", id = "68270086", value = "qqq" };
            ans3 = new Report() { idb = "5790912", id = "68280086", value = "qqq" };
            ans4 = new Report() { idb = "5800912", id = "68290086", value = "qqq" };


            SetAnsver(DoctorQuestioning, state, state.CurrentQuestionTriggerID.ToString(), ans1, ans2, ans3, ans4);

            unit1 = DoctorQuestioning.GetResponse(state);

            ans1 = new Report() { idb = "1210910", id = "68140086", value = "qqq" };
            ans2 = new Report() { idb = "2290123", id = "69970086", value = "qqq" };
            ans3 = new Report() { idb = "5780912", id = "68150086", value = "qqq" };
            ans4 = new Report() { idb = "5790912", id = "68160086", value = "qqq" };

            SetAnsver(DoctorQuestioning, state, state.CurrentQuestionTriggerID.ToString(), ans1, ans2, ans3, ans4);

            unit1 = DoctorQuestioning.GetResponse(state);

            //ans1 = new Report() { idb = "1210910", id = "68380086", value = "qqq" };
            //ans2 = new Report() { idb = "5780912", id = "68390086", value = "qqq" };
            //ans3 = new Report() { idb = "2720123", id = "68700086", value = "1111" };
            //ans4 = new Report() { idb = "5800912", id = "68410086", value = "111" };
            //ans5 = new Report() { idb = "5790912", id = "68400086", value = "1111" };

            //SetAnsver(DoctorQuestioning, state, state.CurrentQuestionTriggerID.ToString(), ans1, ans2, ans3, ans4, ans5);
            
            try
            {
                SetAnsver(DoctorQuestioning, state, state.CurrentQuestionTriggerID.ToString());
                unit1 = DoctorQuestioning.GetResponse(state);
            }
            catch (IASK.InterviewerEngine.Exceptions.UncorrectAnsverException)
            {
               // Assert.IsTrue(true);
            }

            Assert.IsFalse(ReqursiveFinder(unit1[0], null, "������� ������������", out _));
            Assert.IsTrue(ReqursiveFinder(unit1[0], null, "������� ���� �������������", out _));
        }

        [TestMethod]
        public void ThreadSafetyTest1()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 10000; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    Test9_by_Coronastop24();
                }));
            }
            Task.WaitAll(tasks.ToArray());
            foreach (Task task in tasks)
            {
                Assert.IsTrue(task.Status == TaskStatus.RanToCompletion);
            }
        }


        private static void Wrapper1()
        {
            DoctorQuestioning.GetAllFields();
        }
        [TestMethod]
        public void GetAllFieldsTest1()
        {
           Assert.ThrowsException<MethodAccessException>(Wrapper1);
        }

        private static void Wrapper2()
        {
            CheckerTemplate3.GetAllFields();
        }
        [TestMethod]
        public void GetAllFieldsTest2()
        {
            Assert.ThrowsException<MethodAccessException>(Wrapper1);
        }

        [TestMethod]
        public void GetAllFieldsTest3()
        {
            List<InterfaceUnit> iu = Reception.GetAllFields();
            Assert.IsTrue(iu.Count > 1);
        }


        [TestMethod]
        public void OnePageBrowsingMode()
        {
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.doctor;
            OnePage.SetAnswer(state);
            List<InterfaceUnit> temp = OnePage.GetResponse(state);
            CheckPatientsPage(temp[1]);
        }


        [TestMethod]
        public void EMC_Test1()
        {
            Interviewer temp = EMC1;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.doctor;
            temp.SetAnswer(state);
            List<InterfaceUnit> response0 = temp.GetResponse(state);
            Assert.IsTrue(ReqursiveFinder(response0[3], null, "������� �����", out _));
            Assert.IsTrue(response0[3].Content != null && response0[3].Content.Count == 2);
            Assert.IsTrue(response0[3].Content[1].Type == Content.ContentType.id && response0[3].Content[1].Value == "3");
            Assert.IsTrue(response0[3].Content[0].Type == Content.ContentType.lib && response0[3].Content[0].Value == "81");
            Assert.IsTrue(response0[7].Label== "��������������� �������" && response0[7].Condition==InterfaceUnit.BrowseCondition.COLLAPSED_PAGE);


        }


        [TestMethod]
        public void SwitchToSensor()
        {
            Interviewer temp = EMC1;
            InterviewerState state = new InterviewerState();
            state.DialogType = DialogType.doctor;
            temp.SetAnswer(state);
            List<InterfaceUnit> response0 = temp.GetResponse(state);
            Assert.IsTrue(ReqursiveFinder(response0[3], null, "������� �����", out _));
            Assert.IsTrue(response0[3].Content != null && response0[3].Content.Count == 2);
            Assert.IsTrue(response0[3].Content[1].Type == Content.ContentType.id && response0[3].Content[1].Value == "3");
            Assert.IsTrue(response0[3].Content[0].Type == Content.ContentType.lib && response0[3].Content[0].Value == "81");
        }

    }
}
