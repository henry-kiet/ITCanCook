﻿using ITCanCook_DataAcecss.Entities;
using ITCanCook_DataAcecss.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITCanCook_BusinessObject.ServiceModel.RequestModel
{
    public class CookingHobbyCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public CookingHobbyStatus Status { get; set; }
    }
}
