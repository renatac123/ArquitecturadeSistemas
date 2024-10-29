using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositorios
{
    public class VentaRepository: GenericRepository<Venta>, IVentaRepository
    {
        private readonly DbventaContext dbcontext;

        public VentaRepository(DbventaContext dbContext): base(dbContext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
            Venta ventaGenerada = new Venta();

            using (var transaction = dbcontext.Database.BeginTransaction())
            {
                try { 
                    foreach(DetalleVenta dv in modelo.DetalleVenta)
                    {
                        Producto producto_encontrado = dbcontext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();

                        producto_encontrado.Stock = producto_encontrado.Stock - dv.Cantidad; //dv viene a ser cada item que tenemos en la list detalleventa
                        dbcontext.Productos.Update(producto_encontrado);
                    }
                    await dbcontext.SaveChangesAsync();

                    NumeroDocumento correlativo = dbcontext.NumeroDocumentos.First();

                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaRegistro = DateTime.Now;

                    dbcontext.NumeroDocumentos.Update(correlativo);
                    await dbcontext.SaveChangesAsync();

                    int CantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", CantidadDigitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();

                    //00001
                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - CantidadDigitos, CantidadDigitos);

                    //actualizar el numero de documento de nuestra venta
                    modelo.NumeroDocumento = numeroVenta;

                    await dbcontext.Venta.AddAsync(modelo);

                    await dbcontext.SaveChangesAsync();

                    ventaGenerada = modelo;

                    transaction.Commit();

                } catch { 
                    transaction.Rollback(); //devuelve todo como estaba antes en caso de error
                    throw;
                }
                return ventaGenerada;

            }



        }
    }
}
