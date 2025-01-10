
using System.Diagnostics;
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
            using (var bcfData = new RDF.BCF.Project("MyProject"))
            {
                bcfData.SetAuthor("user@company.org", true);

                //
                // create topic
                //
                var topic = bcfData.AddTopic("Topic Type", "Topic Title", "Topic Status");
                if (topic == null)
                {
                    Console.WriteLine(bcfData.ErrorsGet());
                    return;
                }

                topic.Description = "The wall need plaster layer";

                //.BIMFiles.Add("c:\\my.ifc");

                //
                // create comment with viewpoint
                //
                var comment = topic.AddComment();
                comment.Text = "Look here";

                //var viewpoint = CreateViewpointExample(topic);

                //comment.Viewpoint = viewpoint;

                //
                //
                bcfData.FileWrite("MyTest.bcf");

                var errors = bcfData.ErrorsGet();
                if(errors.Length != 0)
                {
                    Console.WriteLine("Errors: " + errors);
                }
            }
        }

        /*
        static RDF.BCF.Viewpoint CreateViewpointExample(RDF.BCF.Topic topic)
        {
            var viewpoint = topic.Viewpoints.Add();

            //hide all except one element
            viewpoint.DefaultVisibility = false;
            viewpoint.Exceptions = new string[] { "15LX1o$dj1O8G53cOqE8W$" };

            return viewpoint;
        }*/

        static void ReadExample()
        {
            using (var bcfData = new RDF.BCF.Project())
            {
                if (!bcfData.FileRead("MyTest.bcf")) {
                    Console.WriteLine("READ BCF ERRORS:");
                    Console.WriteLine(bcfData.ErrorsGet());
                    return;
                }

                WorkWithSchemaExtensionsExample(bcfData);

                foreach (var topic in bcfData.Topics)
                {
                    //
                    Console.WriteLine($"Topic {topic.Title} {topic.TopicType} {topic.TopicStatus}");
                    if (topic.Description != null)
                    {
                        Console.WriteLine(topic.Description);
                    }

                    //
                    foreach (var comment in topic.Comments)
                    {
                        Console.WriteLine(comment.Text);
                        /*var vp = comment.Viewpoint;
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
                        }*/
                    }

                    //
                    /*
                    Console.WriteLine("BIM file paths to load");
                    foreach (var bim in topic.BIMFiles.Items)
                    {
                        Console.WriteLine(bim.Reference);
                    }*/

                }
            }
        }


        static void WorkWithSchemaExtensionsExample(RDF.BCF.Project project)
        {
            RDF.BCF.Extensions schema = project.Extensions;

            //registered
            IEnumerable<string> users = schema.GetEnumeration(RDF.BCF.Interop.BCFEnumeration.Users);
            Console.Write("Users: ");
            foreach (string user in users) 
            { 
                Console.Write($"{user}, ");
            }
            Console.WriteLine();

            //
            schema.EnumerationElementAdd(RDF.BCF.Interop.BCFEnumeration.TopicStatuses, "Assigned");
        }
    }
}
