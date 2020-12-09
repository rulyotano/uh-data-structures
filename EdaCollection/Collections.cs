using System;
using System.Text;

namespace EdaCollection
{
    public class Lista<T> where T:IComparable<T>
    {
        private T[] array;
        private int defaultLenght = 4;
        private int count;


        public Lista()
        {
            array = new T[defaultLenght];
            count = 0;
        }
        public Lista(int iniLenght)
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
    public class ColaPHeap<T>
    {
        private int count;
        private int defaultLength = 10;
        private NodoColaPHeap[] array;
        private bool isMin;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isMin">true si quiere que la ColaHeap devuelva el elemento de menor prioridad, falso si quiere que devuelva el de mayor</param>
        public ColaPHeap(bool isMin)
        {
            this.count = 0;
            this.isMin = isMin;
            this.array = new NodoColaPHeap[defaultLength];
        }
        public ColaPHeap(bool isMin, int iniLength)
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
    public class AVL<T>:System.Collections.Generic.IEnumerable<T> where T:IComparable<T>
    {
    	public class NodoAvl //28 bytes + tamanyo del valoy
    	{
    		public NodoAvl Padre,HijoDercho,HijoIzq;
    		private T valor;    		
			
    		private int altura=0;
    		private int balance=0;
    		
    		internal int altPrev;
    		internal int balPrev;
    					    		
    		public NodoAvl(T valor)
    		{
    			this.valor = valor;
    			ReCalcula();
    		}
    		
    		
    		public T Valor {
				get {
					return valor;
				}
                set { this.valor = value; }
			}
    		
    		public int Altura {
				get {
					return altura;
				}				
			}
    		
    		public int Balance {
				get {
					return balance;
				}				
			}
    		
    		public void ReCalcula()
    		{
    			this.balPrev = this.balance;
    			this.altPrev = this.altura;
    			int hD = this.HijoDercho == null ? -1:this.HijoDercho.altura;
    			int hI = this.HijoIzq == null ? -1 : this.HijoIzq.altura;
    			this.balance = hD-hI; 
    			this.altura = Math.Max(hD,hI);    			
    			this.altura++;
    		}    		
    	}//NodoAvl
       
    	private NodoAvl raiz;
    	private int count;
    	    	
    	public AVL()
    	{
    		this.count = 0;
    		raiz = null;    		
    	}
    	
    	
    	public bool Contains(T item)
    	{
    		return EstaInterno(raiz,item);
    	}
    	
    	public bool Insert(T item)
    	{    		
    		NodoAvl t = AddInterno(this.raiz,item);
            

            if (t!= null && raiz == null) raiz = t;
            else if (t == null) return false;

            this.count++;
    		SubeEnInsercion(t);
            return true;
    	}

        public T Get(T item)
        {
            NodoAvl t;
            if ((t = GetItemInterno(raiz, item)) == null)
                throw new Exception("Elemento no encontrado");
            else return t.Valor;
            
        }
    	
    	public T[] GetSortedItems(bool creciente)
    	{
    		T[] toRet = new T[this.count];
            if (this.count > 0)
            {
                int t = 0;
                if (!creciente) RecorreInOrderByLeft(ref t, ref toRet, this.raiz);
                else RecorreInOrderByRight(ref t, ref toRet, this.raiz);
            }
    		return toRet;
    	}
    	public System.Collections.Generic.IEnumerable<T> GetSortedItemsInv()
        {
            foreach (T item in RecorreInOrderByLeft(raiz))
                yield return item;
        }
        
    	public int Count
    	{
    		get
    		{
    			return count;
    		}
    	}
        public void Clear()
        {
            raiz = null;
            count = 0;
        }

        public bool Elimina(T item)
        {
            try
            {
                SubeEnEliminacion(EliminaRetornaNodoCritico(item));                
                return true;
            }
            catch { return false; }
        }
    	
    	private void RecorreInOrderByLeft(ref int index,ref T[] array,NodoAvl arbol)
    	{    		
    		if (arbol.HijoIzq != null)
    			RecorreInOrderByLeft(ref index,ref array,arbol.HijoIzq);
    		
    		array[index] = arbol.Valor;
    		index++;
    		
    		if (arbol.HijoDercho!=null)
    			RecorreInOrderByLeft(ref index,ref array,arbol.HijoDercho);
    	}
    	
