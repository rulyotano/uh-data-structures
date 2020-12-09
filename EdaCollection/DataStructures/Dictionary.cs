using System;

namespace EdaCollection.DataStructures
{
    public class Dictionary<K, V> : System.Collections.Generic.IEnumerable<V> where K : IComparable<K>
    {
        private Avl<NodoDiccionario> avl = new Avl<NodoDiccionario>();

        public bool Contains(K item)
        {
            return avl.Contains(new NodoDiccionario(item, default(V)));
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

        private class NodoDiccionario : IComparable<NodoDiccionario>
        {
            public V Valor;
            public K Key;

            public NodoDiccionario(K key, V valor)
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
