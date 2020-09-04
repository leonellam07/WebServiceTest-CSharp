using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceTest
{
    public class ApplicationContext : IDisposable
    {
        public Company SBOCompany = null;
        private static string DistributationSQL { get; set; }
        private static string Server { get; set; }
        private static string CompanyDB { get; set; }
        private static string UserName { get; set; }
        private static string Password { get; set; }
        private static string UseTrusted { get; set; }
        private static string LicenseServer { get; set; }

        public ApplicationContext()
        {
            Open();
        }

        private void Open()
        {
            if(SBOCompany != null) { if (SBOCompany.Connected) { return; } }

            DistributationSQL = System.Configuration.ConfigurationManager.AppSettings["DistributationSQL"];
            Server = System.Configuration.ConfigurationManager.AppSettings["Server"];
            CompanyDB = System.Configuration.ConfigurationManager.AppSettings["Database"];
            UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
            Password = System.Configuration.ConfigurationManager.AppSettings["Password"];
            LicenseServer = System.Configuration.ConfigurationManager.AppSettings["LicenseServer"];
            UseTrusted = System.Configuration.ConfigurationManager.AppSettings["UseTrusted"];

            //Validaciones de Parametros
            if (string.IsNullOrEmpty(DistributationSQL)) { throw new Exception("Se necesita distribucion de SQL"); }
            if (string.IsNullOrEmpty(Server)) { throw new Exception("Se necesita servidor"); }
            if (string.IsNullOrEmpty(CompanyDB)) { throw new Exception("Se necesita Base de datos"); }
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password)) { throw new Exception("Se necesita usuario/password"); }

            bool trusted = false;
            Boolean.TryParse(UseTrusted, out trusted);

            //Parametrizacion de Conexion para el Company
            SBOCompany = new Company
            {
                LicenseServer = (!string.IsNullOrEmpty(LicenseServer)) ? LicenseServer : null,
                Server = Server,
                UserName = UserName,
                Password = Password,
                CompanyDB = CompanyDB,
                UseTrusted = trusted
            };

            //Verificar que tipo de Base de Datos
            switch (DistributationSQL)
            {
                case "HANA": SBOCompany.DbServerType = BoDataServerTypes.dst_HANADB; break;
                case "MSSQL2014": SBOCompany.DbServerType = BoDataServerTypes.dst_MSSQL2014; break;
            }

            if(SBOCompany.Connect() != 0) { throw new Exception(SBOCompany.GetLastErrorDescription()); }
        }

        public void Dispose()
        {
            DisconectService();
            GC.SuppressFinalize(this);
        }

        private void DisconectService()
        {
            if(SBOCompany != null)
            {
                if (SBOCompany.Connected)
                {
                    SBOCompany.Disconnect();
                }
                else
                {
                    return;
                }
                SBOCompany = null;
            }
        }
    } 
}