    	private void RecorreInOrderByRight(ref int index,ref T[] array,NodoAvl arbol)
    	{
            if (arbol.HijoDercho != null)
                RecorreInOrderByRight(ref index, ref array, arbol.HijoDercho);

            array[index] = arbol.Valor;
            index++;


            if (arbol.HijoIzq != null)
                RecorreInOrderByRight(ref index, ref array, arbol.HijoIzq);
            
    	}

        private System.Collections.Generic.IEnumerable<T> RecorreInOrderByLeft(NodoAvl arbol)
        {
            if (arbol.HijoIzq != null)
                foreach (T var in RecorreInOrderByLeft(arbol.HijoIzq))
                    yield return var;

            yield return arbol.Valor;

            if (arbol.HijoDercho != null)
                foreach (T var in RecorreInOrderByLeft(arbol.HijoDercho))
                    yield return var;	 
        }

        private System.Collections.Generic.IEnumerable<T> RecorreInOrderByRight(NodoAvl arbol)
        {
            if (arbol.HijoDercho != null)
                foreach (T var in RecorreInOrderByRight(arbol.HijoDercho))
                   yield return var; 
                

            yield return arbol.Valor;

            if (arbol.HijoIzq != null)
                foreach (T var in RecorreInOrderByRight(arbol.HijoIzq))
                    yield return var;
        }

        private void SubeEnInsercion(NodoAvl hijo)
        {
            NodoAvl t = hijo;
            if (t.Padre != null)
            {
                t.Padre.ReCalcula();
                if (t.Padre.Balance == 0 || t.Padre.balPrev == t.Padre.Balance) return;
                t = t.Padre;
                while (t.Padre != null)
                {
                    t.Padre.ReCalcula();                    
                    if ((t.Padre.Balance == 0) || (t.Padre.Balance == t.Padre.balPrev))//si no afecto el balance
                    {
                        return;
                    }
                    else if (((t.Padre.Balance <= -2) || (t.Padre.Balance >= 2)) && ((t.Padre.balPrev >= 1) || (t.Padre.balPrev <= -1)))
                    {

                        //situacion de desbalance
                        if (t.Padre.Balance <= -2 && t.Balance <= -1)
                            t = RotaIII(t.Padre);
                        else if (t.Padre.Balance <= -2 && t.Balance >= 1)
                            t = RotaIID(t.Padre);
                        else if (t.Padre.Balance >= 2 && t.Balance >= 1)
                            t = RotaDDD(t.Padre);
                        else if (t.Padre.Balance >= 2 && t.Balance <= -1)
                            t = RotaDDI(t.Padre);

                        if (t.Padre == null) this.raiz = t;
                        return;
                    }
                    t = t.Padre;
                }
            }            
        }

        private void SubeEnEliminacion(NodoAvl hijo)
        {
            NodoAvl t = hijo;
            t.ReCalcula();
            do
            {
                if (t.balPrev == 0 && (t.Balance == -1 || t.Balance == 1))
                { break; }
                else if (t.Balance == -2 || t.Balance == 2)
                {
                    if (t.Balance == -2)
                    {
                        if (t.HijoIzq.Balance == 0)
                        {
                            t = RotaIII(t);
                            if (t.Padre == null)
                                raiz = t;
                            break;
                        }
                        else if (t.HijoIzq.Balance == -1)
                        {
                            t = RotaIII(t);
                            if (t.Padre == null)
                            {
                                raiz = t;
                                break;
                            }
                            t = t.Padre;
                            continue;//no tengo que recalcularlo pq lo hago en la rotacion
                        }
                        else if (t.HijoIzq.Balance == 1)
                        {
                            t = RotaIID(t);
                            if (t.Padre == null)
                            {
                                raiz = t;
                                break;
                            }
                            t = t.Padre;
                            continue;
                        }
                    }
                    else if (t.Balance == 2)
                    {
                        if (t.HijoDercho.Balance == 0)
                        {
                            t = RotaDDD(t);
                            if (t.Padre == null)
                                raiz = t;
                            break;
                        }
                        else if (t.HijoDercho.Balance == 1)
                        {
                            t = RotaDDD(t);
                            if (t.Padre == null)
                            {
                                raiz = t;
                                break;
                            }
                            t = t.Padre;
                            continue;
                        }
                        else if (t.HijoDercho.Balance == -1)
                        {
                            t = RotaDDI(t);
                            if (t.Padre == null)
                            {
                                raiz = t;
                                break;
                            }
                            t = t.Padre;
                            continue;
                        }
                    }//else 
                }//else bal=2 V bal=-2
                else 
                {
                    if (t.Padre != null)
                    {
                        t = t.Padre;
                        t.ReCalcula();
                    }
                    else break;                    
                }
            } while (true);
        }

