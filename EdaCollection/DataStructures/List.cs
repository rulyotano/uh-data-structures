using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdaCollection.DataStructures
{
    public class List<T> where T : IComparable<T>
    {
        private T[] array;
        private int defaultLenght = 4;
        private int count;


        public List()
        {
            array = new T[defaultLenght];
            count = 0;
        }
        public List(int iniLenght)
        {
            defaultLenght = iniLenght;
            array = new T[defaultLenght];
            count = 0;
        }

        public int IndexOf(T item)
        {
            return IndexOf(item, 0);
        }

        /// <summary>
        /// "index": indice a partir del cual buscar
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"> </param>
        /// <returns></returns>
        public int IndexOf(T item, int index)
        {
            for (int i = index; i < count; i++)
            {
                if (array[i].Equals(item))
                    return i;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index >= count || index < 0) throw new ArgumentException("index out of range");
            if (array.Length == count)
            {
                T[] t = new T[array.Length * 2];
                for (int i = 0; i < index; i++)
                {
                    t[i] = array[i];
                }
                t[index] = item;
                for (int i = index; i < count; i++)
                {
                    t[i + 1] = array[i];
                }
                array = t;
                count++;
            }
            else if (array.Length > count)
            {
                T toPut = item;
                for (int i = index; i < count; i++)
                {
                    T t = array[i];
                    array[i] = toPut;
                    toPut = t;
                }
                array[count] = toPut;
                count++;
            }
        }

        public void RemoveAt(int index)
        {
            if (index >= count || index < 0) throw new ArgumentException("index out of range");
            for (int i = index; i < count - 1; i++)
            {
                array[i] = array[i + 1];
            }
            array[count - 1] = default(T);
            count--;
        }

        public T this[int index]
        {
            get
            {
                return array[index];
            }
            set
            {
                array[index] = value;
            }
        }

        public void Add(T item)
        {
            if (count == array.Length)
            {
                T[] t = new T[array.Length * 2];
                for (int i = 0; i < array.Length; i++)
                {
                    t[i] = array[i];
                }
                t[array.Length] = item;
                array = t;
                count++;
            }
            else
            {
                this.array[count] = item;
                count++;
            }
        }

        public void Clear()
        {
            count = 0;
            array = new T[defaultLenght];
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (array[i].Equals(item))
                    return true;
            }
            return false;
        }

        public void CopyTo(ref T[] array)
        {
            CopyTo(ref array, 0);
        }

        public void CopyTo(ref T[] array, int arrayIndex)
        {
            if (arrayIndex >= count || arrayIndex < 0) throw new ArgumentException("index out of range");
            array = new T[count - arrayIndex];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = this.array[i + arrayIndex];
            }
        }

        public int Count
        {
            get { return this.count; }
        }

        public bool Remove(T item)
        {
            bool toRet = false;
            int index = 0;
            index = this.IndexOf(item, index);
            while (index > 0)
            {
                this.RemoveAt(index);
                index = this.IndexOf(item, index);
                toRet = true;
            }
            return toRet;
        }
    }
}
