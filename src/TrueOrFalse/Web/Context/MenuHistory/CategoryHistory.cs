﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    [Serializable]
    public class CategoryHistory : HistoryBase<CategoryHistoryItem>
    {
    }

    [Serializable]
    public class CategoryHistoryItem : HistoryItemBase
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public HistoryItemType Type { get; set; }

        public CategoryHistoryItem(
            Category category, 
            HistoryItemType type = HistoryItemType.Any)
        {
            Id = category.Id;
            Name = category.Name;
            Type = type;
        }
    }
}
