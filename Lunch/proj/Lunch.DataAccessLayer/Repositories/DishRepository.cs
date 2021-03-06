﻿using Lunch.DataAccess;
using Lunch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunch.DataAccessLayer.Repositories
{
    public class DishRepository : RepositoryBase<Dish>
    {
        #region Constructor
        internal DishRepository(Entities context)
            : base(context)
        {
        }
        #endregion


        #region Public methods
        public void Upsert(Dish entity)
        {
            if (entity.Id == default(int))
            {
                this.InsertEntity(entity);
            }
            else
            {
                this.UpdateEntity(entity);
            }
        }

        public List<Dish> GetAllDishes()
        {
            return this.DbContext.Set<Dish>().ToList();
        }

        public Dish GetDishById(int id)
        {
            return this.DbContext.Set<Dish>().FirstOrDefault(d => d.Id == id);
        }
        #endregion
    }
}
