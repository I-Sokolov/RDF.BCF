
using System.Diagnostics;
using System.Xml.Linq;

namespace CSExample
{
    internal class Example
    {
        static void Main(string[] args)
        {
            CreateExample();
            ReadExample();

            SmokeTest.Run();
        }

        /// <summary>
        /// Example of BCF file creation
        /// </summary>
        static void CreateExample()
        {
            using (var bcfData = new RDF.BCF.Project("MyProject"))
            {
                bcfData.SetOptions("user@company.org", true);

                //
                // create topic
                //
                var topic = bcfData.AddTopic("Example", "The example of a topic", "New");
                if (topic == null)
                {
                    Console.WriteLine(bcfData.GetErrors());
                    return;
                }

                topic.Description = "This topic is made to demonstrate how to create BCF";
                topic.AddFile("..\\TestCases\\Architectural.ifc");

                topic.AddDocumentRefernce("https://example.com/spec.pdf");


                //
                // create comment
                //
                var comment = topic.AddComment();
                comment.Text = "Look here";

                //
                // create viewpoint and set for comment
                //
                var viewpoint = topic.AddViewPoint();
                //minimal viewpoint settings
                viewpoint.SetCameraViewPoint(new RDF.BCF.Interop.BCFPoint());
                viewpoint.SetCameraDirection(new RDF.BCF.Interop.BCFPoint(1));
                viewpoint.SetCameraUpVector(new RDF.BCF.Interop.BCFPoint(0, 0, 1));
                viewpoint.FieldOfView = 60;
                viewpoint.AspectRatio = 1;
                //hide all except one element
                viewpoint.DefaultVisibility = false;   
                viewpoint.AddException("15LX1o$dj1O8G53cOqE8W$");

                comment.ViewPoint = viewpoint;

                //
                //
                for (var version = RDF.BCF.Interop.Version._2_1; version <= RDF.BCF.Interop.Version._3_0; version++)
                {
                    bcfData.FileWrite("MyTest.bcf", version);

                    var errors = bcfData.GetErrors();
                    if (errors.Length != 0)
                    {
                        Console.WriteLine("There were errors: " + errors);
                    }
                }
            }
        }


        /// <summary>
        /// Example of BCF file reading and printing all topics and comments
        /// </summary>
        static void ReadExample()
        {
            string bcfFilePath = "MyTest.bcf";
            using (var bcfData = new RDF.BCF.Project())
            {
                Console.WriteLine($"Reading BCF file: {bcfFilePath}");

                if (!bcfData.FileRead(bcfFilePath, false))
                {
                    Console.WriteLine($"Failed to read BCF file: {bcfData.GetErrors()}");
                    return;
                }

                Console.WriteLine($"Read - project name '{bcfData.Name}', id: {bcfData.ProjectId}");

                Console.WriteLine($"Topics count: {bcfData.GetTopics().Count}");
                foreach (var topic in bcfData.GetTopics())
                {
                    Console.WriteLine($"Topic '{topic.Title}', type: {topic.TopicType}, status: {topic.TopicStatus}");
                    Console.WriteLine($"By {topic.CreationAuthor} {topic.CreationDate} {topic.ModifiedAuthor} {topic.ModifiedDate}");
                    Console.WriteLine($"{topic.Description}");

                    Console.WriteLine($"Comments count: {topic.GetComments().Count}");
                    foreach (var comment in topic.GetComments())
                    {
                        Console.WriteLine($"  Comment by {comment.Author} {comment.Date}: {comment.Text}");
                    }

                    Console.WriteLine($"Document count: {topic.GetDocumentReferences().Count}");
                    foreach (var docRef in topic.GetDocumentReferences())
                    {
                        Console.WriteLine($"  Document GUID: {docRef.Guid}");
                        Console.WriteLine($"       external: {docRef.IsExternal}, description: {docRef.Description}");
                        Console.WriteLine($"       path: {docRef.FilePath}");
                    }
                }
            }
        }
    }
}
