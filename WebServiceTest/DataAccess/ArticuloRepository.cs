using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServiceTest.Models;

namespace WebServiceTest.DataAccess
{
    public class ArticuloRepository
    {
        public Articulo BuscarPorCodigo(string codigo)
        {
            Articulo articulo = new Articulo();
            using (ApplicationContext db = new ApplicationContext())
            {
                string query = string.Format("SELECT T0.\"ItemCode\", T0.\"ItmsGrpCod\", T0.\"ItemName\", T0.\"OnHand\"," +
                    " T0.\"U_Frecuencia\", T0.\"U_CantidaFrecuencia\" " +
                    " FROM OITM T0  WHERE \"ItemCode\" = '{0}' ", codigo);

                Recordset recordset = db.SBOCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery(query);

                if (recordset.RecordCount == 0) { throw new Exception("No se encontro el registro"); }

                articulo.Codigo = recordset.Fields.Item("ItemCode").Value;
                articulo.Nombre = recordset.Fields.Item("ItemName").Value;
                articulo.Disponible = recordset.Fields.Item("OnHand").Value;
                articulo.CodigoGrupo = recordset.Fields.Item("ItmsGrpCod").Value;
                articulo.CantidadFrecuencia = recordset.Fields.Item("U_CantidaFrecuencia").Value;
                articulo.Frecuencia = recordset.Fields.Item("U_Frecuencia").Value;
                articulo.Almacenes = new AlmacenRepository().EncontraPorCodigo(articulo.Codigo);
            }

            return articulo;
        }

        public List<Articulo> Lista(string codigoGrupo)
        {
            List<Articulo> articulos = new List<Articulo>();
            using (ApplicationContext db = new ApplicationContext())
            {
                string query = string.Format("SELECT TOP 10 T0.\"ItemCode\", T0.\"ItmsGrpCod\", T0.\"ItemName\", T0.\"OnHand\" " +
                   " FROM OITM T0 WHERE \"ItmsGrpCod\" = '{0}' ", codigoGrupo);

                Recordset recordset = db.SBOCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery(query);

                int filas = recordset.RecordCount;
                if (recordset.RecordCount == 0) { throw new Exception("No se encontro ningun registro"); }

                while (!recordset.EoF)
                {
                    Articulo articulo = new Articulo()
                    {
                        Codigo = recordset.Fields.Item("ItemCode").Value,
                        Nombre = recordset.Fields.Item("ItemName").Value,
                        Disponible = recordset.Fields.Item("OnHand").Value,
                        CodigoGrupo = recordset.Fields.Item("ItmsGrpCod").Value
                    };

                    articulo.Almacenes = new AlmacenRepository().EncontraPorCodigo(articulo.Codigo);
                    articulos.Add(articulo);
                    recordset.MoveNext();
                }
            }

            return articulos;
        }
    }
}