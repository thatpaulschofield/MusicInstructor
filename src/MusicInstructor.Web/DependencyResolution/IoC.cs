using System.Web.Mvc;
using StructureMap;

namespace MusicInstructor.Web {
    public static class IoC {

        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
                        {
                            x.AddRegistry<DefaultRegistry>();
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                        scan.AddAllTypesOf<Controller>();
                                    });

                        });
            return ObjectFactory.Container;
        }


    }
}