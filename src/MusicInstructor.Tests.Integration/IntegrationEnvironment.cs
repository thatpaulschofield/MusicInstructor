using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using StructureMap.Configuration.DSL;
using StructureMap.Configuration.DSL.Expressions;

namespace MusicInstructor.Tests.Integration
{
    public class IntegrationEnvironment : Registry
    {
        public IntegrationEnvironment()
        {
            Profile("Integration", c =>
                                       {
                                           var store = new EmbeddableDocumentStore
                                           {
                                               RunInMemory = true
                                           }.Initialize();

                                           Raven.Client.Indexes.IndexCreation.CreateIndexes(System.Reflection.Assembly.GetCallingAssembly(), store);
                                           c.For<IDocumentSession>().Use(x => store.OpenSession());
                                               
                                       });
        }
    }
}