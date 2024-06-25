using System.Reflection;
using System.Web;
using NHibernate;
using NHibernate.Cfg;

namespace NHibernateApp
{
    public class NHibernateSession
    {
        private static ISessionFactory _sessionFactory;

        public static NHibernate.ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    string rootPath = Directory.GetCurrentDirectory();
                    var configuration = new Configuration();
                    var configurationPath = Path.Combine(rootPath, "Models\\Nhibernate\\nhibernate.configuration.xml");
                    configuration.Configure(configurationPath);
                    var orderConfigurationFile = Path.Combine(rootPath, "Models\\Nhibernate\\Order.mapping.xml");
                    configuration.AddFile(orderConfigurationFile);
                    var orderItemConfigurationFile = Path.Combine(rootPath, "Models\\Nhibernate\\OrderItem.mapping.xml");
                    configuration.AddFile(orderItemConfigurationFile);
                    _sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }
    }
}
