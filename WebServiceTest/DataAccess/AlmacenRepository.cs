using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServiceTest.Models;

namespace WebServiceTest.DataAccess
{
    public class AlmacenRepository
    {
        public List<Almacen> EncontraPorCodigo(string codigo)
        {
            List<Almacen> almacenes = new List<Almacen>();
            using (ApplicationContext db = new ApplicationContext())
            {
                string query = string.Format("SELECT TOP 10 T0.\"ItemCode\", T0.\"WhsCode\", T0.\"OnHand\" " +
                    "                           FROM OITW T0 WHERE T0.\"ItemCode\" = '{0}' ", codigo);

                Recordset recordset = db.SBOCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery(query);


                while (!recordset.EoF)
                {
                    Almacen almacen = new Almacen()
                    {
                        Codigo = recordset.Fields.Item("ItemCode").Value,
                        Nombre = recordset.Fields.Item("WhsCode").Value,
                        Disponible = recordset.Fields.Item("OnHand").Value,
                    };
                    almacenes.Add(almacen);
                    recordset.MoveNext();
                }
            }

            return almacenes;
        }
    }
}