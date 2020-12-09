using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdaCollection.DataStructures
{
    public class Avl<T> : IEnumerable<T> where T : IComparable<T>
    {
        public class NodoAvl //28 bytes + tamanyo del valoy
        {
            public NodoAvl Padre, HijoDercho, HijoIzq;
            private T valor;

            private int altura = 0;
            private int balance = 0;

            internal int altPrev;
            internal int balPrev;

            public NodoAvl(T valor)
            {
                valor = valor;
                ReCalcula();
            }


            public T Valor
            {
                get
                {
                    return valor;
                }
                set { valor = value; }
            }

            public int Altura
            {
                get
                {
                    return altura;
                }
            }

            public int Balance
            {
                get
                {
                    return balance;
                }
            }

            public void ReCalcula()
            {
                balPrev = balance;
                altPrev = altura;
                int hD = HijoDercho == null ? -1 : HijoDercho.altura;
                int hI = HijoIzq == null ? -1 : HijoIzq.altura;
                balance = hD - hI;
                altura = Math.Max(hD, hI);
                altura++;
            }
        }//NodoAvl

        private NodoAvl raiz;
        private int count;

        public Avl()
        {
            count = 0;
            raiz = null;
        }


        public bool Contains(T item)
        {
            return EstaInterno(raiz, item);
        }

        public bool Insert(T item)
        {
            NodoAvl t = AddInterno(raiz, item);


            if (t != null && raiz == null) raiz = t;
            else if (t == null) return false;

            count++;
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
            T[] toRet = new T[count];
            if (count > 0)
            {
                int t = 0;
                if (!creciente) RecorreInOrderByLeft(ref t, ref toRet, raiz);
                else RecorreInOrderByRight(ref t, ref toRet, raiz);
            }
            return toRet;
        }
        public IEnumerable<T> GetSortedItemsInv()
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

        private void RecorreInOrderByLeft(ref int index, ref T[] array, NodoAvl arbol)
        {
            if (arbol.HijoIzq != null)
                RecorreInOrderByLeft(ref index, ref array, arbol.HijoIzq);

            array[index] = arbol.Valor;
            index++;

            if (arbol.HijoDercho != null)
                RecorreInOrderByLeft(ref index, ref array, arbol.HijoDercho);
        }

        private void RecorreInOrderByRight(ref int index, ref T[] array, NodoAvl arbol)
        {
            if (arbol.HijoDercho != null)
                RecorreInOrderByRight(ref index, ref array, arbol.HijoDercho);

            array[index] = arbol.Valor;
            index++;


            if (arbol.HijoIzq != null)
                RecorreInOrderByRight(ref index, ref array, arbol.HijoIzq);

        }

        private IEnumerable<T> RecorreInOrderByLeft(NodoAvl arbol)
        {
            if (arbol.HijoIzq != null)
                foreach (T var in RecorreInOrderByLeft(arbol.HijoIzq))
                    yield return var;

            yield return arbol.Valor;

            if (arbol.HijoDercho != null)
                foreach (T var in RecorreInOrderByLeft(arbol.HijoDercho))
                    yield return var;
        }

        private IEnumerable<T> RecorreInOrderByRight(NodoAvl arbol)
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

                        if (t.Padre == null) raiz = t;
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
            if (!Contains(item)) throw new InvalidOperationException("No se contiene el elemento");
            count--;
            NodoAvl toRet;
            NodoAvl toDel = GetItemInterno(raiz, item);
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
                if (x.Valor.CompareTo(x.Padre.Valor) > 0)
                    x.Padre.HijoIzq = x;
                else x.Padre.HijoDercho = x;
            }
            r.ReCalcula();
            x.ReCalcula();
            if (x.Padre != null) x.Padre.ReCalcula();
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
                if (w.Valor.CompareTo(w.Padre.Valor) > 0)
                    w.Padre.HijoIzq = w;
                else w.Padre.HijoDercho = w;
            }

            x.ReCalcula();
            r.ReCalcula();
            w.ReCalcula();
            if (w.Padre != null) w.Padre.ReCalcula();
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
            if (x.Padre != null) x.Padre.ReCalcula();
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
            if (w.Padre != null) w.Padre.ReCalcula();
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
            if (arbol == null)
            {
                return false;
            }
            else
            {
                int t = arbol.Valor.CompareTo(valor);
                if (t < 0)
                    return EstaInterno(arbol.HijoIzq, valor);
                else if (t > 0) return EstaInterno(arbol.HijoDercho, valor);
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
            if (count > 0)
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
}
