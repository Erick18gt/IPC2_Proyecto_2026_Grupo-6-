using System;
using System.Collections.Generic;

namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.LogAuditoria_ListaEnlazadaSimple
{
    public class ListaLogAuditoria
    {
        private NodoLogAuditoria? cabeza;
        private int tamano;

        public ListaLogAuditoria()
        {
            cabeza = null;
            tamano = 0;
        }

        public int Tamano => tamano;

        public bool EstaVacia => cabeza == null;

        public void InsertarAlFinal(LogAuditoria log)
        {
            NodoLogAuditoria nuevoNodo = new NodoLogAuditoria(log);
            if (EstaVacia)
            {
                cabeza = nuevoNodo;
            }
            else
            {
                NodoLogAuditoria actual = cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevoNodo;
            }
        }
    }
}