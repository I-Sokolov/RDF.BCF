
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

                topic.Description = "This topic is made to demonstate how to create BCF";
                topic.AddFile("..\\TestCases\\Architectural.ifc");

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
                viewpoint.FieldOfView = 90;
                viewpoint.AspectRatio = 1;
                //hide all except one element
                viewpoint.DefaultVisibility = false;   
                viewpoint.AddException("15LX1o$dj1O8G53cOqE8W$");

                comment.ViewPoint = viewpoint;

                //
                //
                bcfData.FileWrite("MyTest.bcf");

                var errors = bcfData.GetErrors();
                if(errors.Length != 0)
                {
                    Console.WriteLine("There were errors: " + errors);
                }
            }
        }


        /// <summary>
        /// Example of BCF file reading and printing all topics and comments
        /// </summary>
        static void ReadExample()
        {
            using (var bcfData = new RDF.BCF.Project())
            {
                if (!bcfData.FileRead("MyTest.bcf", false)) {
                    Console.WriteLine($"Failed to read BCF file: {bcfData.GetErrors()}");
                    return;
                }

                foreach (var topic in bcfData.GetTopics())
                {
                    Console.WriteLine($"Topic '{topic.Title}', type: {topic.TopicType}, status: {topic.TopicStatus}");
                    Console.WriteLine($"By {topic.CreationAuthor} {topic.CreationDate} {topic.ModifiedAuthor} {topic.ModifiedDate}");
                    Console.WriteLine($"{topic.Description}");

                    foreach (var comment in topic.GetComments())
                    {
                        Console.WriteLine($"  Comment by {comment.Author} {comment.Date}: {comment.Text}");
                    }
                }
            }
        }
    }
}
