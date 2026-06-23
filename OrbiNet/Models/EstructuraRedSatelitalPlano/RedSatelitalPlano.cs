using System;

namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraRedSatelitalPlano
{
    public class RedSatelitalPlano : IAbstractCollection
    {
        private HeaderNode rowHeaders;
        private HeaderNode colHeaders;
        private int count;

        public int Count => count;
        public bool IsEmpty => rowHeaders == null && colHeaders == null;

        public void Clear()
        {
            rowHeaders = null;
            colHeaders = null;
            count = 0;
        }

        private int ObtenerMaxFila()
        {
            if (rowHeaders == null) return 0;
            
            int max = 0;
            HeaderNode actual = rowHeaders;
            while (actual != null)
            {
                if (actual.Index > max) max = actual.Index;
                actual = actual.Next;
            }
            return max;
        }

        private int ObtenerMaxColumna()
        {
            if (colHeaders == null) return 0;
            
            int max = 0;
            HeaderNode actual = colHeaders;
            while (actual != null)
            {
                if (actual.Index > max) max = actual.Index;
                actual = actual.Next;
            }
            return max;
        }

        public void Insertar(SatelitePlano nuevoSatelite)
        {
            if (BuscarNodo(nuevoSatelite.Row, nuevoSatelite.Col) != null)
            {
                throw new Exception($"Colisión: Ya existe un satélite en las coordenadas ({nuevoSatelite.Row}, {nuevoSatelite.Col})");
            }

            MatrixNode nuevoNodo = new MatrixNode(nuevoSatelite);

            HeaderNode cabeceraFila = ObtenerOCrearCabeceraFila(nuevoSatelite.Row);
            HeaderNode cabeceraCol = ObtenerOCrearCabeceraColumna(nuevoSatelite.Col);

            InsertarEnFila(cabeceraFila, nuevoNodo);
            InsertarEnColumna(cabeceraCol, nuevoNodo);

            count++;
        }

        public void Eliminar(int row, int col)
        {
            MatrixNode nodoAEliminar = BuscarNodo(row, col);
            if (nodoAEliminar == null) return;

            if (nodoAEliminar.Left != null)
            {
                nodoAEliminar.Left.Right = nodoAEliminar.Right;
            }
            else
            {
                HeaderNode cabeceraFila = ObtenerCabeceraFilaExistente(row);
                if (cabeceraFila != null) cabeceraFila.Access = nodoAEliminar.Right;
            }

            if (nodoAEliminar.Right != null)
            {
                nodoAEliminar.Right.Left = nodoAEliminar.Left;
            }

            if (nodoAEliminar.Up != null)
            {
                nodoAEliminar.Up.Down = nodoAEliminar.Down;
            }
            else
            {
                HeaderNode cabeceraCol = ObtenerCabeceraColumnaExistente(col);
                if (cabeceraCol != null) cabeceraCol.Access = nodoAEliminar.Down;
            }

            if (nodoAEliminar.Down != null)
            {
                nodoAEliminar.Down.Up = nodoAEliminar.Up;
            }

            count--;
        }

        public MatrixNode BuscarNodo(int row, int col)
        {
            HeaderNode cabeceraFila = ObtenerCabeceraFilaExistente(row);
            if (cabeceraFila == null) return null;

            MatrixNode actual = cabeceraFila.Access;
            while (actual != null && actual.Valor.Col != col)
            {
                actual = actual.Right;
            }

            if (actual != null && actual.Valor.Col == col)
            {
                return actual;
            }
            return null;
        }

        public SatelitePlano BuscarSatelite(int row, int col)
        {
            MatrixNode nodo = BuscarNodo(row, col);
            return nodo?.Valor;
        }

        private HeaderNode ObtenerOCrearCabeceraFila(int row)
        {
            if (rowHeaders == null)
            {
                rowHeaders = new HeaderNode(row);
                return rowHeaders;
            }

            if (row < rowHeaders.Index)
            {
                HeaderNode nueva = new HeaderNode(row);
                nueva.Next = rowHeaders;
                rowHeaders = nueva;
                return nueva;
            }

            HeaderNode actual = rowHeaders;
            while (actual.Next != null && actual.Next.Index <= row)
            {
                actual = actual.Next;
            }

            if (actual.Index == row) return actual;

            HeaderNode nuevaCabecera = new HeaderNode(row);
            nuevaCabecera.Next = actual.Next;
            actual.Next = nuevaCabecera;
            return nuevaCabecera;
        }

        private HeaderNode ObtenerOCrearCabeceraColumna(int col)
        {
            if (colHeaders == null)
            {
                colHeaders = new HeaderNode(col);
                return colHeaders;
            }

            if (col < colHeaders.Index)
            {
                HeaderNode nueva = new HeaderNode(col);
                nueva.Next = colHeaders;
                colHeaders = nueva;
                return nueva;
            }

            HeaderNode actual = colHeaders;
            while (actual.Next != null && actual.Next.Index <= col)
            {
                actual = actual.Next;
            }

            if (actual.Index == col) return actual;

            HeaderNode nuevaCabecera = new HeaderNode(col);
            nuevaCabecera.Next = actual.Next;
            actual.Next = nuevaCabecera;
            return nuevaCabecera;
        }

        private HeaderNode ObtenerCabeceraFilaExistente(int row)
        {
            HeaderNode actual = rowHeaders;
            while (actual != null && actual.Index != row) actual = actual.Next;
            return actual;
        }

        private HeaderNode ObtenerCabeceraColumnaExistente(int col)
        {
            HeaderNode actual = colHeaders;
            while (actual != null && actual.Index != col) actual = actual.Next;
            return actual;
        }

        private void InsertarEnFila(HeaderNode cabecera, MatrixNode nuevoNodo)
        {
            if (cabecera.Access == null)
            {
                cabecera.Access = nuevoNodo;
                return;
            }

            if (nuevoNodo.Valor.Col < cabecera.Access.Valor.Col)
            {
                nuevoNodo.Right = cabecera.Access;
                cabecera.Access.Left = nuevoNodo;
                cabecera.Access = nuevoNodo;
                return;
            }

            MatrixNode actual = cabecera.Access;
            while (actual.Right != null && actual.Right.Valor.Col < nuevoNodo.Valor.Col)
            {
                actual = actual.Right;
            }

            nuevoNodo.Right = actual.Right;
            if (actual.Right != null)
            {
                actual.Right.Left = nuevoNodo;
            }
            actual.Right = nuevoNodo;
            nuevoNodo.Left = actual;
        }

        private void InsertarEnColumna(HeaderNode cabecera, MatrixNode nuevoNodo)
        {
            if (cabecera.Access == null)
            {
                cabecera.Access = nuevoNodo;
                return;
            }

            if (nuevoNodo.Valor.Row < cabecera.Access.Valor.Row)
            {
                nuevoNodo.Down = cabecera.Access;
                cabecera.Access.Up = nuevoNodo;
                cabecera.Access = nuevoNodo;
                return;
            }

            MatrixNode actual = cabecera.Access;
            while (actual.Down != null && actual.Down.Valor.Row < nuevoNodo.Valor.Row)
            {
                actual = actual.Down;
            }

            nuevoNodo.Down = actual.Down;
            if (actual.Down != null)
            {
                actual.Down.Up = nuevoNodo;
            }
            actual.Down = nuevoNodo;
            nuevoNodo.Up = actual;
        }

        public string GenerarTablaDinamica()
        {
            if (IsEmpty) return "La red satelital está vacía. No hay nodos en órbita.";

            int maxFilas = ObtenerMaxFila();
            int maxColumnas = ObtenerMaxColumna();

            string diagrama = $"Matriz Ortogonal (Red Satelital) - Dimensión descubierta: {maxFilas}x{maxColumnas}\n";
            diagrama += new string('-', (maxColumnas * 16) + 1) + "\n";

            for (int r = 1; r <= maxFilas; r++)
            {
                for (int c = 1; c <= maxColumnas; c++)
                {
                    MatrixNode nodo = BuscarNodo(r, c);
                    if (nodo != null)
                    {
                        diagrama += $"| {nodo.Valor.Id,-10} "; 
                    }
                    else
                    {
                        diagrama += "|    ----    ";
                    }
                }
                diagrama += "|\n";
                diagrama += new string('-', (maxColumnas * 16) + 1) + "\n";
            }

            return diagrama;
        }
    }
}