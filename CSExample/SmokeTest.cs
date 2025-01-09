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
            var topic = bcf.AddTopic("Type1", "Title1", "Status1");
            bcf.AddTopic("Type1", "Title1", "Status1", "myGuid");
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

        static void GetCommentVPAttributes(Project bcf, bool newFile)
        {
            var topic = bcf.Topics[0];

            CheckFiles(topic);
            CheckViewPoints(topic);

            var comment = topic.Comments[0];

            ASSERT(comment.Text == "Text comment");
            ASSERT(comment.ViewPoint!=null && comment.ViewPoint.Guid == "ID-2");
            ASSERT(comment.Date.Length == 29);
            ASSERT(comment.Author == "Smoke-tester");
            if (newFile)
            {
                ASSERT(comment.ModifiedDate.Length==0);
                ASSERT(comment.ModifiedAuthor.Length==0);
            }
            else
            {
                ASSERT(comment.ModifiedDate.Length == 29);
                ASSERT(comment.ModifiedAuthor == "Smoke-Editor");
            }
        }

        static void SetFiles(Topic topic)
        {
            ASSERT(topic.Files.Count == 0);

            for (int i = 0; i < 5; i++)
            {
                RDF.BCF.BIMFile file;
                if (i < 2)
                {
                    file = topic.AddFile($"File{i}", i % 2 == 0);
                }
                else
                {
                    file = topic.AddFile(null);
                    file.Filename = $"File{i}";
                    file.Reference = file.Filename;
                    file.IsExternal = (i % 2 == 0);
                }

                file.Date=$"Date-{i}";
                file.IfcProject=$"Project-{i}";
                file.IfcSpatialStructureElement = $"SPA-{i}";
            }

            ASSERT(topic.Files.Count == 5);

            topic.Files[4].Remove();
            ASSERT(topic.Files.Count == 4);
        }

        static void CheckFiles(Topic topic)
        {
            ASSERT(topic.Files.Count == 4);

            for (int i = 0; i < 4; i++)
            {
                var file = topic.Files[i];
                ASSERT(file.Filename == $"File{i}");
                ASSERT(file.Reference == file.Filename);
                ASSERT(file.IsExternal== (i % 2 == 0));                
                ASSERT(file.Date == $"Date-{i}");
                ASSERT(file.IfcProject == $"Project-{i}");
                ASSERT(file.IfcSpatialStructureElement == $"SPA-{i}");
            }
        }
        static ViewPoint SetViewPoints(Topic topic)
        {
            ASSERT(topic.ViewPoints.Count == 0);

            topic.AddViewPoint();
            ASSERT(topic.ViewPoints[0].Guid.Length > 0);
            ASSERT(topic.ViewPoints[0].Snapshot.Length == 0);

            for (int i = 2; i < 4; i++)
            {
                var vp = topic.AddViewPoint($"ID-{i}");

                vp.DefaultVisibility = (i == 0);
                vp.SpaceVisible = (i != 0);
                vp.SpaceBoundariesVisible = (i == 0);
                vp.OpeningsVisible = (i != 0);
                vp.CameraType = (i == 0) ? Interop.BCFCamera.Perspective : Interop.BCFCamera.Orthogonal;
                vp.SetCameraViewPoint(new Interop.BCFPoint() { x = i, y = i + .1, z = i + .2 });
                vp.SetCameraDirection(new Interop.BCFPoint() { x = i + .3, y = i + .4, z = i + .5 });
                vp.SetCameraUpVector(new Interop.BCFPoint() { x = i + .6, y = i + .7, z = i + .8 });
                vp.ViewToWorldScale = i * 3;
                vp.FieldOfView = i * 3.5;
                vp.AspectRatio = i * 4;
                vp.Snapshot = $"Snap {i}";
            }

            ASSERT(topic.ViewPoints.Count == 3);

            ASSERT(topic.ViewPoints[0].Remove());
            ASSERT(topic.ViewPoints.Count == 2);

            return topic.ViewPoints[0];
        }

        static bool EQ(Interop.BCFPoint pt1, Interop.BCFPoint pt2)
        {
            return pt1.x == pt2.x && pt1.y == pt2.y && pt1.z == pt2.z;
        }

        static void CheckViewPoints(Topic topic)
        {
            ASSERT(topic.ViewPoints.Count == 2);

            for (int i = 2; i < 4; i++)
            {
                var vp = topic.ViewPoints[i - 2];

                ASSERT(vp.Guid == $"ID-{i}");

                ASSERT(vp.DefaultVisibility == (i == 0));
                ASSERT(vp.SpaceVisible == (i != 0));
                ASSERT(vp.SpaceBoundariesVisible == (i == 0));
                ASSERT(vp.OpeningsVisible == (i != 0));
                ASSERT(vp.CameraType == ((i == 0) ? Interop.BCFCamera.Perspective : Interop.BCFCamera.Orthogonal));
                ASSERT(EQ(vp.GetCameraViewPoint(), new Interop.BCFPoint() { x = i, y = i + .1, z = i + .2 }));
                ASSERT(EQ(vp.GetCameraDirection(), new Interop.BCFPoint() { x = i + .3, y = i + .4, z = i + .5 }));
                ASSERT(EQ(vp.GetCameraUpVector(), new Interop.BCFPoint() { x = i + .6, y = i + .7, z = i + .8 }));
                ASSERT(vp.ViewToWorldScale == i * 3);
                ASSERT(vp.FieldOfView == i * 3.5);
                ASSERT(vp.AspectRatio == i * 4);
                ASSERT(vp.Snapshot == $"Snap {i}");
            }
        }
    }
}
