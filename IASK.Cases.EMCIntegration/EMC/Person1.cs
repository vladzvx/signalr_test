using EMCLib.EMC.Requests;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using UMKBRequests;

namespace IASK.Cases.EMCIntegration
{
    public static class Person1
    {
        private static Random rnd = new Random();
        public static string GetFIO(ulong id)
        {
            return "фамилия имя отчество " + id.ToString();
        }

        public static CrObj CreatePatient(ulong id, DateTime birthDate)
        {
            CrObj result = new CrObj();
            result.id = id.ToString();
            result.fio = GetFIO(id);
            result.bdate = birthDate.Ticks;
            result.email = "somemail" + id.ToString() + "@mail.ru";
            result.username = id.ToString();
            result.userid = id.ToString();
            result.Type = "pat";
            result.creator = "1234567890";
            result.snils = "123-123-123-123";
            result.passport = "4505 550055";
            result.phone = "+79030000000";
            result.username = id.ToString();

            return result;
        }

        /// <summary>
        /// Генерация одиночной вакцинации
        /// </summary>
        private static List<EMCProtoM> CreateVactination(DateTime time, ulong PatientId, ushort VactinationSubjectLib, uint VactinationSubjectId)
        {
            string RootProtoId = ObjectId.GenerateNewId().ToString();

            EMCProtoM Therapist = new EMCProtoM()
            {
                _id = RootProtoId,
                time= time,
                timelpu= time,
                timeauthor= time,
                author = 0,
                id = IdParser.GetNewBigId(81, 102),
                patient = PatientId,
                
            };

            string EmptyСomplaintId = ObjectId.GenerateNewId().ToString();

            EMCProtoM EmptyСomplaint = new EMCProtoM()///типа жалоб нет
            {
                _id = EmptyСomplaintId,
                author = 0,
                time = time.AddMinutes(2),
                timelpu = time.AddMinutes(2),
                timeauthor = time.AddMinutes(2),
                id = IdParser.GetNewBigId(81, 1),
                patient = PatientId,
                parent = RootProtoId,
                values = new Dictionary<string, List<string>>() { { "val", new List<string>() { "Жалоб нет" } } }
            };

            string InstrumentalMeasurmentsId = ObjectId.GenerateNewId().ToString();

            EMCProtoM InstrumentalMeasurments = new EMCProtoM()
            {
                _id = InstrumentalMeasurmentsId,
                id = IdParser.GetNewBigId(81, 271),
                patient = PatientId,
                parent = RootProtoId,
                time = time.AddMinutes(5),
                timelpu = time.AddMinutes(5),
                timeauthor = time.AddMinutes(5),
                subjects = new List<EMCSubject>()
                {
                    new EMCSubject()
                    {
                        id=IdParser.GetNewBigId(8,9),
                        valueTable= new List<EMCValTab>()
                        { 
                            new EMCValTab()
                            { 
                                type = 0,
                                field="temperature",
                                value_1 = 36.6,
                                unit_1 = IdParser.GetNewBigId(56,43)
                            }
                        }
                    },
                    new EMCSubject()
                    {
                        id=IdParser.GetNewBigId(8,9),
                        valueTable= new List<EMCValTab>()
                        {
                            new EMCValTab()
                            {
                                type = 0,
                                field="pressue",
                                value_1 = 80,
                                unit_1 = IdParser.GetNewBigId(44,76)
                            }
                        }
                    },
                    new EMCSubject()
                    {
                        id=IdParser.GetNewBigId(8,9),
                        valueTable= new List<EMCValTab>()
                        {
                            new EMCValTab()
                            {
                                type = 0,
                                field="pressue",
                                value_1 = 120,
                                unit_1 = IdParser.GetNewBigId(44,87)
                            }
                        }
                    }
                }
            };

            string AllergyId = ObjectId.GenerateNewId().ToString();

            EMCProtoM Allergy = new EMCProtoM()
            {
                _id = AllergyId,
                time = time.AddMinutes(2),
                timelpu = time.AddMinutes(2),
                timeauthor = time.AddMinutes(2),
                id = IdParser.GetNewBigId(81,229),
                parent = RootProtoId,
                patient = PatientId,
            };

            string VaccinationId = ObjectId.GenerateNewId().ToString();

            EMCProtoM Vaccination = new EMCProtoM()
            {
                _id = VaccinationId,
                id = IdParser.GetNewBigId(34, 2),
                parent = RootProtoId,
                patient = PatientId,
                time = time.AddMinutes(10),
                timelpu = time.AddMinutes(10),
                timeauthor = time.AddMinutes(10),

                subjects = new List<EMCSubject>()
                {
                    new EMCSubject()
                    {
                        id=IdParser.GetNewBigId(VactinationSubjectLib, VactinationSubjectId),//Общая вакцинация от короны Гам Ковид Вак 62:87961, 
                        facts = new List<EMCFact>()
                        {
                            new EMCFact()
                            {
                                id=IdParser.GetNewBigId(VactinationSubjectLib, VactinationSubjectId),
                            }
                        }
                    }
                }
            };

            string VaccinationActionId = ObjectId.GenerateNewId().ToString();

            EMCProtoM VaccinationAction = new EMCProtoM()
            {
                _id = VaccinationActionId,
                id = IdParser.GetNewBigId(81, 334),
                parent = VaccinationId,
                patient = PatientId,
                time = time.AddMinutes(25),
                timelpu = time.AddMinutes(25),
                timeauthor = time.AddMinutes(25),
                values=new Dictionary<string, List<string>>()
                {
                    {"text",new List<string>(){"Вакцинация прошла успешно, побочных эффектов не произошло." } }
                }
            };


            Therapist.subproto = new List<string>()
            {
                EmptyСomplaintId,
                InstrumentalMeasurmentsId,
                AllergyId,
                VaccinationId,
                VaccinationActionId
            };

            return new List<EMCProtoM>() { Therapist, EmptyСomplaint, InstrumentalMeasurments, Allergy, Vaccination, VaccinationAction };
        }

