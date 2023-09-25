using Infraestructure.Models;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Infraestructure.Repository
{
    public class RepositoryOrden : IRepositoryOrden
    {
        public IEnumerable<ORDEN> GetOrdenPorEstado(int idEstado)
        {
            IEnumerable<ORDEN> list = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.ORDEN.Where(o => o.ESTADO_ORDEN == idEstado).Include("DETALLE_ORDEN").Include("DETALLE_ORDEN.PRODUCTO").Include("ESTADO_ORDEN1").Include("USUARIO").Include("USUARIO.ROL").Include("MESA")
                        .Include("MESA.RESTAURANTE").ToList();
                }
                return list;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener las órdenes por su estado";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener las órdenes por su estado";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<ORDEN> GetOrden()
        {
            IEnumerable<ORDEN> list = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.ORDEN.Include("DETALLE_ORDEN").Include("ESTADO_ORDEN1").Include("USUARIO").ToList();
                }
                return list;
            }

            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener las órdenes";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener las órdenes";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public ORDEN GetOrdenById(int id)
        {
            ORDEN oOrden = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oOrden = ctx.ORDEN.Include("DETALLE_ORDEN").Include("DETALLE_ORDEN.PRODUCTO").Include("ESTADO_ORDEN1").Include("USUARIO").Include("USUARIO.ROL").Include("MESA")
                        .Include("MESA.RESTAURANTE").Where(o => o.ID_ORDEN == id).FirstOrDefault<ORDEN>();
                }
                return oOrden;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener la órden";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener la órden";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public ORDEN GetOrdenByFactura(int idFactura)
        {
            ORDEN oOrden = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oOrden = ctx.ORDEN.Include("DETALLE_ORDEN").Include("DETALLE_ORDEN.PRODUCTO").Include("ESTADO_ORDEN1").Include("USUARIO").Include("USUARIO.ROL").Include("MESA")
                        .Include("MESA.RESTAURANTE").Where(o => o.ID_FACTURA == idFactura && o.ESTADO_ORDEN == (int)ESTADO_ORDEN_ENUM.Pagada).FirstOrDefault<ORDEN>();
                }
                return oOrden;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener la órden";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener la órden";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<ORDEN> GetOrdenByUser(string idUsuario)
        {
            List<ORDEN> oOrden = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oOrden = ctx.ORDEN.Include("DETALLE_ORDEN").Include("DETALLE_ORDEN.PRODUCTO").Include("ESTADO_ORDEN1").Include("USUARIO").Include("USUARIO.ROL").Include("MESA")
                        .Include("MESA.RESTAURANTE").Where(o => o.ID_USUARIO.Equals(idUsuario) && o.ESTADO_ORDEN != (int)ESTADO_ORDEN_ENUM.Pagada).ToList();
                }
                return oOrden;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener las órdenes del usuario";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener las órdenes del usuario";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public ORDEN Save(ORDEN pOrden)
        {
            int resultado = 0;
            ORDEN orden = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    var existingOrder = ctx.ORDEN.Find(pOrden.ID_ORDEN);

                    if (existingOrder != null)
                    {
                        ctx.Entry(existingOrder).CurrentValues.SetValues(pOrden);
                        ctx.Entry(existingOrder).State = EntityState.Modified;
                    }
                    else
                    {
                        ctx.ORDEN.Add(pOrden);
                    }

                    if (resultado >= 0)
                        orden = GetOrdenById(pOrden.ID_ORDEN);

                    return orden;
                }
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al guardar la órden";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al guardar la órden";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
        }
    }
}
