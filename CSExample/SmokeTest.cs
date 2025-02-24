using RDF.BCF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
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

            Console.WriteLine("TEST Links, Labels, Related topics");
            TestTopicListAttributes();

            Console.WriteLine("TEST Validations");
            Validations();

#if DEBUG
            Console.WriteLine("Dataset test");
            SmokeTest_DataSet("W:\\DevArea\\buildingSMART\\BCF-XML\\Test Cases");
#endif

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
                ASSERT(!bcf.IsModified);

                var errors = bcf.GetErrors();
                ASSERT(errors.Length == 0);

                Console.WriteLine("Expected errors....");
                var res = bcf.FileRead("J:\\NotExist.bcf", false);
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

        const string chinaUser = "My name будет 好";

        static void CheckExtensions(Project bcf)
        {
            var guid = bcf.ProjectId;
            ASSERT(guid == "de894a86-3a08-4ea0-b2d1-6c222b5602d1");

            bcf.Name = "rename";
            ASSERT(bcf.Name == "rename");

            var users = bcf.Extensions.GetElements(Interop.BCFEnumeration.Users);
            ASSERT(users.Count() == 4);
            ASSERT(users[3] == chinaUser);

            var topics = bcf.GetTopics();
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
                var res = bcf.FileRead("..\\TestCases\\кИрилица.bcf", false);
                ASSERT(res);
                ASSERT(!bcf.IsModified);

                var name = bcf.Name;
                ASSERT(name == "BCF 3.0 test cases");

                TestBitmaps(bcf);
                TestSnippest(bcf);
                TestTopics(bcf);
                CheckExtensions(bcf);

                ASSERT(bcf.IsModified);
                var ok = bcf.FileWrite("Кирилица.bcf");
                ASSERT(ok);
                ASSERT(!bcf.IsModified);
            }

            using (var bcf = new Project())
            {
                var res = bcf.FileRead("Кирилица.bcf", false);
                ASSERT(res);

                CheckExtensions(bcf);
            }

            //
            //
            var users = new string[] { "a.b@mail.com", "b Китайский 好 text", "z3", "z4" };

            using (var bcf = new RDF.BCF.Project("MyProject"))
            {
                var lst = bcf.Extensions.GetElements(Interop.BCFEnumeration.Users);
                ASSERT(lst.Count() == 0);

                Console.WriteLine("Expected NULL argument error...");
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                var res = bcf.Extensions.AddElement(Interop.BCFEnumeration.Users, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ASSERT(!res);

                var msg = bcf.GetErrors();
                ASSERT(msg.Length != 0);
                Console.WriteLine(msg);

                res = bcf.Extensions.AddElement(Interop.BCFEnumeration.Users, users[0]);
                ASSERT(res);

                bcf.Extensions.AddElement(Interop.BCFEnumeration.Users, users[0]);
                bcf.Extensions.AddElement(Interop.BCFEnumeration.Users, users[1]);
                bcf.Extensions.AddElement(Interop.BCFEnumeration.Users, users[2]);
                bcf.Extensions.AddElement(Interop.BCFEnumeration.Users, users[3]);

                lst = bcf.Extensions.GetElements(Interop.BCFEnumeration.Users);
                ASSERT(lst.Count() == 4);
                int i = 0;
                foreach (var u in lst)
                {
                    ASSERT(u == users[i++]);
                }

                bcf.Extensions.RemoveElement(Interop.BCFEnumeration.Users, users[1]);
                bcf.Extensions.RemoveElement(Interop.BCFEnumeration.Users, users[0]);
                lst = bcf.Extensions.GetElements(Interop.BCFEnumeration.Users);
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
            var items = bcf.GetTopics();
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
            bcf.SetOptions(chinaUser, false);

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

            items = bcf.GetTopics();
            ASSERT(items.Count() == 1);

            //
            bcf.SetOptions(chinaUser, true);

            topic = bcf.AddTopic("Topic Type", "Topic Title", "Topic Status");
            ASSERT(topic != null);

            if (topic != null)
            {
                topic.SetRelatedTopics(items);

                ASSERT(topic.GetRelatedTopics().Count==1);

                ASSERT(topic.ServerAssignedId.Length == 0);
                var stat = topic.TopicStatus;
                ASSERT(stat == "Topic Status");
                Console.WriteLine(stat);
            }

            items = bcf.GetTopics();
            ASSERT(items.Count() == 2);
            topic = items.First();
            var topic2 = items[1];

            ASSERT(topic2.GetRelatedTopics().Count == 1);
            ASSERT(topic2.GetRelatedTopics()[0].Guid == topic.Guid);

            //
            // remove
            //
            topic.Remove();

            ASSERT(topic2.GetRelatedTopics().Count == 0);
        }

        static void TestBitmaps(Project bcf)
        {
            var topic = bcf.GetTopics().First();
            var viewpoint = topic.GetViewPoints().First();
            
            ASSERT(viewpoint.GetBitmaps().Count == 1);

            var bitmap = viewpoint.GetBitmaps().First();

            ASSERT(bitmap.Format == Interop.BCFBitmapFormat.BCFBitmapPNG);
            ASSERT(bitmap.Reference.EndsWith ("bitmap0.png"));
            ASSERT(EQ(bitmap.GetLocation(), new Interop.BCFPoint(10.064999999983305, -10.40177106506878, 7.011243681990698)));
            ASSERT(EQ(bitmap.GetNormal(), new Interop.BCFPoint(-0.9999999999999999, 1.253656364893038E-16, 0.0)));
            ASSERT(EQ(bitmap.GetUp(), new Interop.BCFPoint(-5.43903050550883E-34, -4.338533794284917E-18, 1.0)));
            ASSERT(bitmap.Height == 1666.1814563907683);
        }

        static void TestSnippest(Project bcf)
        {
            var snippet = bcf.GetTopics().First().GetBimSnippet(false);
            ASSERT(snippet != null);
            if (snippet != null)
            {
                ASSERT(snippet.IsExternal == true);
                ASSERT(snippet.SnippetType == "JSON");
                ASSERT(snippet.Reference == "JsonElement.json");
                ASSERT(snippet.ReferenceSchema == "http://json-schema.org");
            }
        }

        static private void Topics()
        {
            using (var bcf = new RDF.BCF.Project("MyProject"))
            {
                bool ok = bcf.SetOptions("Smoke-tester", true);
                ASSERT(ok);

                SetTopicAttributes(bcf);

                CheckTopicAttributes(bcf, true);

                ok = bcf.FileWrite("TopicsTest.bcf");
                ASSERT(ok);
            }

            using (var bcf = new RDF.BCF.Project())
            {
                var ok = bcf.FileRead("TopicsTest.bcf", false);
                ASSERT(ok);

                var i = CheckTopicAttributes(bcf, true);

                bcf.SetOptions("Smoke-Editor", true);

                bcf.GetTopics()[i].Title = "Modified title";

                CheckTopicAttributes(bcf, false);

                ok = bcf.FileWrite("TopicsTest2.bcf");
                ASSERT(ok);
            }

            using (var bcf = new RDF.BCF.Project())
            {
                var ok = bcf.FileRead("TopicsTest2.bcf", false);
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
            return $"aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa{i/10}{i%10}";
        }

        static private string TestIfcGuid(int i)
        {
            return $"{i}{i}aaaaaaaaaaaaaaaaaaaa";
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

                for (int j = 0; j < 12; j++)
                {
                    string filePath = TestFile((j % 3 == 0) ? "ifc" : "png");
                    bool isExternal = (j%5==0);

                    if (j % 2 == 0)
                    {
                        var reference = topic.AddDocumentRefernce(filePath, isExternal, TestGuid(j));
                        reference.Description = ($"Descr {j}");
                    }
                    else
                    {
                        var reference = topic.AddDocumentRefernce("ftp://ee");
                        reference.SetFilePath("");
                        ASSERT(reference.FilePath.Length==0);
                        reference.SetFilePath (filePath, isExternal);
                    }
                }
                ASSERT(topic.GetDocumentReferences().Count == 12);
                ASSERT(topic.GetDocumentReferences()[11].Remove());
                ASSERT(topic.GetDocumentReferences().Count == 11);

                ASSERT(topic.GetBimSnippet(false) == null);
                ASSERT(topic.GetBimSnippet(true) != null);
                var snippest = topic.GetBimSnippet(false);
                ASSERT(snippest != null);
                if (snippest != null)
                    ASSERT(snippest.Remove());
                ASSERT(topic.GetBimSnippet(false) == null);

                var snippet = topic.GetBimSnippet(true);
                ASSERT(snippet != null);
                if (snippet != null)
                {
                    snippet.IsExternal = true;
                    snippet.SnippetType = "NEW_TYPE";
                    snippet.Reference = "http://aaa";
                    snippet.ReferenceSchema = "http://bbb";
                }
            }

        }

        static private int CheckTopicAttributes(RDF.BCF.Project bcf, bool newFile)
        {
            ASSERT(bcf.GetTopics().Count() == 2);

            int i = 0;
            if (bcf.GetTopics()[i].Guid != TestGuid(0))
            {
                i = 1;
            }

            var topic = bcf.GetTopics()[i];
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
                ASSERT(topic.GetDocumentReferences().Count == 0);
            }

            topic = bcf.GetTopics()[(i+1)%2];
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

                ASSERT(topic.GetDocumentReferences().Count == 11);
                for (int j = 0; j < 11; j++)
                {
                    string filePath = TestFile((j % 3 == 0) ? "ifc" : "png");
                    bool isExternal = (j % 5 == 0);

                    var reference = topic.GetDocumentReferences()[j];
                    if (isExternal)
                    {
                        ASSERT(reference.FilePath == filePath);
                    }
                    else
                    {
                        ASSERT(reference.FilePath.Substring(reference.FilePath.Length-13)==filePath.Substring(filePath.Length-13));
                    }
                    if (j % 2 == 0)
                    {
                        ASSERT(reference.Guid ==     TestGuid(j));
                        ASSERT(reference.Description == ($"Descr {j}"));
                    }
                    else
                    {
                        ASSERT(reference.Guid.Length > 0);
                        ASSERT(reference.Description.Length == 0);
                    }
                }

                var snippet = topic.GetBimSnippet(false);
                ASSERT(snippet != null);
                if (snippet != null)
                {
                    ASSERT(snippet.IsExternal == true);
                    ASSERT(snippet.SnippetType == "NEW_TYPE");
                    ASSERT(snippet.Reference == "http://aaa");
                    ASSERT(snippet.ReferenceSchema == "http://bbb");
                }

            }

            return (i + 1) % 2;
        }

        static void CommentAndViewPoints()
        {
            using (var bcf = new RDF.BCF.Project("MyProject"))
            {
                bool ok = bcf.SetOptions("Smoke-tester", true);
                ASSERT(ok);

                SetCommentAndViewPoints(bcf);
                CheckCommentAndViewPoints(bcf, false, false);

                ok = bcf.FileWrite("TestCommentsVP.bcf");
                ASSERT(ok);
            }

            using (var bcf = new Project())
            {
                bool ok = bcf.FileRead("TestCommentsVP.bcf", false);
                ASSERT(ok);

                CheckCommentAndViewPoints(bcf, true, false);

                ok = bcf.SetOptions("Smoke-Editor", true);
                ASSERT(ok);

                bcf.GetTopics()[0].GetComments()[0].Text = "Modified text";

                CheckCommentAndViewPoints(bcf, true, true);

                ok = bcf.FileWrite("TestCommentsVP2.bcf");
                ASSERT(ok);
            }

            using (var bcf = new Project())
            {
                bool ok = bcf.FileRead("TestCommentsVP2.bcf", false);
                ASSERT(ok);

                CheckCommentAndViewPoints(bcf, true, true);
            }
        }
         
        static void SetCommentAndViewPoints(Project bcf)
        {
            var topic = bcf.AddTopic("Type", "Title", "New");
            ASSERT(topic.GetComments().Count == 0);

            SetFiles(topic);
            SetViewPoints(topic);

            var comment = topic.AddComment();

            ASSERT(topic.GetComments().Count == 1);

            var comment2 = topic.AddComment();
            ASSERT(topic.GetComments().Count == 2);
            comment2.Remove();
            ASSERT(topic.GetComments().Count == 1);


            comment.Text = "Text comment";
            comment.ViewPoint = topic.GetViewPoints()[0];

            //can't comment used viewpoint
            var cnt = topic.GetViewPoints().Count;
            var res = comment.ViewPoint.Remove();
            ASSERT(!res);
            var str = bcf.GetErrors();
            ASSERT(str.Length != 0);
            ASSERT(topic.GetViewPoints().Count == cnt);       
        }

        static void CheckCommentAndViewPoints(Project bcf, bool read, bool modified)
        {
            var topic = bcf.GetTopics()[0];

            CheckFiles(topic);
            CheckViewPoints(topic, read);

            var comment = topic.GetComments()[0];

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
            ASSERT(topic.GetFiles().Count == 0);


            for (int i = 0; i < 5; i++)
            {
                bool isExternal = (i % 2 == 0);
                var reference = TestFile("ifc");

                RDF.BCF.BimFile file;
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

            ASSERT(topic.GetFiles().Count == 5);

            topic.GetFiles()[4].Remove();
            ASSERT(topic.GetFiles().Count == 4);
        }

        static void CheckFiles(Topic topic)
        {
            ASSERT(topic.GetFiles().Count == 4);

            for (int i = 0; i < 4; i++)
            {
                bool isExternal = (i % 2 == 0);
                var reference = TestFile("ifc");

                var file = topic.GetFiles()[i];
                if (i > 1)
                {
                    ASSERT(file.Filename == $"Name-{i}");
                    ASSERT(file.Date == TestDate(i));
                }
                else
                {
                    ASSERT(file.Filename == "Architectural.ifc");
                    ASSERT(file.Date.Length > 0);
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
            ASSERT(topic.GetViewPoints().Count == 0);

            topic.AddViewPoint();
            ASSERT(topic.GetViewPoints()[0].Guid.Length > 0);
            ASSERT(topic.GetViewPoints()[0].Snapshot.Length == 0);

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

                for (int j = 0; j < 4; j++)
                {
                    var comp = vp.AddSelection(TestIfcGuid(i + j));
                    comp.OriginatingSystem = "test";
                    comp.AuthoringToolId = $"ID{i}";

                    comp = vp.AddException();
                    comp.IfcGuid = TestIfcGuid(j);

                    var color = vp.AddColoring((j % 2 == 0) ? $"00FFbb0{j}" : null);
                    if (j % 2 != 0)
                    {
                        color.Color = $"00FFbb0{j}";
                    }
                    for (int k = 0; k < 7; k++)
                    {
                        color.AddComponet(TestIfcGuid(k));
                    }
                    ASSERT(color.GetComponents().Count == 7);
                    color.GetComponents()[0].Remove();
                    ASSERT(color.GetComponents().Count == 6);

                    var pt0 = new Interop.BCFPoint(0, 0, 0);
                    var start = new Interop.BCFPoint(10 + j, 11 +j, 12 + j);
                    var end = new Interop.BCFPoint(13 + j, 14 + j, 15 + j);
                    var location = new Interop.BCFPoint(16 + j, 17 + j, 18 + j);
                    var direction = new Interop.BCFPoint(19 + j, 20 + j, 21 + j);

                    var line = vp.AddLine((j % 2 == 1) ? start : pt0, (j % 2 == 1) ? end : pt0);
                    var plane = vp.AddClippingPlane((j % 2 == 1) ? location : pt0, (j % 2 == 1) ? direction : pt0);

                    if (j % 2 != 1)
                    {
                        line.SetStartPoint(start);
                        line.SetEndPoint(end);
                        plane.SetLocation(location);
                        plane.SetDirection(direction);
                    }
                }

                ASSERT(vp.GetSelection().Count == 4);
                ASSERT(vp.GetExceptions().Count == 4);
                ASSERT(vp.GetColorings().Count == 4);
                ASSERT(vp.GetLines().Count == 4);
                ASSERT(vp.GetClippingPlanes().Count == 4);

                ASSERT(vp.GetSelection()[3].Remove());
                ASSERT(vp.GetExceptions()[3].Remove());
                ASSERT(vp.GetColorings()[3].Remove());
                ASSERT(vp.GetLines()[3].Remove());
                ASSERT(vp.GetClippingPlanes()[3].Remove());

                ASSERT(vp.GetSelection().Count == 3);
                ASSERT(vp.GetExceptions().Count == 3);
                ASSERT(vp.GetColorings().Count == 3);
                ASSERT(vp.GetLines().Count == 3);
                ASSERT(vp.GetClippingPlanes().Count() == 3);

                for (int j = 1; j < 5; j++)
                {
                    var bmp = vp.AddBitmap((i % 2 == 0) ? TestFile("png") : "",
                        (i % 2 == 0) ? Interop.BCFBitmapFormat.BCFBitmapPNG : Interop.BCFBitmapFormat.BCFBitmapJPG,
                        new Interop.BCFPoint(i*j, i + .1, i + .2),
                        new Interop.BCFPoint(i*j + .3, i + .4, i + .5),
                        new Interop.BCFPoint(i*j + .6, i + .7, i + .8),
                        i);

                    if (i % 2 != 0)
                    {
                        bmp.Reference = TestFile("png");
                    }

                    if (i % 3 == 0)
                    {
                        bmp.SetLocation(new Interop.BCFPoint(10 * i, i + .1, i + .2));
                        bmp.SetNormal(new Interop.BCFPoint(10 * i + .3, i + .4, i + .5));
                        bmp.SetUp(new Interop.BCFPoint(10 * i + .6, i + .7, i + .8));
                        bmp.Height = 1.2 * j;
                    }
                }
                ASSERT(vp.GetBitmaps().Count == 4);
                vp.GetBitmaps()[3].Remove();
                ASSERT(vp.GetBitmaps().Count == 3);
            }

            ASSERT(topic.GetViewPoints().Count == 5);

            ASSERT(topic.GetViewPoints()[0].Remove());
            ASSERT(topic.GetViewPoints().Count == 4);

        }

        static bool EQ(Interop.BCFPoint pt1, Interop.BCFPoint pt2)
        {
            return pt1.X == pt2.X && pt1.Y == pt2.Y && pt1.Z == pt2.Z;
        }

        static void CheckViewPoints(Topic topic, bool read)
        {
            ASSERT(topic.GetViewPoints().Count == 4);

            for (int i = 0; i < 4; i++)
            {
                var vp = topic.GetViewPoints()[i];

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

                var selection = vp.GetSelection();
                ASSERT(selection.Count == 3);

                var expt = vp.GetExceptions();
                ASSERT(expt.Count == 3);

                var clr = vp.GetColorings();
                ASSERT(clr.Count == 3);

                var lines = vp.GetLines();
                ASSERT(lines.Count == 3);

                var planes = vp.GetClippingPlanes();
                ASSERT(planes.Count == 3);

                for (int j = 0; j < 3; j++)
                {
                    var comp = selection[j];
                    ASSERT(comp.IfcGuid == TestIfcGuid(i + j));
                    ASSERT(comp.OriginatingSystem == "test");
                    ASSERT(comp.AuthoringToolId == $"ID{i}");

                    comp = expt[j];
                    ASSERT(comp.IfcGuid == TestIfcGuid(j));
                    ASSERT(comp.OriginatingSystem == "");
                    ASSERT(comp.AuthoringToolId == "");

                    ASSERT(clr[j].Color== $"00FFbb0{j}");
                    ASSERT(clr[j].GetComponents().Count == 6);
                    for (int k = 0; k < 6; k++)
                    {
                        ASSERT(clr[j].GetComponents()[k].IfcGuid == TestIfcGuid(k+1));
                    }

                    var start = new Interop.BCFPoint(10 + j, 11 + j, 12 + j);
                    var end = new Interop.BCFPoint(13 + j, 14 + j, 15 + j);
                    var location = new Interop.BCFPoint(16 + j, 17 + j, 18 + j);
                    var direction = new Interop.BCFPoint(19 + j, 20 + j, 21 + j);
                    
                    ASSERT(EQ(lines[j].GetStartPoint(), start));
                    ASSERT(EQ(lines[j].GetEndPoint(), end));
                    ASSERT(EQ(planes[j].GetLocation(), location));
                    ASSERT(EQ(planes[j].GetDirection(), direction));
                }

                ASSERT(vp.GetBitmaps().Count == 3);
                for (int j = 1; j < 4; j++)
                {
                    var bmp = vp.GetBitmaps()[j - 1];
                 
                    ASSERT(bmp.Reference.EndsWith ("Architectural.png"));
                    ASSERT(bmp.Format == ((i % 2 == 0) ? Interop.BCFBitmapFormat.BCFBitmapPNG : Interop.BCFBitmapFormat.BCFBitmapJPG));

                    if (i % 3 == 0)
                    {
                        ASSERT(EQ(bmp.GetLocation(), new Interop.BCFPoint(10 * i, i + .1, i + .2)));
                        ASSERT(EQ(bmp.GetNormal(), new Interop.BCFPoint(10 * i + .3, i + .4, i + .5)));
                        ASSERT(EQ(bmp.GetUp(), new Interop.BCFPoint(10 * i + .6, i + .7, i + .8)));
                        var diff = bmp.Height - 1.2 * j;
                        ASSERT(Math.Abs(diff)<1e-8);
                    }
                    else
                    {
                        ASSERT(EQ(bmp.GetLocation(), new Interop.BCFPoint(i * j, i + .1, i + .2)));
                        ASSERT(EQ(bmp.GetNormal(), new Interop.BCFPoint(i * j + .3, i + .4, i + .5)));
                        ASSERT(EQ(bmp.GetUp(), new Interop.BCFPoint(i * j + .6, i + .7, i + .8)));
                        ASSERT(bmp.Height == i);

                    }
                }
            }
        }

        static void Validations()
        {
            using (var bcf = new Project("MyTset"))
            {
                bcf.SetOptions("me", true);

                var topic = bcf.AddTopic("", "B", "C");

                ASSERT(bcf.GetErrors().Length == 0);
                var ok = bcf.FileWrite("Validation.bcf");
                ASSERT(!ok);
                var err = bcf.GetErrors();
                ASSERT(err.Contains("Missed property"));
                ASSERT(err.Contains("TopicType"));

                topic.TopicType = "Type";
                ok = bcf.FileWrite("Validation.bcf");
                ASSERT(ok);

                //
                var file = topic.AddFile(null);

                ASSERT(bcf.GetErrors().Length == 0);

                file.Reference = "WrongPath";
                ASSERT(file.Reference.Length == 0);
                ASSERT(bcf.GetErrors().Length != 0);

                file.Date = "WrongDate";
                ASSERT(file.Date.Length == 0);
                ASSERT(bcf.GetErrors().Length != 0);

                file.IfcSpatialStructureElement = "wrongGUID";
                ASSERT(file.IfcSpatialStructureElement.Length == 0);
                ASSERT(bcf.GetErrors().Length != 0);

                //
                var comment = topic.AddComment();
                ASSERT(bcf.GetErrors().Length == 0);

                ok = bcf.FileWrite("Validation.bcf");
                ASSERT(!ok);
                err = bcf.GetErrors();
                ASSERT(err.Contains("Missed property"));
                ASSERT(err.Contains("Comment"));

                comment.Text = "Text";
                ok = bcf.FileWrite("Validation.bcf");
                ASSERT(ok);

                //component
                var viewPoint = topic.AddViewPoint();

                ok = bcf.FileWrite("Validation.bcf");
                err = bcf.GetErrors();
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

                //
                var comp = viewPoint.AddSelection();
                comp.IfcGuid = "wrong";
                ASSERT(comp.IfcGuid.Length == 0);
                ok = bcf.FileWrite("Validation.bcf");
                err = bcf.GetErrors();
                ASSERT(!ok);
                ASSERT(err.Contains("Invalid value"));
                ASSERT(err.Contains("Missed property"));
                ASSERT(err.Contains("IfcGuid"));

                comp.IfcGuid = TestIfcGuid(0);
                ok = bcf.FileWrite("Validation.bcf");
                err = bcf.GetErrors();
                ASSERT(ok);

                var bmp = viewPoint.AddBitmap("", Interop.BCFBitmapFormat.BCFBitmapPNG, new Interop.BCFPoint(), new Interop.BCFPoint(), new Interop.BCFPoint(), 0);
                ok = bcf.FileWrite("Validation.bcf");
                ASSERT(!ok);
                err = bcf.GetErrors();
                ASSERT(err.Contains("Missed property"));
                ASSERT(err.Contains("Reference"));

                bmp.Remove();
                ok = bcf.FileWrite("Validation.bcf");
                ASSERT(ok);

                //
                var clr = viewPoint.AddColoring("");
                clr.Color = "lalala";
                ASSERT(clr.Color.Length == 0);
                err = bcf.GetErrors();
                ASSERT(err.Contains("Invalid value"));
                ASSERT(err.Contains("Color"));
                clr.Color = "AABBCC";
                ASSERT(clr.Color == "AABBCC");

                ok = bcf.FileWrite("Validation.bcf");
                ASSERT(!ok);
                err = bcf.GetErrors();
                ASSERT(err.Contains("Missed property"));
                ASSERT(err.Contains("Components"));

                clr.AddComponet(TestIfcGuid(0));
                ok = bcf.FileWrite("Validation.bcf");
                ASSERT(ok);

                //
                var docref = topic.AddDocumentRefernce("");
                ok = bcf.FileWrite("Validation.bcf");
                err = bcf.GetErrors();
                ASSERT(!ok);
                ASSERT(err.Contains("Missed property"));
                ASSERT(err.Contains("Url"));

                docref.SetFilePath ("http://ss");
                ok = bcf.FileWrite("Validation.bcf");
                ASSERT(ok);
            }
        }

        static void TestTopicListAttributes()
        {
            using (var project = new Project("TestListAttr"))
            {
                project.SetOptions("creator", true);

                for (int i = 0; i < 7; i++)
                {
                    project.AddTopic($"Topic Type {i}", $"Topic Title {i}", $"Topic Status {i}", TestGuid(i));
                }

                CreateTopicLists(project, 1, 3);
                CheckTopicLists(project, 1, 3);

                CreateTopicLists(project, 3, 7);
                CheckTopicLists(project, 3, 7);

                var ok = project.FileWrite("TestListAttr.bcf");
                ASSERT(ok);
            }

            using (var project = new Project("ddd"))
            {
                var ok = project.FileRead("TestListAttr.bcf", false);
                ASSERT(ok);

                CheckTopicLists(project, 3, 7);
            }
        }

        static void CreateTopicLists(Project project, int A, int B)
        {
            var topic = project.GetTopics().First();

            var labels = new List<string>();
            var links = new List<string>();
            var refs = new List<Topic>();
            for (int i = A; i < B; i++)
            {
                labels.Add($"Label {i}");
                links.Add($"http://{i}");
                refs.Add(project.GetTopics()[i]);
            }

            topic.SetLabels(labels);
            topic.SetReferenceLinks(links);
            topic.SetRelatedTopics(refs);
        }

        static void CheckTopicLists(Project project, int A, int B)
        {
            var topic = project.GetTopics().First();

            var labels = topic.GetLabels();
            ASSERT(labels.Count == B-A);

            var links = topic.GetReferenceLinks();
            ASSERT(links.Count == B-A);

            var refs = topic.GetRelatedTopics();
            ASSERT(refs.Count == B-A);

            for (int i = A; i < B; i++)
            {
                ASSERT(labels[i-A]==$"Label {i}");
                ASSERT(links[i-A] == $"http://{i}");
                ASSERT(refs[i-A].Guid == project.GetTopics()[i].Guid);
            }

            labels = project.Extensions.GetElements(Interop.BCFEnumeration.TopicLabels);
            ASSERT(labels.Count == B-1);
            for (int i = 1; i < B; i++)
            {
                ASSERT(labels[i - 1] == $"Label {i}");
            }
        }
    }
}
