using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EfRepositories
{
	public class EfOrderRepository : IOrderDal
	{
		public void Add(Order order)
		{
			using var c = new Context();
			c.Orders.Add(order);
			c.SaveChanges();
		}

		public void Delete(Order order)
		{
			using var c = new Context();
			c.Orders.Remove(order);
			c.SaveChanges();
		}

		public List<Order> GetAll()
		{
			using var c = new Context();
			return c.Orders.ToList();
		}

		public Order GetById(int Id)
		{
			using var c = new Context();
			return c.Orders.Find(Id);
		}

		public void Update(Order order)
		{
			using var c = new Context();
			c.Orders.Update(order);
			c.SaveChanges();
		}
	}
}
