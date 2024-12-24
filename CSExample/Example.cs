
using System.Xml.Linq;

namespace CSExample
{
    internal class Example
    {
        static void Main(string[] args)
        {
            SmokeTest.Run();

            CreateExample();
            ReadExample();
        }

        static void CreateExample()
        {
            using (var bcfData = new RDF.BCF.Project("user@company.org", true))
            {
                //
                // init and set options
                //
                bcfData.InitNew();

                //
                // create topic
                //
                var topic = bcfData.Topics.Add(title: "Topic1", topicType: "Test", topicStatus: "New");

                topic.Description = "The wall need plaster layer";

                topic.BIMFiles.Add("c:\\my.ifc");

                //
                // create comment with viewpoint
                //
                var comment = topic.Comments.Add("Look here");

                var viewpoint = CreateViewpointExample(topic);

                comment.Viewpoint = viewpoint;

                //
                //
                bcfData.WriteFile("MyTest.bcf");
            }
        }


        static RDF.BCF.Viewpoint CreateViewpointExample(RDF.BCF.Topic topic)
        {
            var viewpoint = topic.Viewpoints.Add();

            //hide all except one element
            viewpoint.DefaultVisibility = false;
            viewpoint.Exceptions = new string[] { "15LX1o$dj1O8G53cOqE8W$" };

            return viewpoint;
        }

        static void ReadExample()
        {
            using (var bcfData = new RDF.BCF.Project())
            {
                bcfData.ReadFile("MyTest.bcf");

                WorkWithSchemaExtensionsExample(bcfData);

                foreach (var topic in bcfData.Topics.Items)
                {
                    //
                    Console.WriteLine($"Topic {topic.Title} {topic.TopicType} {topic.TopicStatus}");
                    if (topic.Description != null)
                    {
                        Console.WriteLine(topic.Description);
                    }

                    //
                    Console.WriteLine("BIM file paths to load");
                    foreach (var bim in topic.BIMFiles.Items)
                    {
                        Console.WriteLine(bim.Reference);
                    }

                    //
                    foreach (var comment in topic.Comments.Items)
                    {
                        Console.WriteLine(comment.Text ?? "-no message-");
                        var vp = comment.Viewpoint;
                        if (vp != null)
                        {
                            if (vp.DefaultVisibility)
                                Console.WriteLine("Show all except: ");
                            else
                                Console.WriteLine("Show nothing except: ");
                            foreach (var id in vp.Exceptions) 
                            { 
                                Console.WriteLine($"   {id}");
                            }
                        }
                    }

                }
            }
        }


        static void WorkWithSchemaExtensionsExample(RDF.BCF.Project project)
        {
            RDF.BCF.Extensions schema = project.Extensions;

            //registered
            IEnumerable<string> users = schema.GetEnumeration(RDF.BCF.Native.BCFEnumeration.Users);
            Console.Write("Users: ");
            foreach (string user in users) 
            { 
                Console.Write($"{user}, ");
            }
            Console.WriteLine();

            //
            schema.AddEnumerationElement(RDF.BCF.Native.BCFEnumeration.TopicStatuses, "Assigned");
        }
    }
}
