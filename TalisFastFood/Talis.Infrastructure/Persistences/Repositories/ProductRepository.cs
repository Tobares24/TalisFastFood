using System.Data;
using Talis.Domain.Models;
using Talis.Infrastructure.Commons.Bases.Request;
using Talis.Infrastructure.Commons.Bases.Response;
using Talis.Infrastructure.Persistences.Interfaces;

namespace Talis.Infrastructure.Persistences.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ITalisContext _context;

        public ProductRepository(ITalisContext context)
        {
            this._context = context;
        }

        public async Task<BaseEntityResponse<Product>> GetAllAsync(BaseFiltersRequest filters)
        {
            try
            {
                var response = new BaseEntityResponse<Product>();
                var products = new List<Product>();

                using (var connection = _context.GetSqlConnection())
                using (var cmd = connection.CreateCommand())
                {
                    await connection.OpenAsync();

                    cmd.CommandText = "SP_GetAll_Product";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TextFilter", filters.TextFilter);
                    cmd.Parameters.AddWithValue("@LogActivo", filters.StateFilter);
                    cmd.Parameters.AddWithValue("@RecordsToSkip", filters.RecordsToSkip);
                    cmd.Parameters.AddWithValue("@NumRecordsPages", filters.NumRecordsPage);
                    cmd.Parameters.AddWithValue("@CodCategory", filters.Category);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Product oProduct = new Product()
                            {
                                CodProducto = Convert.ToInt32(reader["COD_PRODUCTO"]),
                                Fotografía = (byte[])reader["FOTOGRAFIA"],
                                Nombre = reader["NOMBRE"].ToString(),
                                DscProducto = reader["DSC_PRODUCTO"].ToString(),
                                CantidadMínima = Convert.ToInt32(reader["CANTIDAD_MINIMA"]),
                                CantidadExistencia = Convert.ToInt32(reader["CANTIDAD_EXISTENCIA"]),
                                LogActivo = Convert.ToInt32(reader["LOG_ACTIVO"]),
                                Precio  = Convert.ToDecimal(reader["PRECIO"])
                            };

                            products.Add(oProduct);
                        }
                    }
                    await connection.DisposeAsync();

                    response.TotalRecords = products.Count();
                    response.Items = products.ToList();
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
