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
    }
}
