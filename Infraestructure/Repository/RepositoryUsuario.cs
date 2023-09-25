using Infraestructure.Models;
using Infraestructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Infraestructure.Repository
{
    public class RepositoryUsuario : IRepositoryUsuario
    {
        public IEnumerable<USUARIO> GetMeseros()
        {
            IEnumerable<USUARIO> list = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.USUARIO.Where(u => u.ID_ROL == (int)ROL_ENUM.MESERO && u.ESTADO != (int)ESTADO_ENUM.ELIMINADO).ToList();
                }
                return list;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener el usuario mesero";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener el usuario mesero";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<USUARIO> GetUsuarios()
        {
            IEnumerable<USUARIO> list = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.USUARIO.Where(x => x.ESTADO != (int)ESTADO_ENUM.ELIMINADO).Include("ROL").ToList();
                }
                return list;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener los usuarios";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener los usuarios";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public USUARIO GetUsuario(string email, string password)
        {
            USUARIO oUsuario = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oUsuario = ctx.USUARIO.
                     Where(x => x.EMAIL.Equals(email) && x.CLAVE.Equals(password) && x.ESTADO == (int)ESTADO_ENUM.ACTIVO).FirstOrDefault();
                }
                if (oUsuario != null)
                    oUsuario = GetUsuarioById(oUsuario.ID_USUARIO);
                return oUsuario;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener el usuario por email y password";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener el usuario por email y password";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public USUARIO GetUsuarioById(string id)
        {
            USUARIO oUsuario = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oUsuario = ctx.USUARIO.Where(u => u.ID_USUARIO.Equals(id) && u.ESTADO != (int)ESTADO_ENUM.ELIMINADO)
                        .Include("ROL").Include("RESTAURANTE").FirstOrDefault();
                }
                return oUsuario;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener el usuario";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener el usuario";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<USUARIO> GetUsuariosById(string id)
        {
            IEnumerable<USUARIO> oUsuario = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oUsuario = ctx.USUARIO.Where(x => x.ESTADO != (int)ESTADO_ENUM.ELIMINADO).Include("ROL")
                        .ToList().FindAll(u => u.ID_USUARIO.ToLower().Contains(id.ToLower()));
                }
                return oUsuario;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener el usuario por cédula";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener el usuario por cédula";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public USUARIO Save(USUARIO usuario)
        {
            USUARIO oUsuario = null;
            int retorno = 0;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oUsuario = GetUsuarioById(usuario.ID_USUARIO);

                    IRepositoryRol repositoryRol = new RepositoryRol();
                    IRepositoryRestaurante repositoryRestaurante = new RepositoryRestaurante();
                    IRepositoryUsuario repositoryUsuario = new RepositoryUsuario();

                    var rolToAdd = repositoryRol.GetRolById((int)usuario.ID_ROL);
                    var restauranteToAdd = repositoryRestaurante.GetRestauranteById((int)usuario.ID_RESTAURANTE);

                    if (oUsuario is null)
                    {
                        usuario.ROL = rolToAdd;
                        usuario.RESTAURANTE = restauranteToAdd;

                        ctx.USUARIO.Add(usuario);
                        retorno = ctx.SaveChanges();
                    }
                    else
                    {
                        usuario.ROL = rolToAdd;
                        usuario.RESTAURANTE = restauranteToAdd;
                    }
                    ctx.USUARIO.Add(usuario);
                    ctx.Entry(usuario).State = EntityState.Modified;
                    retorno = ctx.SaveChanges();
                }

                if (retorno >= 0)
                    oUsuario = GetUsuarioById(usuario.ID_USUARIO);

                return oUsuario;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al guardar usuario";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al guardar usuario";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public void DeleteUsuario(string id)
        {
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    var oUsuario = ctx.USUARIO.Find(id);
                    if (oUsuario != null)
                    {
                        oUsuario.ESTADO = (int)ESTADO_ENUM.ELIMINADO;
                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al eliminar el usuario";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al eliminar el usuario";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }
    }
}