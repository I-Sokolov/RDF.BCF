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

            Console.WriteLine("TEST Topics");
            Topics();

            Console.WriteLine("TEST Comments and ViewPoint");
            CommentsVP();

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
                ASSERT(errors.Length == 0);

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
                var res = bcf.FileRead("..\\TestCases\\кИрилица.bcf");
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
            var users = new string[] { "a.b@mail.com", "b Китайский 好 text" };

            using (var bcf = new RDF.BCF.Project())
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
            var items = bcf.Topics;
            ASSERT(items.Count() == 1);

            var topic = items.First();
            ASSERT(topic.Guid == "7ad1a717-bf20-4c12-b511-cbd90370ddba");

            //
            // new topic create
            //
            Console.WriteLine("Expected exception - author not set");
            bool ex = false;
            try
            {
                topic = bcf.CreateTopic("Topic Type", "Topic Title", "Topic Status");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ex = true;
            }
            ASSERT(ex);

            //
            bcf.SetAuthor("John Smith", false);

            Console.WriteLine("Expected exception - author unknown");
            ex = false;
            try
            {
                topic = bcf.CreateTopic("Topic Type", "Topic Title", "Topic Status");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                ex = true;
            }
            ASSERT(ex);

            items = bcf.Topics;
            ASSERT(items.Count() == 1);

            //
            bcf.SetAuthor("John Smith", true);

            topic = bcf.CreateTopic("Topic Type", "Topic Title", "Topic Status");
            ASSERT(topic != null);

            if (topic != null)
            {
                ASSERT(topic.ServerAssignedId.Length == 0);
                var stat = topic.TopicStatus;
                ASSERT(stat == "Topic Status");
                Console.WriteLine(stat);
            }

            items = bcf.Topics;
            ASSERT(items.Count() == 2);
            topic = items.First();

            //
            // remove
            //
            topic.Remove();

            items = bcf.Topics;
            ASSERT(items.Count() == 1);

            topic = items.First();
            ASSERT(topic.Guid != "7ad1a717-bf20-4c12-b511-cbd90370ddba");
        }

        static private void Topics()
        {
            using (var bcf = new RDF.BCF.Project())
            {
                bool ok = bcf.SetAuthor("Smoke-tester", true);
                ASSERT(ok);

                SetTopicAttributes(bcf, true);
                GetTopicAttributes(bcf, true);
            }

            //TODO - save, read, modify
        }

        static private void SetTopicAttributes(RDF.BCF.Project bcf, bool newFile)
        {
            var topic = bcf.CreateTopic("Type1", "Title1", "Status1");
            bcf.CreateTopic("Type1", "Title1", "Status1", "myGuid");
            ASSERT(topic != null);
            if (topic != null)
            {
                topic.TopicType = "Type";
                topic.Title = "Title";
                topic.TopicStatus = "Status";
                topic.ServerAssignedId = "ServerAssignedId";
                topic.TopicType = "TopicType";
                topic.Priority = "Priority";
                topic.DueDate = "DueDate";
                topic.AssignedTo = "AssignedTo";
                topic.Description = "Description";
                topic.Stage = "Stage";
                topic.Index = 7;
            }

        }

        static private void GetTopicAttributes(RDF.BCF.Project bcf, bool newFile)
        {
            ASSERT(bcf.Topics.Count() == 2);

            var topic = bcf.Topics[1];
            ASSERT(topic != null);
            if (topic != null)
            {
                ASSERT(topic.TopicType == "Type1");
                ASSERT(topic.Title == "Title1");
                ASSERT(topic.TopicStatus == "Status1");
                ASSERT(topic.ServerAssignedId == "");
                ASSERT(topic.TopicType == "Type1");
                ASSERT(topic.Priority == "");
                ASSERT(topic.DueDate == "");
                ASSERT(topic.AssignedTo == "");
                ASSERT(topic.Description == "");
                ASSERT(topic.Stage == "");
                ASSERT(topic.Index == 0);

                ASSERT(topic.CreationDate.Length > 0);
                ASSERT(topic.CreationAuthor == "Smoke-tester");
                ASSERT(topic.ModifiedDate.Length == 0);
                ASSERT(topic.ModifiedAuthor.Length == 0);
            }

            topic = bcf.Topics[0];
            ASSERT(topic != null);
            if (topic != null)
            {
                ASSERT(topic.TopicType == "TopicType");
                ASSERT(topic.Title == "Title");
                ASSERT(topic.TopicStatus == "Status");
                ASSERT(topic.ServerAssignedId == "ServerAssignedId");
                ASSERT(topic.TopicType == "TopicType");
                ASSERT(topic.Title == "Title");
                ASSERT(topic.Priority == "Priority");
                ASSERT(topic.DueDate == "DueDate");
                ASSERT(topic.AssignedTo == "AssignedTo");
                ASSERT(topic.Description == "Description");
                ASSERT(topic.Stage == "Stage");
                ASSERT(topic.Index == 7);

                ASSERT(topic.CreationDate.Length > 0);
                ASSERT(topic.CreationAuthor == "Smoke-tester");
                if (newFile)
                {
                    ASSERT(topic.ModifiedDate.Length == 0);
                    ASSERT(topic.ModifiedAuthor.Length == 0);
                }
                else
                {
                    ASSERT(topic.ModifiedDate.Length > 0);
                    ASSERT(topic.ModifiedAuthor == "Smoke-Editor");
                }
            }
        }

        static void CommentsVP()
        {
            using (var bcf = new RDF.BCF.Project())
            {
                bool ok = bcf.SetAuthor("Smoke-tester", true);
                ASSERT(ok);

                SetCommentVPAttributes(bcf, true);
                GetCommentVPAttributes(bcf, true);
            }

            //TODO - save, read, modify
        }

        static void SetCommentVPAttributes(Project bcf, bool newFile)
        {
            var topic = bcf.CreateTopic("Type", "Title", "New");
            ASSERT(topic.Comments.Count == 0);

            var comment = topic.CreateComment();

            ASSERT(topic.Comments.Count == 1);

            var comment2 = topic.CreateComment();
            ASSERT(topic.Comments.Count == 2);
            comment2.Remove();
            ASSERT(topic.Comments.Count == 1);


            comment.Text = "Text comment";
        }

        static void GetCommentVPAttributes(Project bcf, bool newFile)
        {
            var topic = bcf.Topics[0];
            var comment = topic.Comments[0];

            ASSERT(comment.Text == "Text comment");
            ASSERT(comment.ViewPoint == null);
            ASSERT(comment.Date.Length == 28);
            ASSERT(comment.Author == "Smoke-tester");
            if (newFile)
            {
                ASSERT(comment.ModifiedDate.Length==0);
                ASSERT(comment.ModifiedAuthor.Length==0);
            }
            else
            {
                ASSERT(comment.ModifiedDate.Length == 28);
                ASSERT(comment.ModifiedAuthor == "Smoke-Editor");
            }
        }
    }
}
