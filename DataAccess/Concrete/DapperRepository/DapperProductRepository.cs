using System.Data;
using System.Data.SqlClient;
using Dapper;
using DataAccess.Abstract;
using Entity.Concrete.Dtos;
using Entity.Concrete;

public class DapperProductRepository : IProductDal
{
	private readonly string _connectionString;

	public DapperProductRepository()
	{
		_connectionString = "server=Cagri; database=CrmDb; integrated security=true; TrustServerCertificate=True";
	}

	private IDbConnection CreateConnection()
	{
		return new SqlConnection(_connectionString);
	}

	public void Add(Product t)
	{
		using (var connection = CreateConnection())
		{
			var sql = @"INSERT INTO Products (Name, CategoryId, Price)
                        VALUES (@Name, @CategoryId, @Price)";
			connection.Execute(sql, t);
		}
	}

	public void Delete(Product t)
	{
		using (var connection = CreateConnection())
		{
			var sql = "DELETE FROM Products WHERE Id = @Id";
			connection.Execute(sql, new { t.Id });
		}
	}

	public List<Product> GetAll()
	{
		using (var connection = CreateConnection())
		{
			var sql = "SELECT * FROM Products";
			return connection.Query<Product>(sql).ToList();
		}
	}

	public Product GetById(int id)
	{
		using (var connection = CreateConnection())
		{
			var sql = "SELECT * FROM Products WHERE Id = @Id";
			return connection.QueryFirstOrDefault<Product>(sql, new { Id = id });
		}
	}

	public List<ProductWithCategory> ListProductView()
	{
		using (var connection = CreateConnection())
		{
			var sql = @"
                SELECT p.Id,
                       p.Name,
                       p.Price,
                       c.Name AS CategoryName,
					   p.ImagePath,
					   p.Description,
					   p.QuantityInStock
                FROM Products p
                INNER JOIN Categories c ON p.CategoryId = c.Id";
			return connection.Query<ProductWithCategory>(sql).ToList();
		}
	}

	public void Update(Product t)
	{
		using (var connection = CreateConnection())
		{
			var sql = @"UPDATE Products
                        SET Name = @Name,
                            CategoryId = @CategoryId,
                            Price = @Price
                        WHERE Id = @Id";
			connection.Execute(sql, t);
		}
	}
}
