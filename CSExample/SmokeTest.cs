using RDF.BCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSExample
{
    internal class SmokeTest
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Run()
        {
            Console.WriteLine($"Running smoke tests. Current directory {System.IO.Directory.GetCurrentDirectory()}.");
            Console.WriteLine("...");

            Console.WriteLine("TEST Errors");
            Errors();

            Console.WriteLine("TEST Extensions");
            Extensions();

            Console.WriteLine("TESTS PASSED");
        }

        /// <summary>
        /// 
        /// </summary>
        static void ASSERT(bool condition)
        {
            if (!condition)
            {
                throw new ApplicationException("ASSERT failed");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static void Errors()
        {
            using (var bcf = new RDF.BCF.Project())
            {
                var errors = bcf.GetErrors();
                ASSERT(errors.Length==0);

                Console.WriteLine("Expected errors....");
                var res = bcf.ReadFile("J:\\NotExist.bcf");
                ASSERT(!res);

                errors = bcf.GetErrors(false);
                ASSERT(errors.Length != 0);
                Console.WriteLine(errors);

                errors = bcf.GetErrors();
                ASSERT(errors.Length != 0);

                errors = bcf.GetErrors();
                ASSERT(errors.Length == 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static void Extensions()
        {
            using (var bcf = new RDF.BCF.Project())
            {
                var res = bcf.ReadFile("..\\TestCases\\User assignment.bcf");
                ASSERT(res);

                var guid = bcf.ProjectId;
                ASSERT(guid == "de894a86-3a08-4ea0-b2d1-6c222b5602d1");

                var name = bcf.Name;
                ASSERT(name == "BCF 3.0 test cases");

                bcf.Name = "rename";
                ASSERT(bcf.Name == "rename");

                var lst = bcf.Extensions.GetEnumeration(Native.BCFEnumeration.Users);
                ASSERT(lst.Count() == 3);
            }

            //
            //
            var users = new string[] { "a.b@mail.com", "b " }; // Китайский 好 text" };

            using (var bcf  = new RDF.BCF.Project())
            {
                var lst = bcf.Extensions.GetEnumeration(Native.BCFEnumeration.Users);
                ASSERT(lst.Count() == 0);

                Console.WriteLine("Expected NULL argument error...");
                var res = bcf.Extensions.AddEnumerationElement(Native.BCFEnumeration.Users, null);
                ASSERT(!res);

                var msg = bcf.GetErrors();
                ASSERT(msg.Length != 0);
                Console.WriteLine(msg);

                res = bcf.Extensions.AddEnumerationElement(Native.BCFEnumeration.Users, users[0]);
                ASSERT(res);

                bcf.Extensions.AddEnumerationElement(Native.BCFEnumeration.Users, users[1]);
                bcf.Extensions.AddEnumerationElement(Native.BCFEnumeration.Users, users[0]);

                lst = bcf.Extensions.GetEnumeration(Native.BCFEnumeration.Users);
                ASSERT(lst.Count() == 2);
                int i = 0;
                foreach (var u in lst)
                {
                    ASSERT(u == users[i++]);
                }

                bcf.Extensions.RemoveEnumerationElement(Native.BCFEnumeration.Users, users[1]);
                bcf.Extensions.RemoveEnumerationElement(Native.BCFEnumeration.Users, users[0]);
                lst = bcf.Extensions.GetEnumeration(Native.BCFEnumeration.Users);
                ASSERT(lst.Count() == 0);
            }
        }
    }
}
