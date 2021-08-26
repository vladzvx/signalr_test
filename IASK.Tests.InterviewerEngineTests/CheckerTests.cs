using IASK.Common;
using IASK.Common.Services;
using IASK.InterviewerEngine;
using IASK.InterviewerEngine.Models.Input;
using IASK.InterviewerEngine.Models.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UMKBRequests;

namespace IASK.InterviewerEngineTests
{

    [TestClass]
    public class CheckerTests
    {
        static InterviewerWrapper checker;
        static InterviewerWrapper DoctorChecker;
        static Interviewer.Factory interviewerFactory;
        static ProbabilityCalculator.Factory probFactory;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {

            interviewerFactory = new Interviewer.Factory(new KeyProvider());
            probFactory = new ProbabilityCalculator.Factory(new KeyProvider());
            ulong CheckerId = IdParser.GetNewBigId(86, 506);
            checker = new InterviewerWrapper(interviewerFactory, probFactory);
            checker.Init(CheckerId);

            ulong DoctorCheckerId = IdParser.GetNewBigId(86, 6408);
            DoctorChecker = new InterviewerWrapper(interviewerFactory, probFactory);
            DoctorChecker.Init(DoctorCheckerId);
        }

        public static Answer CreateAnsver(string TriggerId, params Report[] report)
        {
            List<InputStruct> buffer = new List<InputStruct>();
            foreach (Report rep in report)
            {
                buffer.Add(new InputStruct()
                {
                    Id = rep.id,
                    Idb = rep.idb,
                    Value = rep.value,
                    //Labels = new string[1] { rep.label }
                });
            }
            Answer ans = new Answer()
            {
                TriggerId = TriggerId,

                CheckerResponse = buffer.ToArray()
            };
            return ans;
        }