        private NodoAvl EliminaRetornaNodoCritico(T item)
        {
            if (raiz == null) throw new InvalidOperationException("Arbol vacio");
            if (!this.Contains(item)) throw new InvalidOperationException("No se contiene el elemento");
            count--;
            NodoAvl toRet;
            NodoAvl toDel = GetItemInterno(raiz,item);
            if (toDel.HijoIzq == null && toDel.HijoDercho == null)
            {
                toRet = toDel.Padre;
                if (toRet == null) { raiz = null; return null; }
                else
                {
                    int t = toDel.Valor.CompareTo(toDel.Padre.Valor);
                    if (t < 0) toDel.Padre.HijoDercho = null;
                    else toDel.Padre.HijoIzq = null;
                    toDel.Padre = null;
                    return toRet;
                }
            }
            else if ((toDel.HijoIzq == null && toDel.HijoDercho != null) || (toDel.HijoIzq != null && toDel.HijoDercho == null))
            {
                NodoAvl toDelHijo = toDel.HijoDercho == null ? toDel.HijoIzq : toDel.HijoDercho;
                toRet = toDel.Padre;
                if (toRet == null)
                {
                    toDelHijo.Padre = null;
                    raiz = toDelHijo;
                    toDel.HijoDercho = null; toDel.HijoIzq = null;
                    return toRet;
                }
                else
                {
                    int t = toDel.Valor.CompareTo(toDel.Padre.Valor);
                    if (t < 0)
                    {
                        toDel.Padre.HijoDercho = toDelHijo;
                        toDelHijo.Padre = toDel.Padre;
                        toDel.Padre = null;
                        toDel.HijoDercho = null; toDel.HijoIzq = null;
                        return toRet;
                    }
                    else
                    {
                        toDel.Padre.HijoIzq = toDelHijo;
                        toDelHijo.Padre = toDel.Padre;
                        toDel.Padre = null;
                        toDel.HijoDercho = null; toDel.HijoIzq = null;
                        return toRet;
                    }
                }//else del padre null
            }
            else
            {
                NodoAvl menorDeMayores = GetMenorInterno(toDel.HijoIzq);
                if (menorDeMayores.HijoIzq == null)
                {
                    toDel.Valor = menorDeMayores.Valor;
                    toRet = menorDeMayores.Padre;
                    if (menorDeMayores.Valor.CompareTo(menorDeMayores.Padre.Valor) < 0)
                        menorDeMayores.Padre.HijoDercho = null;
                    else menorDeMayores.Padre.HijoIzq = null;
                    menorDeMayores.Padre = null;
                    return toRet;
                }
                else
                {
                    toDel.Valor = menorDeMayores.Valor;
                    toRet = menorDeMayores.Padre;
                    if (menorDeMayores.Valor.CompareTo(menorDeMayores.Padre.Valor) < 0)
                    {
                        menorDeMayores.Padre.HijoDercho = menorDeMayores.HijoIzq;
                        menorDeMayores.HijoIzq.Padre = menorDeMayores.Padre;
                    }
                    else
                    {
                        menorDeMayores.Padre.HijoIzq = menorDeMayores.HijoIzq;
                        menorDeMayores.HijoIzq.Padre = menorDeMayores.Padre;
                    }
                    menorDeMayores.Padre = null;
                    menorDeMayores.HijoIzq = null;
                    return toRet;
                }
                
            }
        }

