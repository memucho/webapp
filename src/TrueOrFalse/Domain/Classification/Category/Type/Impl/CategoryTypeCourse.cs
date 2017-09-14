﻿using System;
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeCourse : CategoryTypeBase<CategoryTypeCourse>
{

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.Course; } }
}