        [TestMethod]
        public void Test1_by_Coronastop24()
        {
            string dialogType = "dialog_patient";
            List<Answer> ansvers = new List<Answer>();
            var res1 = checker.SetAnsvers(ansvers, dialogType);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(new Answer() { TriggerId = res1.Item2 });
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckAge(tempUnit)[6]));//"24 - 35",
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckSex(tempUnit)[0]));//male
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckBlood(tempUnit)[0]));//1
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckEvents(tempUnit)[1]));//"Контактировал с больным с подтвержденным диагнозом COVID-19"
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckPrevention(tempUnit)[0]));//далее
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckWork(tempUnit)[5]));//office
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckMoving(tempUnit)[0]));//Общественный транспорт
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            var symptoms1 = InterviewerTests.CheckSimpLast2Week(tempUnit);

            //обоняние и вкус, Повышение температуры тела до 38°C, Кашель, сухой или с небольшим количеством мокроты
            //ansvers.Add(CreateAnsver(res1.Item2, symptoms1[0], symptoms1[3], symptoms1[5]));
            ansvers.Add(new Answer() { TriggerId = res1.Item2 });
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            var symptoms2 = InterviewerTests.CheckCurrentSimptoms(tempUnit);
            //одыщка, слабость, температура, заложенность в гр клетке
            //ansvers.Add(CreateAnsver(res1.Item2, symptoms2[4], symptoms2[5], symptoms2[6], symptoms2[3]));
            ansvers.Add(new Answer() { TriggerId = res1.Item2 });
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckChronicles(tempUnit)[4]));//next
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckAdditionalFactors(tempUnit)[4]));//ничего
            res1 = checker.SetAnsvers(ansvers, dialogType);
             
            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            //Assert.IsTrue(QuestioningTests.ReqursiveFinder(tempUnit, "Вероятность наличия:", out var iuNS));
            Assert.IsTrue(InterviewerTests.ReqursiveFinder(tempUnit, "Риск развития:", out var iuRF));
            Assert.IsTrue(iuRF.Content.FindIndex(item => item.Value.Equals("коронавирусная инфекция COVID-19") && item.Value2.Equals("46")) >= 0);
            //Assert.IsTrue(iuRF.Content.FindIndex(item => item.value.Equals("острый респираторный дистресс-синдром") && item.value2.Equals("70")) >= 0);
            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckResults(tempUnit)[0]));//ничего
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            //Assert.IsTrue(QuestioningTests.ReqursiveFinder(tempUnit, null, "Рекомендуется вызвать скорую помощь", out var rr));
            Assert.IsTrue(InterviewerTests.ReqursiveFinder(tempUnit, null, "УСИЛЬТЕ МЕРЫ ПРОФИЛАКТИКИ", out var rr));
        }

        [TestMethod]
        public void Test2_by_Coronastop24()
        {
            string dialogType = "dialog_patient";
            List<Answer> ansvers = new List<Answer>();
            var res1 = checker.SetAnsvers(ansvers, dialogType);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(new Answer() { TriggerId = res1.Item2 });
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckAge(tempUnit)[2]));//"61 - 74",
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckSex(tempUnit)[1]));//female
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckBlood(tempUnit)[0]));//1
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckEvents(tempUnit)[1]));//"Контактировал с больным с подтвержденным диагнозом COVID-19"
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };


            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckPrevention(tempUnit)[0]));//далее
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };


            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckWork(tempUnit)[5]));//office
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };



            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckMoving(tempUnit)[0]));//Общественный транспорт
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            var symptoms1 = InterviewerTests.CheckSimpLast2Week(tempUnit);

            //обоняние и вкус, Повышение температуры тела до 38°C, Кашель, сухой или с небольшим количеством мокроты
            ansvers.Add(CreateAnsver(res1.Item2, symptoms1[0], symptoms1[3], symptoms1[5]));
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            var symptoms2 = InterviewerTests.CheckCurrentSimptoms(tempUnit);
            //одыщка, слабость, температура, заложенность в гр клетке
            ansvers.Add(CreateAnsver(res1.Item2, symptoms2[4], symptoms2[5], symptoms2[6], symptoms2[3]));
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckChronicles(tempUnit)[4]));//next
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckAdditionalFactors(tempUnit)[4]));//ничего
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            Assert.IsTrue(InterviewerTests.ReqursiveFinder(tempUnit, "Вероятность наличия:", out var iuNS));
            //Assert.IsTrue(QuestioningTests.ReqursiveFinder(tempUnit, "Риск развития:", out var iuRF));
            Assert.IsTrue(iuNS.Content.FindIndex(item => item.Value.Equals("коронавирусная инфекция COVID-19") && item.Value2.Equals("92")) >= 0);
            Assert.IsTrue(iuNS.Content.FindIndex(item => item.Value.Equals("острый респираторный дистресс-синдром") && item.Value2.Equals("70")) >= 0);
            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckResults(tempUnit)[0]));//ничего
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            Assert.IsTrue(InterviewerTests.ReqursiveFinder(tempUnit, null, "Рекомендуется вызвать скорую помощь", out var rr));

        }

        [TestMethod]
        public void Test3_by_Coronastop24()
        {
            string dialogType = "dialog_patient";
            List<Answer> ansvers = new List<Answer>();
            var res1 = checker.SetAnsvers(ansvers, dialogType);

            InterfaceUnit tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(new Answer() { TriggerId = res1.Item2 });
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckAge(tempUnit)[6]));//"24 - 35",
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckSex(tempUnit)[0]));//male
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckBlood(tempUnit)[0]));//1
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckEvents(tempUnit)[1]));//"Контактировал с больным с подтвержденным диагнозом COVID-19"
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };


            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckPrevention(tempUnit)[0]));//далее
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };


            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckWork(tempUnit)[5]));//office
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };



            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckMoving(tempUnit)[0]));//Общественный транспорт
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            var symptoms1 = InterviewerTests.CheckSimpLast2Week(tempUnit);

            //обоняние и вкус, Повышение температуры тела до 38°C, Кашель, сухой или с небольшим количеством мокроты
            //ansvers.Add(CreateAnsver(res1.Item2, symptoms1[0], symptoms1[3], symptoms1[5]));
            ansvers.Add(new Answer() { TriggerId = res1.Item2 });
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            var symptoms2 = InterviewerTests.CheckCurrentSimptoms(tempUnit);
            //одыщка, слабость, температура, заложенность в гр клетке
            ansvers.Add(CreateAnsver(res1.Item2, symptoms2[4], symptoms2[5], symptoms2[6], symptoms2[3]));
            //ansvers.Add(new Answer() { TriggerId = res1.Item2 });
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckChronicles(tempUnit)[4]));//next
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckAdditionalFactors(tempUnit)[4]));//ничего
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            Assert.IsTrue(InterviewerTests.ReqursiveFinder(tempUnit, "Вероятность наличия:", out var iuNS));
            Assert.IsTrue(InterviewerTests.ReqursiveFinder(tempUnit, "Риск развития:", out var iuRF));
            Assert.IsTrue(iuRF.Content.FindIndex(item => item.Value.Equals("коронавирусная инфекция COVID-19") && item.Value2.Equals("46")) >= 0);
            Assert.IsTrue(iuNS.Content.FindIndex(item => item.Value.Equals("острый респираторный дистресс-синдром") && item.Value2.Equals("65")) >= 0);
            ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckResults(tempUnit)[0]));//ничего
            res1 = checker.SetAnsvers(ansvers, dialogType);

            tempUnit = new InterfaceUnit() { Units = res1.Item1 };

            //Assert.IsTrue(QuestioningTests.ReqursiveFinder(tempUnit, null, "Рекомендуется вызвать скорую помощь", out var rr));
            Assert.IsTrue(InterviewerTests.ReqursiveFinder(tempUnit, null, "НЕОБХОДИМО ВЫЗВАТЬ ВРАЧА НА ДОМ", out var rr));

        }

        public void Test3_by_CoronastopForThreadings(object cancellationToken)
        {
            CancellationToken ct = (CancellationToken)cancellationToken;
            while (!ct.IsCancellationRequested)
            {
                string dialogType = "dialog_patient";
                List<Answer> ansvers = new List<Answer>();
                var res1 = checker.SetAnsvers(ansvers, dialogType);

                if (ct.IsCancellationRequested) return;
                InterfaceUnit tempUnit = new InterfaceUnit() { Units = res1.Item1 };

                ansvers.Add(new Answer() { TriggerId = res1.Item2 });
                res1 = checker.SetAnsvers(ansvers, dialogType);
                if (ct.IsCancellationRequested) return;
                tempUnit = new InterfaceUnit() { Units = res1.Item1 };

                ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckAge(tempUnit)[6]));//"24 - 35",
                res1 = checker.SetAnsvers(ansvers, dialogType);
                if (ct.IsCancellationRequested) return;
                tempUnit = new InterfaceUnit() { Units = res1.Item1 };

                ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckSex(tempUnit)[0]));//male
                res1 = checker.SetAnsvers(ansvers, dialogType);

                tempUnit = new InterfaceUnit() { Units = res1.Item1 };
                if (ct.IsCancellationRequested) return;
                ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckBlood(tempUnit)[0]));//1
                res1 = checker.SetAnsvers(ansvers, dialogType);

                tempUnit = new InterfaceUnit() { Units = res1.Item1 };
                if (ct.IsCancellationRequested) return;
                ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckEvents(tempUnit)[1]));//"Контактировал с больным с подтвержденным диагнозом COVID-19"
                res1 = checker.SetAnsvers(ansvers, dialogType);

                tempUnit = new InterfaceUnit() { Units = res1.Item1 };
                if (ct.IsCancellationRequested) return;

                ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckPrevention(tempUnit)[0]));//далее
                res1 = checker.SetAnsvers(ansvers, dialogType);
                if (ct.IsCancellationRequested) return;
                tempUnit = new InterfaceUnit() { Units = res1.Item1 };

                if (ct.IsCancellationRequested) return;
                ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckWork(tempUnit)[5]));//office
                res1 = checker.SetAnsvers(ansvers, dialogType);
                if (ct.IsCancellationRequested) return;
                tempUnit = new InterfaceUnit() { Units = res1.Item1 };


                if (ct.IsCancellationRequested) return;
                ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckMoving(tempUnit)[0]));//Общественный транспорт
                res1 = checker.SetAnsvers(ansvers, dialogType);
                if (ct.IsCancellationRequested) return;
                tempUnit = new InterfaceUnit() { Units = res1.Item1 };
                if (ct.IsCancellationRequested) return;
                var symptoms1 = InterviewerTests.CheckSimpLast2Week(tempUnit);

                //обоняние и вкус, Повышение температуры тела до 38°C, Кашель, сухой или с небольшим количеством мокроты
                //ansvers.Add(CreateAnsver(res1.Item2, symptoms1[0], symptoms1[3], symptoms1[5]));
                ansvers.Add(new Answer() { TriggerId = res1.Item2 });
                if (ct.IsCancellationRequested) return;
                res1 = checker.SetAnsvers(ansvers, dialogType);
                if (ct.IsCancellationRequested) return;
                tempUnit = new InterfaceUnit() { Units = res1.Item1 };
                if (ct.IsCancellationRequested) return;
                var symptoms2 = InterviewerTests.CheckCurrentSimptoms(tempUnit);
                //одыщка, слабость, температура, заложенность в гр клетке
                ansvers.Add(CreateAnsver(res1.Item2, symptoms2[4], symptoms2[5], symptoms2[6], symptoms2[3]));
                if (ct.IsCancellationRequested) return;
                //ansvers.Add(new Answer() { TriggerId = res1.Item2 });
                res1 = checker.SetAnsvers(ansvers, dialogType);
                if (ct.IsCancellationRequested) return;
                tempUnit = new InterfaceUnit() { Units = res1.Item1 };

                ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckChronicles(tempUnit)[4]));//next
                res1 = checker.SetAnsvers(ansvers, dialogType);
                if (ct.IsCancellationRequested) return;
                tempUnit = new InterfaceUnit() { Units = res1.Item1 };

                ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckAdditionalFactors(tempUnit)[4]));//ничего
                res1 = checker.SetAnsvers(ansvers, dialogType);
                if (ct.IsCancellationRequested) return;
                tempUnit = new InterfaceUnit() { Units = res1.Item1 };
                if (ct.IsCancellationRequested) return;
                Assert.IsTrue(InterviewerTests.ReqursiveFinder(tempUnit, "Вероятность наличия:", out var iuNS));
                Assert.IsTrue(InterviewerTests.ReqursiveFinder(tempUnit, "Риск развития:", out var iuRF));
                if (ct.IsCancellationRequested) return;
                Assert.IsTrue(iuRF.Content.FindIndex(item => item.Value.Equals("коронавирусная инфекция COVID-19") && item.Value2.Equals("46")) >= 0);
                Assert.IsTrue(iuNS.Content.FindIndex(item => item.Value.Equals("острый респираторный дистресс-синдром") && item.Value2.Equals("65")) >= 0);
                if (ct.IsCancellationRequested) return;
                ansvers.Add(CreateAnsver(res1.Item2, InterviewerTests.CheckResults(tempUnit)[0]));//ничего
                res1 = checker.SetAnsvers(ansvers, dialogType);
                if (ct.IsCancellationRequested) return;
                tempUnit = new InterfaceUnit() { Units = res1.Item1 };
                if (ct.IsCancellationRequested) return;
                //Assert.IsTrue(QuestioningTests.ReqursiveFinder(tempUnit, null, "Рекомендуется вызвать скорую помощь", out var rr));
                Assert.IsTrue(InterviewerTests.ReqursiveFinder(tempUnit, null, "НЕОБХОДИМО ВЫЗВАТЬ ВРАЧА НА ДОМ", out var rr));
                if (ct.IsCancellationRequested) return;
            }


        }


        [TestMethod]
        public void ThreadSafetyTest1()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 50; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    Test3_by_Coronastop24();
                }));
            }
            Task.WaitAll(tasks.ToArray());
            foreach (Task task in tasks)
            {
                Assert.IsTrue(task.Status == TaskStatus.RanToCompletion);
            }
        }

        [TestMethod]
        public void ThreadSafetyTest2()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            ConcurrentBag<Thread> Threads = new ConcurrentBag<Thread>();
            Parallel.For(0, 100, (i) =>
            {

                if (!cts.IsCancellationRequested)
                {
                    Thread thr = new Thread(new ParameterizedThreadStart(Test3_by_CoronastopForThreadings));
                    thr.Start(cts.Token);
                    Threads.Add(thr);
                }

            });
            Thread.Sleep(10000);
            cts.Cancel();
        }

        [TestMethod]
        public void DoctorTest1()
        {
            string dialogType = "doctor";
            List<Answer> ansvers = new List<Answer>();
            var res = DoctorChecker.SetAnsvers(ansvers, dialogType);

            Report ans1 = new Report() { idb = "3030081", id = "64200086", value = "qqq" };
            Report ans2 = new Report() { idb = "3040081", id = "64210086", value = "qqq" };
            Report ans3 = new Report() { idb = "3050081", id = "64220086", value = "1111" };
            Report ans4 = new Report() { idb = "1510081", id = "64100086", value = "111" };
            Report ans5 = new Report() { idb = "1560081", id = "64110086", value = "1111" };
            Report ans6 = new Report() { idb = "3020081", id = "64130086", value = "1111" };

            ansvers.Add(InterviewerTests.CreateAnswer(res.Item2, ans1, ans2, ans3, ans4, ans5, ans6));
            res = DoctorChecker.SetAnsvers(ansvers, dialogType);

            ans1 = new Report() { idb = "5780912", id = "67620086", value = "qqq" };
            ans2 = new Report() { idb = "5790912", id = "67630086", value = "qqq" };
            ans3 = new Report() { idb = "3480121", id = "67550086", value = "qqq" };
            ans4 = new Report() { idb = "20165", id = "67540086", value = "123" };

            ansvers.Add(InterviewerTests.CreateAnswer(res.Item2, ans1, ans2, ans3, ans4));
            res = DoctorChecker.SetAnsvers(ansvers, dialogType);

            ans1 = new Report() { idb = "1210910", id = "68260086", value = "qqq" };
            ans2 = new Report() { idb = "5780912", id = "68270086", value = "qqq" };
            ans3 = new Report() { idb = "5790912", id = "68280086", value = "qqq" };
            ans4 = new Report() { idb = "5800912", id = "68290086", value = "qqq" };

            ansvers.Add(InterviewerTests.CreateAnswer(res.Item2, ans1, ans2, ans3, ans4));
            res = DoctorChecker.SetAnsvers(ansvers, dialogType);

            ans1 = new Report() { idb = "1210910", id = "68140086", value = "qqq" };
            ans2 = new Report() { idb = "2290123", id = "69970086", value = "1123" };
            ans3 = new Report() { idb = "5780912", id = "68150086", value = "qqq" };
            ans4 = new Report() { idb = "5790912", id = "68160086", value = "qqq" };

            ansvers.Add(InterviewerTests.CreateAnswer(res.Item2, ans1, ans2, ans3, ans4));
            res = DoctorChecker.SetAnsvers(ansvers, dialogType);

            //ans1 = new Report() { idb = "1210910", id = "68380086", value = "qqq" };
            //ans2 = new Report() { idb = "5780912", id = "68390086", value = "qqq" };
            //ans3 = new Report() { idb = "2720123", id = "68700086", value = "1111" };
            //ans4 = new Report() { idb = "5800912", id = "68410086", value = "111" };
            //ans5 = new Report() { idb = "5790912", id = "68400086", value = "1111" };
            Answer answer = InterviewerTests.CreateAnswer(res.Item2);//, ans1, ans2, ans3, ans4, ans5);
            ansvers.Add(answer);//;
            res = DoctorChecker.SetAnsvers(ansvers, dialogType);
            Assert.IsTrue(res.Item1[0].Label.Contains("Укажите Вашу специальность"));
        }

        [TestMethod]
        public void DoctorTest2()
        {
            string dialogType = "doctor";
            List<Answer> ansvers = new List<Answer>();
            var res = DoctorChecker.SetAnsvers(ansvers, dialogType);

            Report ans1 = new Report() { idb = "3030081", id = "64200086", value = "qqq" };
            Report ans2 = new Report() { idb = "3040081", id = "64210086", value = "qqq" };
            Report ans3 = new Report() { idb = "3050081", id = "64220086", value = "1111" };
            Report ans4 = new Report() { idb = "1510081", id = "64100086", value = "111" };
            Report ans5 = new Report() { idb = "1560081", id = "64110086", value = "1111" };
            Report ans6 = new Report() { idb = "3020081", id = "64130086", value = "1111" };

            ansvers.Add(InterviewerTests.CreateAnswer(res.Item2, ans1, ans2, ans3, ans4, ans5, ans6));
            res = DoctorChecker.SetAnsvers(ansvers, dialogType);

            ans1 = new Report() { idb = "5780912", id = "67620086", value = "qqq" };
            ans2 = new Report() { idb = "5790912", id = "67630086", value = "qqq" };
            ans3 = new Report() { idb = "3480121", id = "67550086", value = "qqq" };
            ans4 = new Report() { idb = "20165", id = "67540086", value = "123" };

            ansvers.Add(InterviewerTests.CreateAnswer(res.Item2, ans1, ans2, ans3, ans4));
            res = DoctorChecker.SetAnsvers(ansvers, dialogType);

            ans1 = new Report() { idb = "1210910", id = "68260086", value = "qqq" };
            ans2 = new Report() { idb = "5780912", id = "68270086", value = "qqq" };
            ans3 = new Report() { idb = "5790912", id = "68280086", value = "qqq" };
            ans4 = new Report() { idb = "5800912", id = "68290086", value = "qqq" };

            ansvers.Add(InterviewerTests.CreateAnswer(res.Item2, ans1, ans2, ans3, ans4));
            res = DoctorChecker.SetAnsvers(ansvers, dialogType);

            ans1 = new Report() { idb = "1210910", id = "68140086", value = "qqq" };
            ans2 = new Report() { idb = "2290123", id = "69970086", value = "1123" };
            ans3 = new Report() { idb = "5780912", id = "68150086", value = "qqq" };
            ans4 = new Report() { idb = "5790912", id = "68160086", value = "qqq" };

            ansvers.Add(InterviewerTests.CreateAnswer(res.Item2, ans1, ans2, ans3, ans4));
            res = DoctorChecker.SetAnsvers(ansvers, dialogType);

            ans1 = new Report() { idb = "1210910", id = "68380086", value = "qqq" };
            ans2 = new Report() { idb = "5780912", id = "68390086", value = "qqq" };
            ans3 = new Report() { idb = "2720123", id = "68700086", value = "1111" };
            ans4 = new Report() { idb = "5800912", id = "68410086", value = "111" };
            ans5 = new Report() { idb = "5790912", id = "68400086", value = "1111" };
            Answer answer = InterviewerTests.CreateAnswer(res.Item2, ans1, ans2, ans3, ans4, ans5);
            ansvers.Add(answer);//;
            res = DoctorChecker.SetAnsvers(ansvers, dialogType);
            Assert.IsTrue(res.Item1[0].Label.Contains("доктор"));
        }
    }
    [TestClass]
    public class ProbabilityTests///TODO перенести отработанные сценарии в checkertest
    {
        static Interviewer CheckerTemplate1;
        static ProbabilityCalculator calc;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Interviewer.Factory Factory = new Interviewer.Factory(null);
            CheckerTemplate1 = Factory.Create(IdParser.ParseBigIdToNewBigId(860000506), Secrets.Semantic, Secrets.Descriptions, Secrets.VarcharList,Secrets.BoolSattelite, Secrets.GetNames);
            calc = ProbabilityCalculator.Factory.Create(IdParser.ParseBigIdToNewBigId(860000506), Secrets.Semantic, Secrets.Descriptions);
        }
    }
}
