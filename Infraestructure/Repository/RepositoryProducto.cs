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
    public class RepositoryProducto : IRepositoryProducto
    {
        public IEnumerable<PRODUCTO> GetProducto()
        {
            IEnumerable<PRODUCTO> list = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.PRODUCTO.Where(x => x.ID_ESTADO != (int)ESTADO_ENUM.ELIMINADO).Include("RESTAURANTE").Include("CATEGORIA_PRODUCTO").ToList();
                }
                return list;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener los productos";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener los productos";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public PRODUCTO GetProductoByID(int id)
        {
            PRODUCTO oProducto = null;
            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oProducto = ctx.PRODUCTO.
                    Where(l => l.ID_PRODUCTO == id && l.ID_ESTADO != (int)ESTADO_ENUM.ELIMINADO).Include("CATEGORIA_PRODUCTO").Include("RESTAURANTE").
                    FirstOrDefault();
            }
            return oProducto;
        }

        public IEnumerable<PRODUCTO> GetProductoByRestaurant(int restaurantId)
        {
            IEnumerable<PRODUCTO> list = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.PRODUCTO.Where(x => x.ID_ESTADO != (int)ESTADO_ENUM.ELIMINADO)
                        .Include("CATEGORIA_PRODUCTO").Include(r => r.RESTAURANTE)
                        .Where(r => r.RESTAURANTE.Any(o => o.ID_RESTAURANTE == restaurantId)).ToList();
                }
                return list;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener los productos por restaurantes";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener los productos por restaurantes";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<PRODUCTO> GetProductoPorCategoria(int idCategoria)
        {
            IEnumerable<PRODUCTO> list = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.PRODUCTO.Where(p => p.ID_CATEGORIA == idCategoria && p.ID_ESTADO != (int)ESTADO_ENUM.ELIMINADO)
                        .Include("CATEGORIA_PRODUCTO").Include("RESTAURANTE").ToList();
                }
                return list;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener los productos por categoría";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener los productos por categoría";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<PRODUCTO> GetProductoByNombre(string nombre)
        {
            try
            {
                IEnumerable<PRODUCTO> oProducto = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oProducto = ctx.PRODUCTO.Where(x => x.ID_ESTADO != (int)ESTADO_ENUM.ELIMINADO).ToList().FindAll(p => p.NOMBRE.ToLower().Contains(nombre.ToLower()));
                }
                return oProducto;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al obtener el producto por nombre";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al obtener el producto por nombre";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public PRODUCTO Save(PRODUCTO producto, string[] selectedRestaurantes)
        {
            int retorno = 0;
            PRODUCTO oProducto = null;
            IRepositoryRestaurante _RepositoryRestaurante = new RepositoryRestaurante();
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oProducto = GetProductoByID((int)producto.ID_PRODUCTO);

                    if (oProducto is null)
                    {
                        if (selectedRestaurantes != null)
                        {
                            producto.RESTAURANTE = new List<RESTAURANTE>();
                            foreach (var restaurante in selectedRestaurantes)
                            {
                                var restauranteToAdd = _RepositoryRestaurante.GetRestauranteById(int.Parse(restaurante));
                                producto.RESTAURANTE.Add(restauranteToAdd);
                            }
                        }
                        oProducto.ID_ESTADO = (int)ESTADO_ENUM.ACTIVO;
                        ctx.PRODUCTO.Add(producto);
                        retorno = ctx.SaveChanges();
                    }
                    else
                    {
                        ctx.PRODUCTO.Add(producto);
                        ctx.Entry(producto).State = EntityState.Modified;
                        retorno = ctx.SaveChanges();

                        var selectedRestaurantesID = new HashSet<string>(selectedRestaurantes);
                        if (selectedRestaurantes != null)
                        {
                            ctx.Entry(producto).Collection(p => p.RESTAURANTE).Load();
                            var newRestauranteForProducto = ctx.RESTAURANTE.Where(x => selectedRestaurantesID.Contains(x.ID_RESTAURANTE.ToString())).ToList();
                            producto.RESTAURANTE = newRestauranteForProducto;

                            ctx.Entry(producto).State = EntityState.Modified;
                            retorno = ctx.SaveChanges();
                        }
                    }

                    if (retorno >= 0)
                        oProducto = GetProductoByID(producto.ID_PRODUCTO);

                    return oProducto;
                }
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al agregar el producto";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al agregar el producto";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public void DeleteProducto(int id)
        {
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    var oProducto = ctx.PRODUCTO.Find(id);
                    if (oProducto != null)
                    {
                        oProducto.ID_ESTADO = (int)ESTADO_ENUM.ELIMINADO;
                        ctx.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "Ha ocurrido un error al eliminar el producto";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "Ha ocurrido un error al eliminar el producto";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }
    }
}