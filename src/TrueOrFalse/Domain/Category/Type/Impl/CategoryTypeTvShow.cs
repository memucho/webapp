﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeTvShow : CategoryTypeBase<CategoryTypeTvShow>
{

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.TvShow; } }
}