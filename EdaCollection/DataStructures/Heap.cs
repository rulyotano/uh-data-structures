using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdaCollection.DataStructures
{
    public class Heap<T>
    {
        private int count;
        private int defaultLength = 10;
        private NodoColaPHeap[] array;
        private bool isMin;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isMin">true si quiere que la ColaHeap devuelva el elemento de menor prioridad, falso si quiere que devuelva el de mayor</param>
        public Heap(bool isMin)
        {
            this.count = 0;
            this.isMin = isMin;
            this.array = new NodoColaPHeap[defaultLength];
        }
        public Heap(bool isMin, int iniLength)
        {
            this.count = 0;
            this.isMin = isMin;
            this.defaultLength = iniLength;
            this.array = new NodoColaPHeap[defaultLength];
        }

        public class NodoColaPHeap
        {
            T valor;
            int prioridad;

            public NodoColaPHeap(T valor, int prioridad)
            {
                this.valor = valor;
                this.prioridad = prioridad;
            }

            public T Valor
            {
                get
                { return this.valor; }
            }
            public int Prioridad
            {
                get
                { return this.prioridad; }
            }

        }
        public int Count
        { get { return this.count; } }

        /// <summary>
        /// Devuelve true si la cola devuelve el valor de menor prioridad, falso si el de mayor
        /// </summary>
        public bool IsMin
        { get { return isMin; } }

        /// <summary>
        /// Devuelve y quita el Valor Minimo si la cola lo permite,si no, retorna null
        /// </summary>
        /// <returns></returns>
        public NodoColaPHeap GetTopAndDelete()
        {
            NodoColaPHeap toRet;
            if (count > 0)
            {
                if (count == 1)
                {
                    toRet = array[0];
                    array[0] = null;
                    count--;
                    return toRet;
                }
                else
                {
                    toRet = array[0];
                    array[0] = array[count - 1];
                    array[count - 1] = null;
                    HeapyfiToDown(0);
                    count--;
                    return toRet;
                }
            }
            else return null;

        }

        /// <summary>
        /// Devuelve el tope pero no lo borra
        /// </summary>
        /// <returns></returns>
        public NodoColaPHeap GetTop()
        {
            return array[0];
        }
        public void Insert(NodoColaPHeap p)
        {
            if (array.Length == count)
                Add(p);
            else array[count] = p;
            count++;
            HeapyfiToUp(count - 1);
        }


        #region Private Functions
        private int GetFather(int i)
        {
            return ((i + 1) / 2) - 1;
        }
        private int GetRightSon(int i)
        { return 2 * i + 2; }
        private int GetLeftSon(int i)
        { return 2 * i + 1; }

        private void Add(NodoColaPHeap p)
        {
            if (array.Length == count)
            {
                NodoColaPHeap[] t = new NodoColaPHeap[array.Length * 2];
                for (int i = 0; i < array.Length; i++)
                {
                    t[i] = array[i];
                }
                t[count] = p;
                array = t;
            }
        }

        private void HeapyfiToUp(int i)
        {
            if (isMin)
            {
                int father = GetFather(i);
                if (father > -1 && array[father].Prioridad > array[i].Prioridad)
                {
                    NodoColaPHeap t = array[father];
                    array[father] = array[i];
                    array[i] = t;
                    HeapyfiToUp(father);
                }
            }
            else
            {
                int father = GetFather(i);
                if (father > -1 && array[father].Prioridad < array[i].Prioridad)
                {
                    NodoColaPHeap t = array[father];
                    array[father] = array[i];
                    array[i] = t;
                    HeapyfiToUp(father);
                }
            }
        }
        private void HeapyfiToDown(int i)
        {
            if (isMin)
            {
                #region HeapyFi To down Min
                int l = GetLeftSon(i);
                int r = GetRightSon(i);

                if (r < count)
                {
                    NodoColaPHeap right = array[r];
                    NodoColaPHeap left = array[l];
                    int t;
                    if (right != null && left != null)
                    {
                        t = left.Prioridad < right.Prioridad ? l : r;
                    }
                    else if (right != null)
                        t = r;

                    else if (left != null)
                        t = l;
                    else return;

                    if (array[t].Prioridad < array[i].Prioridad)
                    {
                        NodoColaPHeap temp = array[t];
                        array[t] = array[i];
                        array[i] = temp;
                        HeapyfiToDown(t);
                    }
                }
                else if (l < count)
                {
                    NodoColaPHeap left = array[l];
                    int t;
                    if (left != null)
                        t = l;
                    else return;
                    if (array[t].Prioridad < array[i].Prioridad)
                    {
                        NodoColaPHeap temp = array[t];
                        array[t] = array[i];
                        array[i] = temp;
                        HeapyfiToDown(t);
                    }
                }
                #endregion
            }
            else
            {
                #region HeapyFi To down NOT Min
                int l = GetLeftSon(i);
                int r = GetRightSon(i);

                if (r < count)
                {
                    NodoColaPHeap right = array[r];
                    NodoColaPHeap left = array[l];
                    int t;
                    if (right != null && left != null)
                    {
                        t = left.Prioridad > right.Prioridad ? l : r;
                    }
                    else if (right != null)
                        t = r;

                    else if (left != null)
                        t = l;
                    else return;

                    if (array[t].Prioridad > array[i].Prioridad)
                    {
                        NodoColaPHeap temp = array[t];
                        array[t] = array[i];
                        array[i] = temp;
                        HeapyfiToDown(t);
                    }
                }
                else if (l < count)
                {
                    NodoColaPHeap left = array[l];
                    int t;
                    if (left != null)
                        t = l;
                    else return;
                    if (array[t].Prioridad > array[i].Prioridad)
                    {
                        NodoColaPHeap temp = array[t];
                        array[t] = array[i];
                        array[i] = temp;
                        HeapyfiToDown(t);
                    }
                }
                #endregion
            }
        }
        #endregion
    }
}
