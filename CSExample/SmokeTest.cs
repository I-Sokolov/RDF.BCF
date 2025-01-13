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
            CommentAndViewPoints();

            Console.WriteLine("TEST Validarions");
            Validations();

            Console.WriteLine("Dataset test");
            SmokeTest_DataSet("W:\\DevArea\\buildingSMART\\BCF-XML\\Test Cases\\v3.0");

            Console.WriteLine("TESTS PASSED");
        }

        [System.Runtime.InteropServices.DllImport("bcfEngine.dll", EntryPoint = "SmokeTest_DataSet")]

        private static extern void SmokeTest_DataSet(string folder);

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

        const string chinaUser = "My name будет 好";

        static void CheckExtensions(Project bcf)
        {
            var guid = bcf.ProjectId;
            ASSERT(guid == "de894a86-3a08-4ea0-b2d1-6c222b5602d1");

            bcf.Name = "rename";
            ASSERT(bcf.Name == "rename");

            var users = bcf.Extensions.GetEnumeration(Interop.BCFEnumeration.Users);
            ASSERT(users.Count() == 4);
            ASSERT(users[3] == chinaUser);

            var topics = bcf.Topics;
            ASSERT(topics.Count() == 1);

            var topic = topics.First();
            ASSERT(topic.Guid != "7ad1a717-bf20-4c12-b511-cbd90370ddba");
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

                var name = bcf.Name;
                ASSERT(name == "BCF 3.0 test cases");

                TestTopics(bcf);

                CheckExtensions(bcf);

                var ok = bcf.FileWrite("Кирилица.bcf");
                ASSERT(ok);
            }

            using (var bcf = new Project())
            {
                var res = bcf.FileRead("Кирилица.bcf");
                ASSERT(res);

                CheckExtensions(bcf);
            }

            //
            //
            var users = new string[] { "a.b@mail.com", "b Китайский 好 text", "z3", "z4" };

            using (var bcf = new RDF.BCF.Project("MyProject"))
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

                bcf.Extensions.EnumerationElementAdd(Interop.BCFEnumeration.Users, users[0]);
                bcf.Extensions.EnumerationElementAdd(Interop.BCFEnumeration.Users, users[1]);
                bcf.Extensions.EnumerationElementAdd(Interop.BCFEnumeration.Users, users[2]);
                bcf.Extensions.EnumerationElementAdd(Interop.BCFEnumeration.Users, users[3]);

                lst = bcf.Extensions.GetEnumeration(Interop.BCFEnumeration.Users);
                ASSERT(lst.Count() == 4);
                int i = 0;
                foreach (var u in lst)
                {
                    ASSERT(u == users[i++]);
                }

                bcf.Extensions.EnumerationElementRemove(Interop.BCFEnumeration.Users, users[1]);
                bcf.Extensions.EnumerationElementRemove(Interop.BCFEnumeration.Users, users[0]);
                lst = bcf.Extensions.GetEnumeration(Interop.BCFEnumeration.Users);
                ASSERT(lst.Count() == 2);
                i = 2;
                foreach (var u in lst)
                {
                    ASSERT(u == users[i++]);
                }
                var ok = bcf.FileWrite("TestExtensions.bcf");
                ASSERT(ok);
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
                topic = bcf.AddTopic("Topic Type", "Topic Title", "Topic Status");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ex = true;
            }
            ASSERT(ex);

            //
            bcf.SetAuthor(chinaUser, false);

            Console.WriteLine("Expected exception - author unknown");
            ex = false;
            try
            {
                topic = bcf.AddTopic("Topic Type", "Topic Title", "Topic Status");
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
            bcf.SetAuthor(chinaUser, true);

            topic = bcf.AddTopic("Topic Type", "Topic Title", "Topic Status");
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
        }

        static private void Topics()
        {
            using (var bcf = new RDF.BCF.Project("MyProject"))
            {
                bool ok = bcf.SetAuthor("Smoke-tester", true);
                ASSERT(ok);

                SetTopicAttributes(bcf);

                CheckTopicAttributes(bcf, true);

                ok = bcf.FileWrite("TopicsTest.bcf");
                ASSERT(ok);
            }

            using (var bcf = new RDF.BCF.Project())
            {
                var ok = bcf.FileRead("TopicsTest.bcf");
                ASSERT(ok);

                CheckTopicAttributes(bcf, true);

                bcf.SetAuthor("Smoke-Editor", true);

                bcf.Topics[0].Title = "Modified title";

                CheckTopicAttributes(bcf, false);

                ok = bcf.FileWrite("TopicsTest2.bcf");
                ASSERT(ok);
            }

            using (var bcf = new RDF.BCF.Project())
            {
                var ok = bcf.FileRead("TopicsTest2.bcf");
                ASSERT(ok);

                CheckTopicAttributes(bcf, false);
            }
        }

        static private string TestDate(int i)
        {
            return $"202{i}-01-10T12:03:53+04:00";
        }

        static private string TestGuid(int i)
        {
            return $"aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa{i}";
        }

        static private void SetTopicAttributes(RDF.BCF.Project bcf)
        {
            var topic = bcf.AddTopic("Type1", "Title1", "Status1");
            bcf.AddTopic("Type1", "Title1", "Status1", TestGuid(0));
            ASSERT(topic != null);
            if (topic != null)
            {
                topic.TopicType = "Type";
                topic.Title = "Title";
                topic.TopicStatus = "Status";
                topic.ServerAssignedId = "ServerAssignedId";
                topic.TopicType = "TopicType";
                topic.Priority = "Priority";
                topic.DueDate = TestDate(0);
                topic.AssignedTo = "AssignedTo";
                topic.Description = "Description";
                topic.Stage = "Stage";
                topic.Index = 7;
            }

        }

        static private void CheckTopicAttributes(RDF.BCF.Project bcf, bool newFile)
        {
            ASSERT(bcf.Topics.Count() == 2);

            var topic = bcf.Topics[1];
            ASSERT(topic != null);
            if (topic != null)
            {
                ASSERT(topic.Guid == TestGuid(0));
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
                ASSERT(topic.TopicStatus == "Status");
                ASSERT(topic.ServerAssignedId == "ServerAssignedId");
                ASSERT(topic.TopicType == "TopicType");
                ASSERT(topic.Priority == "Priority");
                ASSERT(topic.DueDate == TestDate(0));
                ASSERT(topic.AssignedTo == "AssignedTo");
                ASSERT(topic.Description == "Description");
                ASSERT(topic.Stage == "Stage");
                ASSERT(topic.Index == 7);

                ASSERT(topic.CreationDate.Length > 0);
                ASSERT(topic.CreationAuthor == "Smoke-tester");
                if (newFile)
                {
                    ASSERT(topic.Title == "Title");
                    ASSERT(topic.ModifiedDate.Length == 0);
                    ASSERT(topic.ModifiedAuthor.Length == 0);
                }
                else
                {
                    ASSERT(topic.Title == "Modified title");
                    ASSERT(topic.ModifiedDate.Length > 0);
                    ASSERT(topic.ModifiedAuthor == "Smoke-Editor");
                }
            }
        }

        static void CommentAndViewPoints()
        {
            using (var bcf = new RDF.BCF.Project("MyProject"))
            {
                bool ok = bcf.SetAuthor("Smoke-tester", true);
                ASSERT(ok);

                SetCommentAndViewPoints(bcf);
                CheckCommentAndViewPoints(bcf, false, false);

                ok = bcf.FileWrite("TestCommentsVP.bcf");
                ASSERT(ok);
            }

            using (var bcf = new Project())
            {
                bool ok = bcf.FileRead("TestCommentsVP.bcf");
                ASSERT(ok);

                CheckCommentAndViewPoints(bcf, true, false);

                ok = bcf.SetAuthor("Smoke-Editor", true);
                ASSERT(ok);

                bcf.Topics[0].Comments[0].Text = "Modified text";

                CheckCommentAndViewPoints(bcf, true, true);

                ok = bcf.FileWrite("TestCommentsVP2.bcf");
                ASSERT(ok);
            }

            using (var bcf = new Project())
            {
                bool ok = bcf.FileRead("TestCommentsVP2.bcf");
                ASSERT(ok);

                CheckCommentAndViewPoints(bcf, true, true);
            }
        }
         
        static void SetCommentAndViewPoints(Project bcf)
        {
            var topic = bcf.AddTopic("Type", "Title", "New");
            ASSERT(topic.Comments.Count == 0);

            SetFiles(topic);
            SetViewPoints(topic);

            var comment = topic.AddComment();

            ASSERT(topic.Comments.Count == 1);

            var comment2 = topic.AddComment();
            ASSERT(topic.Comments.Count == 2);
            comment2.Remove();
            ASSERT(topic.Comments.Count == 1);


            comment.Text = "Text comment";
            comment.ViewPoint = topic.ViewPoints[0];
        }

        static void CheckCommentAndViewPoints(Project bcf, bool read, bool modified)
        {
            var topic = bcf.Topics[0];

            CheckFiles(topic);
            CheckViewPoints(topic, read);

            var comment = topic.Comments[0];

            ASSERT(comment.ViewPoint!=null && comment.ViewPoint.Guid == TestGuid(0));
            ASSERT(comment.Date.Length == 25);
            ASSERT(comment.Author == "Smoke-tester");
            if (!modified)
            {
                ASSERT(comment.Text == "Text comment");
                ASSERT(comment.ModifiedDate.Length==0);
                ASSERT(comment.ModifiedAuthor.Length==0);
            }
            else
            {
                ASSERT(comment.Text == "Modified text");
                ASSERT(comment.ModifiedDate.Length == 25);
                ASSERT(comment.ModifiedAuthor == "Smoke-Editor");
            }
        }

        static string TestFile(string ext)
        {
            var cwd = System.IO.Directory.GetCurrentDirectory();
            var filePath = Path.Combine(cwd, "..", "TestCases", "Architectural.");
            filePath = filePath + ext;
            ASSERT(System.IO.File.Exists(filePath));
            return filePath;
        }

        static void SetFiles(Topic topic)
        {
            ASSERT(topic.Files.Count == 0);


            for (int i = 0; i < 5; i++)
            {
                bool isExternal = (i % 2 == 0);
                var reference = TestFile("ifc");

                RDF.BCF.BIMFile file;
                if (i < 2)
                {
                    file = topic.AddFile(reference, isExternal);
                }
                else
                {
                    file = topic.AddFile(null);
                    file.Reference = reference;
                    file.IsExternal = (i % 2 == 0);
                }

                if (i > 1)
                {
                    file.Filename = $"Name-{i}";
                    file.Date = TestDate(i);
                }

                file.IfcProject=TestIfcGuid(i+1);
                file.IfcSpatialStructureElement = TestIfcGuid(i);
            }

            ASSERT(topic.Files.Count == 5);

            topic.Files[4].Remove();
            ASSERT(topic.Files.Count == 4);
        }

        static private string TestIfcGuid(int i)
        {
            return $"{i}{i}{i}{i}Uv4EX5LAhcVpDp2dUH";
        }

        static void CheckFiles(Topic topic)
        {
            ASSERT(topic.Files.Count == 4);

            for (int i = 0; i < 4; i++)
            {
                bool isExternal = (i % 2 == 0);
                var reference = TestFile("ifc");

                var file = topic.Files[i];
                if (i > 1)
                {
                    ASSERT(file.Filename == $"Name-{i}");
                    ASSERT(file.Date == TestDate(i));
                }
                else
                {
                    ASSERT(file.Filename == "Architectural.ifc");
                    ASSERT(file.Date.Length == 25);
                }
                if (isExternal)
                {
                    ASSERT(file.Reference == reference);
                }
                else
                {
                    ASSERT(file.Reference.EndsWith("Architectural.ifc"));
                }
                ASSERT(file.IsExternal== isExternal);
                ASSERT(file.IfcProject == TestIfcGuid(i+1));
                ASSERT(file.IfcSpatialStructureElement == TestIfcGuid(i));
            }
        }
        static void SetViewPoints(Topic topic)
        {
            ASSERT(topic.ViewPoints.Count == 0);

            topic.AddViewPoint();
            ASSERT(topic.ViewPoints[0].Guid.Length > 0);
            ASSERT(topic.ViewPoints[0].Snapshot.Length == 0);

            for (int i = 0; i < 4; i++)
            {
                var vp = topic.AddViewPoint(TestGuid(i));

                bool b = (i % 2 == 0);

                vp.DefaultVisibility = b;
                vp.SpaceVisible = !b;
                vp.SpaceBoundariesVisible = b;
                vp.OpeningsVisible = !b;
                vp.CameraType = b ? Interop.BCFCamera.Perspective : Interop.BCFCamera.Orthogonal;
                vp.SetCameraViewPoint(new Interop.BCFPoint(i, i + .1, i + .2));
                vp.SetCameraDirection(new Interop.BCFPoint(i + .3, i + .4, i + .5 ));
                vp.SetCameraUpVector(new Interop.BCFPoint(i + .6, i + .7, i + .8 ));
                vp.ViewToWorldScale = i * 3;
                vp.FieldOfView = i * 3.5 + 0.1;
                vp.AspectRatio = i * 4 + 0.1;
                vp.Snapshot = TestFile("png");
            }

            ASSERT(topic.ViewPoints.Count == 5);

            ASSERT(topic.ViewPoints[0].Remove());
            ASSERT(topic.ViewPoints.Count == 4);
        }

        static bool EQ(Interop.BCFPoint pt1, Interop.BCFPoint pt2)
        {
            return pt1.X == pt2.X && pt1.Y == pt2.Y && pt1.Z == pt2.Z;
        }

        static void CheckViewPoints(Topic topic, bool read)
        {
            ASSERT(topic.ViewPoints.Count == 4);

            for (int i = 0; i < 4; i++)
            {
                var vp = topic.ViewPoints[i];

                ASSERT(vp.Guid == TestGuid(i));

                bool b = (i % 2 == 0);

                ASSERT(vp.DefaultVisibility == b);
                ASSERT(vp.SpaceVisible == !b);
                ASSERT(vp.SpaceBoundariesVisible == b);
                ASSERT(vp.OpeningsVisible == !b);
                if (b)
                {
                    ASSERT(vp.CameraType == Interop.BCFCamera.Perspective);
                    ASSERT(vp.FieldOfView == i * 3.5 + 0.1);
                    ASSERT(vp.ViewToWorldScale == (read ? 0 : i * 3));
                }
                else
                {
                    ASSERT(vp.CameraType == Interop.BCFCamera.Orthogonal);
                    ASSERT(vp.FieldOfView == (read ? 0 : i * 3.5 + 0.1));
                    ASSERT(vp.ViewToWorldScale == i * 3);
                }
                ASSERT(EQ(vp.GetCameraViewPoint(), new Interop.BCFPoint(i, i + .1, i + .2 )));
                ASSERT(EQ(vp.GetCameraDirection(), new Interop.BCFPoint(i + .3, i + .4, i + .5 )));
                ASSERT(EQ(vp.GetCameraUpVector(), new Interop.BCFPoint( i + .6, i + .7, i + .8 )));
                ASSERT(vp.AspectRatio == i * 4+0.1);
                ASSERT(vp.Snapshot.EndsWith("Architectural.png"));
                ASSERT(Path.Exists(vp.Snapshot));
            }
        }

        static void Validations()
        {
            using (var bcf = new Project("MyTset"))
            {
                bcf.SetAuthor("me", true);

                var topic = bcf.AddTopic("", "B", "C");

                ASSERT(bcf.ErrorsGet().Length == 0);
                var ok = bcf.FileWrite("Validation.bcf");
                ASSERT(!ok);
                var err = bcf.ErrorsGet();
                ASSERT(err.Contains("Missed property"));
                ASSERT(err.Contains("TopicType"));

                topic.TopicType = "Type";
                ok = bcf.FileWrite("Validation.bcf");
                ASSERT(ok);

                //
                var file = topic.AddFile(null);

                ASSERT(bcf.ErrorsGet().Length == 0);

                file.Reference = "WrongPath";
                ASSERT(file.Reference.Length == 0);
                ASSERT(bcf.ErrorsGet().Length != 0);

                file.Date = "WrongDate";
                ASSERT(file.Date.Length == 0);
                ASSERT(bcf.ErrorsGet().Length != 0);

                file.IfcSpatialStructureElement = "wrongGUID";
                ASSERT(file.IfcSpatialStructureElement.Length == 0);
                ASSERT(bcf.ErrorsGet().Length != 0);

                //
                var comment = topic.AddComment();
                ASSERT(bcf.ErrorsGet().Length == 0);

                ok = bcf.FileWrite("Validation.bcf");
                ASSERT(!ok);
                err = bcf.ErrorsGet();
                ASSERT(err.Contains("Missed property"));
                ASSERT(err.Contains("Comment"));

                comment.Text = "Text";
                ok = bcf.FileWrite("Validation.bcf");
                ASSERT(ok);

                //component
                var viewPoint = topic.AddViewPoint();

                ok = bcf.FileWrite("Validation.bcf");
                err = bcf.ErrorsGet();
                ASSERT(!ok);
                ASSERT(err.Contains("Missed property"));
                ASSERT(err.Contains("CameraViewPoint"));

                viewPoint.SetCameraViewPoint(new Interop.BCFPoint(0, 0, 0));
                viewPoint.SetCameraDirection(new Interop.BCFPoint(1, 1, 1));
                viewPoint.SetCameraUpVector(new Interop.BCFPoint(0, 0, 1));
                viewPoint.FieldOfView = 90;
                viewPoint.AspectRatio = 1;

                ok = bcf.FileWrite("Validation.bcf");
                ASSERT(ok);
            }
        }
    }
}