    	#region PrivateStaticMethos
    	/*
    	 * factor de balance = 
    	  III -> --,-
    	  IID -> --,+
    	  DDD -> ++,+
    	  DDI -> ++,-
    	 */
    	private static NodoAvl RotaIII(NodoAvl r)
    	{
    		/*       r
    		 *      /
    		 *     x
    		 *    /
    		 *   w
    		 */
    		NodoAvl x = r.HijoIzq;              
    		r.HijoIzq = x.HijoDercho;
            if (r.HijoIzq != null) r.HijoIzq.Padre = r;//imp

    		x.HijoDercho = r;
    		x.Padre = r.Padre;
    		r.Padre = x;
            if (x.Padre != null)
            {
                if ( x.Valor.CompareTo(x.Padre.Valor)>0)
                    x.Padre.HijoIzq = x;
                else x.Padre.HijoDercho = x;
            }
    		r.ReCalcula();
    		x.ReCalcula();
    		if (x.Padre!=null) x.Padre.ReCalcula();
            return x;
    	}
        private static NodoAvl RotaIID(NodoAvl r)
    	{
    		/*    r
    		 *   / 
    		 *  x
    		 *   \
    		 *    w
    		 */
    		NodoAvl x = r.HijoIzq;
    		NodoAvl w = x.HijoDercho;
    		
    		x.HijoDercho = w.HijoIzq;
            if (x.HijoDercho != null) x.HijoDercho.Padre = x;

    		r.HijoIzq = w.HijoDercho;
            if (r.HijoIzq != null) r.HijoIzq.Padre = r;

    		w.HijoIzq = x;
    		w.HijoDercho = r;
    		w.Padre = r.Padre;
    		x.Padre = w;
    		r.Padre = w;
            if (w.Padre != null)
            {
                if (w.Valor.CompareTo(w.Padre.Valor)>0)
                    w.Padre.HijoIzq = w;
                else w.Padre.HijoDercho = w;
            }

    		x.ReCalcula();
    		r.ReCalcula();
    		w.ReCalcula();
    		if (w.Padre!=null) w.Padre.ReCalcula();
            return w;
    	}
        private static NodoAvl RotaDDD(NodoAvl r)
    	{
    		/*       r
    		 *        \
    		 *         x
    		 *          \
    		 *           w
    		 */
    		NodoAvl x = r.HijoDercho;              
    		r.HijoDercho = x.HijoIzq;
            if (r.HijoDercho != null) r.HijoDercho.Padre = r;//imp

    		x.HijoIzq = r;
    		x.Padre = r.Padre;
    		r.Padre = x;
            if (x.Padre != null)
            {
                if (x.Valor.CompareTo(x.Padre.Valor) > 0)
                    x.Padre.HijoIzq = x;
                else x.Padre.HijoDercho = x;
            }
            r.ReCalcula();
    		x.ReCalcula();
    		if (x.Padre!=null) x.Padre.ReCalcula();
            return x;    		
    	}
        private static NodoAvl RotaDDI(NodoAvl r)
    	{
    		
    		/*    r
    		 *     \ 
    		 *      x
    		 *     /  
    		 *    w
    		 */
    		NodoAvl x = r.HijoDercho;
    		NodoAvl w = x.HijoIzq;
    		
    		x.HijoIzq = w.HijoDercho;
            if (x.HijoIzq != null) x.HijoIzq.Padre = x;

    		r.HijoDercho = w.HijoIzq;
            if (r.HijoDercho != null) r.HijoDercho.Padre = r;

    		w.HijoIzq = r;
    		w.HijoDercho = x;
    		w.Padre = r.Padre;
    		x.Padre = w;
    		r.Padre = w;
            if (w.Padre != null)
            {
                if (w.Valor.CompareTo(w.Padre.Valor) > 0)
                    w.Padre.HijoIzq = w;
                else w.Padre.HijoDercho = w;
            }

    		x.ReCalcula();    		
    		r.ReCalcula();
    		w.ReCalcula();
    		if (w.Padre!=null) w.Padre.ReCalcula();
            return w;
    	}
    	/// <summary>
    	/// Devuelve el Nodo Despues que se inserto
    	/// </summary>
    	/// <param name="valor"></param>
    	/// <returns></returns>
    	private static NodoAvl AddInterno(NodoAvl arbol, T valor)
    	{
            if (arbol != null)
            {
                int t = arbol.Valor.CompareTo(valor);
                if (t < 0)
                {
                    if (arbol.HijoIzq == null)
                    {
                        arbol.HijoIzq = new NodoAvl(valor);
                        arbol.HijoIzq.Padre = arbol;
                        return arbol.HijoIzq;
                    }
                    else return AddInterno(arbol.HijoIzq, valor);
                }
                else if (t > 0)
                {
                    if (arbol.HijoDercho == null)
                    {
                        arbol.HijoDercho = new NodoAvl(valor);
                        arbol.HijoDercho.Padre = arbol;
                        return arbol.HijoDercho;
                    }
                    else return AddInterno(arbol.HijoDercho, valor);
                }
                else return null;
                //no se modifican la altura y fb del padre
            }
            else return new NodoAvl(valor);
    		
    	}
    	private static bool EstaInterno(NodoAvl arbol, T valor)
    	{    		
    		if (arbol==null)
    		{    			
    			return false;
    		}else
    		{
    			int t = arbol.Valor.CompareTo(valor);
    			if (t<0)
    				return EstaInterno(arbol.HijoIzq,valor);
    			else if (t>0) return EstaInterno(arbol.HijoDercho,valor);
    			else return true;
    		}
    	}
        private static NodoAvl GetItemInterno(NodoAvl arbol, T valor)
        {
            if (arbol != null)
            {
                int t = arbol.Valor.CompareTo(valor);
                if (t == 0) return arbol;
                else if (t > 0 && arbol.HijoDercho != null) return GetItemInterno(arbol.HijoDercho, valor);
                else if (t < 0 && arbol.HijoIzq != null) return GetItemInterno(arbol.HijoIzq, valor);
                else return null;
            }
            else return null;
        }
        private static NodoAvl GetMayorInterno(NodoAvl arbol)
        {
            if (arbol != null)
            {
                if (arbol.HijoIzq != null) return GetMayorInterno(arbol.HijoIzq);
                else return arbol;
            }
            else return null;
        }
        private static NodoAvl GetMenorInterno(NodoAvl arbol)
        {
            if (arbol != null)
            {
                if (arbol.HijoDercho != null) return GetMenorInterno(arbol.HijoDercho);
                else return arbol;
            }
            else return null;
        }
    	#endregion