        public static List<List<EMCProtoM>> CreateVactinationsFull(ulong PatientId, DateTime BirthDay)
        {
            List<List<EMCProtoM>> result = new List<List<EMCProtoM>>();
            if (BirthDay.AddDays(1) <DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(1), PatientId, 52, 5613)); //гепатит Б
            if (BirthDay.AddDays(60) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(60),PatientId, 52, 5744));//столбняк, коклюш, дифтерия
            if (BirthDay.AddDays(365) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(365),PatientId, 52, 5744));//столбняк, коклюш, дифтерия
            if (BirthDay.AddDays(300) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(300),PatientId, 52, 5914));//корь, краснуха, паратит
            if (BirthDay.AddDays(700) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(700),PatientId, 52, 5914));//корь, краснуха, паратит
            if (BirthDay.AddDays(550) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(550),PatientId, 52, 6474)); //полиэмилит
            if (BirthDay.AddDays(440) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(440), PatientId, 52, 4746)); //оспа
            return result;
        }

        public static List<List<EMCProtoM>> CreateVactinationsCommon(ulong PatientId, DateTime BirthDay)
        {
            List<List<EMCProtoM>> result = new List<List<EMCProtoM>>();
            if (BirthDay.AddDays(1) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(1), PatientId, 52, 5613)); //гепатит Б
            if (BirthDay.AddDays(60) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(60), PatientId, 52, 5744));//столбняк, коклюш, дифтерия
            if (BirthDay.AddDays(365) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(365), PatientId, 52, 5744));//столбняк, коклюш, дифтерия
            if (BirthDay.AddDays(300) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(300), PatientId, 52, 5914));//корь, краснуха, паратит
            if (BirthDay.AddDays(440) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(440), PatientId, 52, 4746)); //оспа
            return result;
        }

        public static List<List<EMCProtoM>> CreateVactinationsMinimum(ulong PatientId, DateTime BirthDay)
        {
            List<List<EMCProtoM>> result = new List<List<EMCProtoM>>();
            if (BirthDay.AddDays(1) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(1), PatientId, 52, 5613)); //гепатит Б
            return result;
        }

        public static List<List<EMCProtoM>> CreateVactinationsEmpty(ulong PatientId, DateTime BirthDay)
        {
            List<List<EMCProtoM>> result = new List<List<EMCProtoM>>();
            return result;
        }

        public static List<List<EMCProtoM>> CreateVactinationsHalf(ulong PatientId, DateTime BirthDay)
        {
            List<List<EMCProtoM>> result = new List<List<EMCProtoM>>();
            if (BirthDay.AddDays(1) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(1), PatientId, 52, 5613)); //гепатит Б
            if (BirthDay.AddDays(60) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(60), PatientId, 52, 5744));//столбняк, коклюш, дифтерия
            if (BirthDay.AddDays(365) < DateTime.UtcNow)
                result.Add(CreateVactination(BirthDay.AddDays(365), PatientId, 52, 5744));//столбняк, коклюш, дифтерия

            return result;
        }

        public static List<List<EMCProtoM>> CreateCOVID(ulong PatientId, DateTime Date)
        {
            List<List<EMCProtoM>> result = new List<List<EMCProtoM>>();
            if (Date.AddDays(1) < DateTime.UtcNow)
            {
                result.Add(CreateVactination(Date.AddDays(1), PatientId, 52, 20049)); //корона

            }
            return result;
        }

        public static List<List<EMCProtoM>> CreateStolb(ulong PatientId, DateTime Date)
        {
            List<List<EMCProtoM>> result = new List<List<EMCProtoM>>();
            if (Date.AddDays(1) < DateTime.UtcNow)
            {
                result.Add(CreateVactination(Date.AddDays(1), PatientId, 52, 11503));//столбняк
            }
            return result;
        }

        public static List<List<EMCProtoM>> CreateEncephStart(ulong PatientId, DateTime Date)
        {
            List<List<EMCProtoM>> result = new List<List<EMCProtoM>>();
            if (Date.AddDays(1) < DateTime.UtcNow)
            {
                result.Add(CreateVactination(Date.AddDays(1), PatientId, 52, 6110));//энцефалит
            }
            return result;
        }

        public static List<List<EMCProtoM>> CreateEncephFull (ulong PatientId, DateTime Date)
        {
            List<List<EMCProtoM>> result = new List<List<EMCProtoM>>();
            if (Date.AddDays(1) < DateTime.UtcNow)
            {
                result.Add(CreateVactination(Date.AddDays(1), PatientId, 52, 6110));//энцефалит
                result.Add(CreateVactination(Date.AddDays(-150), PatientId, 52, 6110));//энцефалит
                result.Add(CreateVactination(Date.AddDays(-300), PatientId, 52, 6110));//энцефалит
            }
            return result;
        }

        public static List<List<EMCProtoM>> CreateRandomVacinationProfile(ulong PatientId, DateTime BirthDate)
        {
            List<List<EMCProtoM>> result = new List<List<EMCProtoM>>();
            int val = rnd.Next(0, 4);
            switch (val)
            {
                case 0:
                    {
                        result.AddRange(CreateVactinationsFull(PatientId, BirthDate));
                        break;
                    }
                case 1:
                    {
                        result.AddRange(CreateVactinationsHalf(PatientId, BirthDate));
                        break;
                    }
                case 2:
                    {
                        result.AddRange(CreateVactinationsCommon(PatientId, BirthDate));
                        break;
                    }
                case 3:
                    {
                        result.AddRange(CreateVactinationsMinimum(PatientId, BirthDate));
                        break;
                    }
                case 4:
                    {
                        result.AddRange(CreateVactinationsEmpty(PatientId, BirthDate));
                        break;
                    }

            }
            val = rnd.Next(0, 10);
            switch (val)
            {
                case 0:
                case 1:
                case 2:
                    {
                        DateTime dt = BirthDate.AddYears(rnd.Next(14,50));
                        if (dt < DateTime.UtcNow)
                            result.AddRange(CreateStolb(PatientId, dt));
                        break;
                    }
                case 3:
                    {
                        DateTime dt = BirthDate.AddYears(rnd.Next(14, 50));
                        if (dt < DateTime.UtcNow)
                            result.AddRange(CreateEncephFull(PatientId, dt));
                        break;
                    }
                case 4:
                case 5:
                    {

                        if (DateTime.UtcNow.Subtract(BirthDate).TotalDays>18*365)
                            result.AddRange(CreateCOVID(PatientId, DateTime.UtcNow.AddDays(-rnd.Next(0,150))));
                        break;
                    }
                case 6:
                    {
                        DateTime dt = BirthDate.AddYears(rnd.Next(14, 50));
                        if (dt < DateTime.UtcNow)
                            result.AddRange(CreateEncephStart(PatientId, dt));
                        break;
                    }
                   
            }
            return result;
        }


        public static List<List<EMCProtoM>> CreateRandomDiseaseProfile(ulong PatientId, DateTime BirthDate)
        {
            List<List<EMCProtoM>> result = new List<List<EMCProtoM>>();
            int val = rnd.Next(0, 4);
            return result;
        }
    }
}
