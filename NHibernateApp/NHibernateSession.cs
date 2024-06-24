using System.Reflection;
using System.Web;
using NHibernate;
using NHibernate.Cfg;

namespace NHibernateApp
{
    public class NHibernateSession
    {
        public static NHibernate.ISession OpenSession()
        {

            string rootPath = Directory.GetCurrentDirectory();
            var configuration = new Configuration();
            var configurationPath = Path.Combine(rootPath, "Models\\Nhibernate\\nhibernate.configuration.xml");
            configuration.Configure(configurationPath);
            var orderConfigurationFile = Path.Combine(rootPath, "Models\\Nhibernate\\Order.mapping.xml");
            configuration.AddFile(orderConfigurationFile);
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}
