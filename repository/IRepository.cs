﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repository
{
    public interface IRepository<T>
    {
        void Delete(int id);
        T Get(int id);
        T[] GetAll(string pattern);
        void Insert(T item);
        void Update(T item);
    }
}
