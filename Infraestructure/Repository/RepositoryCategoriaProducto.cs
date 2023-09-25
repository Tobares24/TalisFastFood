using Infraestructure.Models;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Infraestructure.Repository
{
    public class RepositoryCategoria : IRepositoryCategoriaProducto
    {
        public IEnumerable<CATEGORIA_PRODUCTO> GetCategoria()
        {
            IEnumerable<CATEGORIA_PRODUCTO> list = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.CATEGORIA_PRODUCTO.Include("PRODUCTO").ToList();
                }
                return list;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener la lista de categorías";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener la lista de categorías";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public CATEGORIA_PRODUCTO GetCategoriaById(int id)
        {
            try
            {
                CATEGORIA_PRODUCTO oCategoria = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oCategoria = ctx.CATEGORIA_PRODUCTO.Where(x => x.ID_CATEGORIA == id).FirstOrDefault();
                }
                return oCategoria;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener la categoría";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener la categoría";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }
    }
}