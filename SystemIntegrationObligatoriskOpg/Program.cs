using Data;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SystemIntegrationObligatoriskOpg
{
    public class Program
    {
        public const string GET_GENDER_REQUEST = "CprGetGender";
        public const string UPDATE_FROM_CPR_REQUEST = "CprUpdate";

        private static void Main(string[] args)
        {
            var euccid = new EUCCIdRegistre();
            var cpr = new CPRRegistre();
            var hospital = new HospitalSystem(euccid);

            MessageQueuesManager.Instance.CreateQueue(hospital, "Hospital");
            MessageQueuesManager.Instance.CreateQueue(MessageRouter.Instance, "Message");

            MessageQueuesManager.Instance.CreateQueue<Tuple<string, string>>(cpr, "CprPath1");
            MessageQueuesManager.Instance.CreateQueue<Tuple<string, Gender>>(cpr, "CprPath2");

            MessageQueuesManager.Instance.CreateQueue<CPRRegisterData>(euccid, "cprReceive");
            MessageQueuesManager.Instance.CreateQueue<Tuple<string, Gender, string, string, string, string>>(euccid, "eupath");

            MessageRouter.Instance.AddEUCCIDQueue(typeof(CPRRegisterData), euccid[typeof(CPRRegisterData)]);

            List<string> options = new List<string>() { "Give birth", "Update all current cpr info", "See available cpr", "See available euccid" };

            while (options.Count > 0)
            {
                Console.Clear();

                Console.WriteLine("Press escape to close");
                Console.WriteLine("Press given key for action");

                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine(options[i] + " " + (i + 1).ToString());
                }

                var key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                        GiveBirth(cpr, euccid, hospital);
                        Console.ReadKey();
                        break;

                    case ConsoleKey.D2:
                        UpdateCPRNumbers(cpr, euccid);
                        Console.ReadKey();
                        break;

                    case ConsoleKey.Escape:
                        options.Clear();
                        break;

                    case ConsoleKey.D3:
                        AvailableCpr(cpr);
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void AvailableCpr(CPRRegistre cpr)
        {
            Console.Clear();

            for (int i = 0; i < cpr.Count; i++)
            {
                var ph = cpr[i];

                Console.WriteLine("Firstname: " + ph.FirstName + " | Lastname: " + ph.LastName + " | Cpr-nr " + ph.CPRNumber);
            }

            Console.ReadKey();
        }

        private static void UpdateCPRNumbers(CPRRegistre cpr, EUCCIdRegistre euccid)
        {
            Console.Clear();
            cpr.SendMessages(euccid);
            Console.ReadKey();
        }

        private static void GiveBirth(CPRRegistre cpr, EUCCIdRegistre euccid, HospitalSystem hospital)
        {
            string familyName = string.Empty;

            while (familyName.Length == 0)
            {
                Console.Clear();
                Console.WriteLine("Familyname only one name");
                var ph = Console.ReadLine();

                if (!ph.Contains(" "))
                {
                    familyName = ph;
                }
            }

            Console.WriteLine("Gender press 1 for male, or 2 for female");

            Gender gender = Gender.None;

            while (gender == Gender.None)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        gender = Gender.Male;
                        break;

                    case ConsoleKey.D2:
                        gender = Gender.Female;
                        break;
                }
            }

            Console.WriteLine("Birthcountry");

            var birthCountry = Console.ReadLine();

            Console.WriteLine("Adress");

            var adress = Console.ReadLine();

            while (!Regex.IsMatch(adress, @"\d"))
            {
                adress = Console.ReadLine();
            }

            Console.WriteLine("Apartment number write none if you dont live in a flat");
            var apartmentNumber = Console.ReadLine();

            while (!Regex.IsMatch(apartmentNumber, @"\d") && apartmentNumber.ToLower() != "none")
            {
                apartmentNumber = Console.ReadLine();
            }

            Console.WriteLine("City");

            hospital.SendMessage(familyName, gender, birthCountry, adress, apartmentNumber, Console.ReadLine());
        }
    }
}