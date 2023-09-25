using Infraestructure.Models;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Infraestructure.Repository
{
    public class RepositoryRestaurante : IRepositoryRestaurante
    {
        public RESTAURANTE GetRestauranteById(int idRestaurante)
        {
            RESTAURANTE oRestaurante = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oRestaurante = ctx.RESTAURANTE.Include("PRODUCTO").Where(r => r.ID_RESTAURANTE == idRestaurante).FirstOrDefault();
                }
                return oRestaurante;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener el restaurante";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener el restaurante";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<RESTAURANTE> GetRestaurantes()
        {
            try
            {
                IEnumerable<RESTAURANTE> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.RESTAURANTE.ToList();
                }
                return lista;
            }

            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener los restauranes";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener los restauranes";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }
    }
}
