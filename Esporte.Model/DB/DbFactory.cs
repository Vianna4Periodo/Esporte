using MySql.Data.MySqlClient;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Context;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Esporte.Model.DB
{
    public class DbFactory
    {
        private static DbFactory _instance = null;
        private ISessionFactory _sessionFactory;
        //public RepositoryPessoa RepositoryPessoa { get; set; }
        private DbFactory()
        {
            Connection();

            //RepositoryPessoa = new RepositoryPessoa(Session);
        }

        public static DbFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DbFactory();
                }

                return _instance;
            }
        }

        private void Connection()
        {
            try
            {
                var server = "localhost";
                var port = "3306";
                var dbName = "db_agenda";
                var user = "root";
                var psw = "root";
                var stringConnection = "Persist Security Info=False;" +
                                       "server =" + server +
                                       ";port=" + port +
                                       ";database=" + dbName +
                                       ";uid=" + user +
                                       ";pwd=" + psw;

                try
                {
                    var mysql = new MySqlConnection(stringConnection);
                    mysql.Open();

                    if (mysql.State == ConnectionState.Open)
                    {
                        mysql.Close();
                    }
                }
                catch
                {
                    CreateSchema(server, port, dbName, psw, user);
                }
                ConfigureNHibernate(stringConnection);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot connect to database. Error: ", ex);
            }
        }

        private void CreateSchema(string server, string port, string dbName, string psw, string user)
        {
            try
            {
                var stringConnection = "server =" + server +
                                       ";user=" + user +
                                       ";port=" + port +
                                       ";pwd=" + psw + ";";
                var mysql = new MySqlConnection(stringConnection);
                var cmd = mysql.CreateCommand();
                mysql.Open();
                cmd.CommandText = "CREATE DATABASE IF NOT EXISTS `" + dbName + "`;";
                cmd.ExecuteNonQuery();
                mysql.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot create database Schema. Error: ", ex);
            }
        }

        private void ConfigureNHibernate(string stringConnection)
        {
            try
            {
                //using NHibernate.cfg
                var config = new Configuration();

                //Configuracao do NHibernate com o MySQL
                config.DataBaseIntegration(i =>
                {
                    //Dialeto do banco
                    i.Dialect<NHibernate.Dialect.MySQLDialect>();
                    //Conexao string
                    i.ConnectionString = stringConnection;
                    //Drive de conexao com o banco
                    i.Driver<NHibernate.Driver.MySqlDataDriver>();
                    //Provedor de conexao do MySQL
                    i.ConnectionProvider<NHibernate.Connection.DriverConnectionProvider>();
                    //Gera log dos sql executados no console
                    i.LogSqlInConsole = true;
                    //descomentar caso queira visualizar o log de sql formatado no console
                    i.LogFormattedSql = true;
                    //cria schema do banco de dados sempre que a configuration for utilizada
                    i.SchemaAction = SchemaAutoAction.Update;
                });

                //Realiza o mapeamento das classes
                var maps = this.Mapping();
                config.AddMapping(maps);

                //Verifica se a aplicação é Desktop ou web
                if (HttpContext.Current == null)
                {
                    config.CurrentSessionContext<ThreadStaticSessionContext>();
                }
                else
                {
                    config.CurrentSessionContext<WebSessionContext>();
                }

                this._sessionFactory = config.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot configure NHibernate. Error: ", ex);
            }
        }

        private HbmMapping Mapping()
        {
            try
            {
                var mapper = new ModelMapper();

                //mapper.AddMapping(PessoaMap);

                //este método é suficiente para mapear TODAS as classes de mapeamento, pois o método getAssembly 
                //retorna todos as classes herdadas de ClassMapping, basta colocar uma classe qualquer como parametro
                //do método typeof()

                //mapper.AddMappings(
                //    Assembly.GetAssembly(typeof(PessoaMap)).GetTypes()
                //);
                return mapper.CompileMappingForAllExplicitlyAddedEntities();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public ISession Session
        {
            get
            {
                try
                {
                    if (CurrentSessionContext.HasBind(_sessionFactory))
                        return _sessionFactory.GetCurrentSession();

                    var session = _sessionFactory.OpenSession();
                    session.FlushMode = FlushMode.Commit;

                    CurrentSessionContext.Bind(session);

                    return session;
                }
                catch (Exception ex)
                {
                    throw new Exception("Cannot create session.", ex);
                }
            }
        }
    }
}
