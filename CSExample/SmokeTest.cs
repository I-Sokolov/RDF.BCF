using RDF.BCF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                var errors = bcf.ErrorsGet();
                ASSERT(errors.Length==0);

                Console.WriteLine("Expected errors....");
                var res = bcf.FileRead("J:\\NotExist.bcf");
                ASSERT(!res);

                errors = bcf.ErrorsGet(false);
                ASSERT(errors.Length != 0);
                Console.WriteLine(errors);

                errors = bcf.ErrorsGet();
                ASSERT(errors.Length != 0);

                errors = bcf.ErrorsGet();
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
                var res = bcf.FileRead("..\\TestCases\\User assignment.bcf");
                ASSERT(res);

                var guid = bcf.ProjectId;
                ASSERT(guid == "de894a86-3a08-4ea0-b2d1-6c222b5602d1");

                var name = bcf.Name;
                ASSERT(name == "BCF 3.0 test cases");

                bcf.Name = "rename";
                ASSERT(bcf.Name == "rename");

                var lst = bcf.Extensions.GetEnumeration(Interop.BCFEnumeration.Users);
                ASSERT(lst.Count() == 3);

                TestTopics(bcf);
            }

            //
            //
            var users = new string[] { "a.b@mail.com", "b " }; // Китайский 好 text" };

            using (var bcf  = new RDF.BCF.Project())
            {
                var lst = bcf.Extensions.GetEnumeration(Interop.BCFEnumeration.Users);
                ASSERT(lst.Count() == 0);

                Console.WriteLine("Expected NULL argument error...");
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                var res = bcf.Extensions.EnumerationElementAdd(Interop.BCFEnumeration.Users, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ASSERT(!res);

                var msg = bcf.ErrorsGet();
                ASSERT(msg.Length != 0);
                Console.WriteLine(msg);

                res = bcf.Extensions.EnumerationElementAdd(Interop.BCFEnumeration.Users, users[0]);
                ASSERT(res);

                bcf.Extensions.EnumerationElementAdd(Interop.BCFEnumeration.Users, users[1]);
                bcf.Extensions.EnumerationElementAdd(Interop.BCFEnumeration.Users, users[0]);

                lst = bcf.Extensions.GetEnumeration(Interop.BCFEnumeration.Users);
                ASSERT(lst.Count() == 2);
                int i = 0;
                foreach (var u in lst)
                {
                    ASSERT(u == users[i++]);
                }

                bcf.Extensions.EnumerationElementRemove(Interop.BCFEnumeration.Users, users[1]);
                bcf.Extensions.EnumerationElementRemove(Interop.BCFEnumeration.Users, users[0]);
                lst = bcf.Extensions.GetEnumeration(Interop.BCFEnumeration.Users);
                ASSERT(lst.Count() == 0);
            }
        }

        static private void TestTopics(RDF.BCF.Project bcf)
        {
            RDF.BCF.Topics topics = bcf.Topics;

            var items = topics.Items;
            ASSERT(items.Count() == 1);
            
            var topic = items.First();
            ASSERT(topic.Guid == "7ad1a717-bf20-4c12-b511-cbd90370ddba");

            //
            // new topic create
            // 
            Console.WriteLine("Expected error - author not set");
            topic = topics.TopicCreate("Topic Type", "Topic Title", "Topic Status");
            ASSERT(topic == null);
            Console.WriteLine(bcf.ErrorsGet());

            //
            bcf.SetEditor("John Smith", false);

            Console.WriteLine("Expected error - author unknown");
            topic = topics.TopicCreate("Topic Type", "Topic Title", "Topic Status");
            ASSERT(topic == null);
            Console.WriteLine(bcf.ErrorsGet());

            items = topics.Items;
            ASSERT(items.Count() == 1);

            //
            bcf.SetEditor("John Smith", true);

            topic = topics.TopicCreate("Topic Type", "Topic Title", "Topic Status");
            ASSERT(topic != null);


            items = topics.Items;
            ASSERT(items.Count() == 2);
            topic = items.First();

            //
            // remove
            //
            if (topic != null)
            {
                topics.TopicRemove(topic);
            }

            items = topics.Items;
            ASSERT(items.Count() == 1);

            topic = items.First();
            ASSERT(topic.Guid != "7ad1a717-bf20-4c12-b511-cbd90370ddba");
        }
    }
}
