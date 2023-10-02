﻿using ITCanCook_DataAcecss.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCanCook_DataAcecss.Repository
{
    public interface ICookingMethodRepo:IBaseRepository<CookingMethod>
    {
    }

    public class CookingMethodRepository : BaseRepository<CookingMethod>, ICookingMethodRepo
    {
        public CookingMethodRepository(DbContext context) : base(context)
        {
        }
    }
}
