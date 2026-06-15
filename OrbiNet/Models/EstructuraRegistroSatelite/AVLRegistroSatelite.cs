namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraRegistroSatelite
{
    public class AVLRegistroSatelite : IAbstractCollection
    {
        private NodoRegistroSatelite raiz;
        private int tamano;

        public int Count => tamano;
        public bool IsEmpty => raiz == null;

        public AVLRegistroSatelite()
        {
            raiz = null;
            tamano = 0;
        }

        public void Clear()
        {
            raiz = null;
            tamano = 0;
        }

        private int GetAltura(NodoRegistroSatelite nodo)
        {
            return nodo?.Height ?? 0;
        }

        private int GetBalance(NodoRegistroSatelite nodo)
        {
            return nodo == null ? 0 : GetAltura(nodo.RightChild) - GetAltura(nodo.LeftChild);
        }

        private NodoRegistroSatelite RotacionIzquierda(NodoRegistroSatelite x)
        {
            NodoRegistroSatelite y = x.RightChild;
            NodoRegistroSatelite T2 = y.LeftChild;

            y.LeftChild = x;
            x.RightChild = T2;

            x.Height = Math.Max(GetAltura(x.LeftChild), GetAltura(x.RightChild)) + 1;
            y.Height = Math.Max(GetAltura(y.LeftChild), GetAltura(y.RightChild)) + 1;

            return y;
        }

        private NodoRegistroSatelite RotacionDerecha(NodoRegistroSatelite y)
        {
            NodoRegistroSatelite x = y.LeftChild;
            NodoRegistroSatelite T2 = x.RightChild;

            x.RightChild = y;
            y.LeftChild = T2;

            y.Height = Math.Max(GetAltura(y.LeftChild), GetAltura(y.RightChild)) + 1;
            x.Height = Math.Max(GetAltura(x.LeftChild), GetAltura(x.RightChild)) + 1;

            return x;
        }

        private NodoRegistroSatelite RotacionIzquierdaDerecha(NodoRegistroSatelite nodo)
        {
            nodo.LeftChild = RotacionIzquierda(nodo.LeftChild);
            return RotacionDerecha(nodo);
        }

        private NodoRegistroSatelite RotacionDerechaIzquierda(NodoRegistroSatelite nodo)
        {
            nodo.RightChild = RotacionDerecha(nodo.RightChild);
            return RotacionIzquierda(nodo);
        }

        public void Insertar(RegistroSatelite nuevoRegistroSatelite)
        {
            raiz = InsertarRecursivo(raiz, nuevoRegistroSatelite);
        }

        private NodoRegistroSatelite InsertarRecursivo(NodoRegistroSatelite nodo, RegistroSatelite nuevoRegistroSatelite)
        {
            if (nodo == null)
            {
                tamano++;
                return new NodoRegistroSatelite(nuevoRegistroSatelite);
            }

            int comparacion = string.Compare(nuevoRegistroSatelite.SatelliteId, nodo.Valor.SatelliteId, StringComparison.Ordinal);

            if(comparacion < 0)
            {
                nodo.LeftChild = InsertarRecursivo(nodo.LeftChild, nuevoRegistroSatelite);
            }
            else if(comparacion > 0)
            {
                nodo.RightChild = InsertarRecursivo(nodo.RightChild, nuevoRegistroSatelite);
            }
            else
            {
                return nodo;
            }

            nodo.Height = Math.Max(GetAltura(nodo.LeftChild), GetAltura(nodo.RightChild)) + 1;

            int balance = GetBalance(nodo);

            if(balance >= 2 && GetBalance(nodo.RightChild) >= 0)
            {
                return RotacionIzquierda(nodo);
            }

            if(balance <= -2 && GetBalance(nodo.LeftChild) <= 0)
            {
                return RotacionDerecha(nodo);
            }

            if(balance >= 2 && GetBalance(nodo.RightChild) < 0)
            {
                return RotacionDerechaIzquierda(nodo);
            }

            if(balance <= -2 && GetBalance(nodo.LeftChild) > 0)
            {
                return RotacionIzquierdaDerecha(nodo);
            }

            return nodo;
        }

        public void MostrarArbolVisual(){
            MostrarArbolVisualRecursivo(raiz, 0);
        }

        private void MostrarArbolVisualRecursivo(NodoRegistroSatelite nodo, int nivel)
        {
            if (nodo == null)
            {
                return;
            }

            MostrarArbolVisualRecursivo(nodo.RightChild, nivel + 1);
            Console.WriteLine(new string(' ', nivel * 4) + nodo.Valor.SatelliteId + " | " + nodo.Height);
            MostrarArbolVisualRecursivo(nodo.LeftChild, nivel + 1);
        }
    }
}