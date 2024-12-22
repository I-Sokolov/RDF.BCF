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
        public static void Run()
        {
            Errors();
            Extensions();
        }

        static void ASSERT(bool condition)
        {
            if (!condition)
            {
                throw new ApplicationException("ASSERT failed");
            }
        }

        static void Errors()
        {
            using (var bcf = new RDF.BCF.Project())
            {
                var errors = bcf.GetErrors();
                ASSERT(errors.Length==0);

                var res = bcf.ReadFile("J:\\NotExist.bcf");
                ASSERT(!res);

                errors = bcf.GetErrors();
                ASSERT(errors.Length != 0);
                Console.WriteLine(errors);

                bcf.ClearErrors();

                errors = bcf.GetErrors();
                ASSERT(errors.Length == 0);
            }
        }

        static void Extensions()
        {
            var users = new string[] { "a.b@mail.com", "b " }; // Китайский 好 text" };

            using (var bcf  = new RDF.BCF.Project())
            {
                var lst = bcf.Extensions.GetEnumeration(Native.BCFEnumeration.Users);
                ASSERT(lst.Count() == 0);

                var res = bcf.Extensions.AddEnumerationElement(Native.BCFEnumeration.Users, null);
                ASSERT(!res);

                var msg = bcf.GetErrors();
                ASSERT(msg.Length != 0);
                Console.WriteLine(msg);

                bcf.ClearErrors();

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
