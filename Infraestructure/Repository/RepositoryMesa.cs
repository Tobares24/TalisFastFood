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
    public class RepositoryMesa : IRepositoryMesa
    {
        public void DeleteMesa(int id)
        {
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    var oMesa = ctx.MESA.FirstOrDefault(m => m.ID_MESA == id);

                    if (oMesa != null)
                    {
                        oMesa.ID_ESTADO_MESA = (int)ESTADO_MESA_ENUM.ELIMINADA;
                        ctx.MESA.AddOrUpdate(oMesa);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al eliminar una mesa";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al eliminar una mesa";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<MESA> GetMesa()
        {
            IEnumerable<MESA> list = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.MESA.Where(x => x.ID_ESTADO_MESA != (int)ESTADO_MESA_ENUM.ELIMINADA)
                        .OrderByDescending(x => x.ID_MESA).Include("RESTAURANTE").ToList();
                }
                return list;
            }

            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener las mesas";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener las mesas";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public MESA GetMesaById(int id)
        {
            MESA oMesa = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oMesa = ctx.MESA.Where(m => m.ID_MESA == id && m.ID_ESTADO_MESA != (int)ESTADO_MESA_ENUM.ELIMINADA)
                        .Include("RESTAURANTE").FirstOrDefault();
                }
                return oMesa;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener la mesa";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener la mesa";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public MESA GetMesaByRestaurante(int idRestaurante)
        {
            MESA oMesa = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oMesa = ctx.MESA.Where(m => m.RESTAURANTE.ID_RESTAURANTE == idRestaurante && m.ID_ESTADO_MESA != (int)ESTADO_MESA_ENUM.ELIMINADA)
                        .Include("RESTAURANTE").FirstOrDefault();
                }
                return oMesa;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener la mesa";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener la mesa";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<MESA> GetMesasPorRestaurante(int idRestaurante)
        {
            IEnumerable<MESA> list = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.MESA.Where(m => m.ID_RESTAURANTE == idRestaurante && m.ID_ESTADO_MESA != (int)ESTADO_MESA_ENUM.ELIMINADA)
                        .OrderByDescending(x => x.ID_MESA).Include("RESTAURANTE").ToList();
                }
                return list;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener las mesas por restaurante";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener las mesas por restaurante";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public MESA Save(MESA mesa)
        {
            int retorno = 0;
            MESA oMesa = null;
            try
            {
                int consecutivo = ObtenerConsecutivo(mesa) + 1;

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oMesa = GetMesaById((int)mesa.ID_MESA);
                    IRepositoryRestaurante _RepositoryRestaurante = new RepositoryRestaurante();
                    IRepositoryMesa _RepositoryMesa = new RepositoryMesa();
                    IRepositoryEstadoMesa _RepositoryEstadoMesa = new RepositoryEstadoMesa();

                    var restauranteToAdd = _RepositoryRestaurante.GetRestauranteById(mesa.ID_RESTAURANTE);
                    var estadoMesaToAdd = _RepositoryEstadoMesa.GetEstadoMesaById((int)ESTADO_MESA_ENUM.DISPONIBLE);
                    var mesaToAdd = _RepositoryMesa.GetMesaById(mesa.ID_MESA);

                    if (oMesa == null)
                    {
                        ctx.RESTAURANTE.Attach(restauranteToAdd);
                        ctx.ESTADO_MESA.Attach(estadoMesaToAdd);
                        mesa.RESTAURANTE = restauranteToAdd;
                        mesa.ESTADO_MESA = estadoMesaToAdd;
                        var codigo = "M" + consecutivo + "TFF";
                        mesa.COD_MESA = codigo;
                        ctx.MESA.Add(mesa);
                        retorno = ctx.SaveChanges();
                    }
                    else
                    {
                        ctx.RESTAURANTE.Attach(restauranteToAdd);
                        ctx.ESTADO_MESA.Attach(estadoMesaToAdd);
                        mesa.RESTAURANTE = restauranteToAdd;
                        mesa.ESTADO_MESA = estadoMesaToAdd;
                        mesa.COD_MESA = mesaToAdd.COD_MESA;

                        ctx.Entry(mesa).State = EntityState.Modified;
                        retorno = ctx.SaveChanges();
                    }
                }
                if (retorno >= 0)
                    oMesa = GetMesaById((int)mesa.ID_MESA);

                return oMesa;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al guardar la información de la mesa";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al guardar la información de la mesa";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        private int ObtenerConsecutivo(MESA mesa)
        {
            IRepositoryMesa repositoryMesa = new RepositoryMesa();

            try
            {
                var lista = repositoryMesa.GetMesa();

                int consecutivo = 0;

                foreach (var item in lista)
                {
                    if (mesa.ID_RESTAURANTE == item.ID_RESTAURANTE)
                    {
                        consecutivo += 1;
                    }
                }
                return consecutivo;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener el consecutivo";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener el consecutivo";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }
    }
}