﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class CategoryBook : CategoryBase<CategoryBook>
{
    public string Title;
    public string Subtitle;
    public string Author;
    public string ISBN;
    public string Publisher;
    public string PublicationCity;
    public string PublicationYear;
}