        #region IEnumerable<T> Members

        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            if (this.count > 0)
            {
                foreach (T item in RecorreInOrderByRight(raiz))
                {
                    yield return item;
                }
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (T item in this)
                yield return item;
        }

        #endregion
    }

    public class Diccionario<K, V>:System.Collections.Generic.IEnumerable<V> where K : IComparable<K>

    {
        private AVL<NodoDiccionario> avl = new AVL<NodoDiccionario>();

        public bool Contains(K item)
        {
            return avl.Contains(new NodoDiccionario(item,default(V)));
        }

        public V GetValue(K item)
        {
            try
            {
                return avl.Get(new NodoDiccionario(item, default(V))).Valor;
            }
            catch
            {
                throw new InvalidOperationException("Extraer valor que no existe");
            }
        }

        public int Count
        {
            get
            {
                return avl.Count;
            }
        }

        public void Clear()
        {
            avl.Clear();
        }

        public void Add(K llave, V valor)
        {
            avl.Insert(new NodoDiccionario(llave, valor));
        }

        public bool Delete(K llave)
        {
            return avl.Elimina(new NodoDiccionario(llave, default(V)));
        }
       
        private class NodoDiccionario:IComparable<NodoDiccionario>
        {
            public V Valor;
            public K Key;

            public NodoDiccionario(K key,V valor)
            {
                this.Valor = valor;
                this.Key = key;
            }
            #region IComparable<NodoDiccionario> Members

            public int CompareTo(NodoDiccionario other)
            {
                return Key.CompareTo(other.Key);
            }

            #endregion
            public override bool Equals(object obj)
            {
                
                NodoDiccionario t = (NodoDiccionario)obj;
                return base.Equals(Key.Equals(t.Key));
            }
            public override int GetHashCode()
            {
                return Key.GetHashCode();
            }
        }

        #region IEnumerable<V> Members

        public System.Collections.Generic.IEnumerator<V> GetEnumerator()
        {
            if (this.Count > 0)
            {
                foreach (NodoDiccionario item in avl)
                {
                    yield return item.Valor;
                }
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (V item in this)
            {
                yield return item;                
            }
        }

        #endregion
    }

}
