﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathOperations.Services.Interfaces
{
  public interface IMathExpressionEvualationService
  {
    decimal Evaluate(string expression);
  }
}
