﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IAnswerPattern
{
    string Name { get; }
    bool IsMatch(List<AnswerHistory> listOfAnswers);
}

