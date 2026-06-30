namespace OrbiNet.Services.Ingestion;

using System.Xml;
using IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraRegistroSatelite;
using IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraLogAuditoria;
using IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraRedSatelitalPlano;

public class XmlIngestionService
{
    private readonly TransactionScope transaction = new TransactionScope();
    private readonly GraphvizRenderService graphviz = new GraphvizRenderService();
    private readonly RegexValidtorService validator = new RegexValidtorService();

    private readonly AVLRegistroSatelite catalogoSatelites = new AVLRegistroSatelite();
    private readonly ListaLogAuditoria logs = new ListaLogAuditoria();
    private readonly RedSatelitalPlano redPlano = new RedSatelitalPlano();

    private RegistroSatelite[] satelitesTemporales = new RegistroSatelite[100];
    private int totalSatelitesTemporales = 0;

    private SatelitePlano[] redTemporal = new SatelitePlano[100];
    private int totalRedTemporal = 0;

    public IngestionResult CargarXml(string xmlContent)
    {
        transaction.Begin();
        totalSatelitesTemporales = 0;
        totalRedTemporal = 0;

        if (string.IsNullOrWhiteSpace(xmlContent))
        {
            return CrearError("El contenido XML está vacío.", 0);
        }

        XmlDocument doc = new XmlDocument();
        doc.XmlResolver = null;

        try
        {
            doc.LoadXml(xmlContent);
        }
        catch
        {
            return CrearError("El XML no tiene una estructura válida.", 0);
        }

        if (doc.DocumentElement == null || doc.DocumentElement.Name != "orbitnet")
        {
            return CrearError("El nodo raíz debe ser <orbitnet>.", 0);
        }

        int procesados = 0;

        IngestionResult resultadoEcuatoriales = ValidarSatelitesEcuatoriales(doc, ref procesados);
        if (!resultadoEcuatoriales.Success)
        {
            return resultadoEcuatoriales;
        }

        IngestionResult resultadoPolares = ValidarSatelitesPolares(doc, ref procesados);
        if (!resultadoPolares.Success)
        {
            LimpiarTemporales();
            return resultadoPolares;
        }

        IngestionResult resultadoAntenas = ValidarAntenasTerrestres(doc, ref procesados);
        if (!resultadoAntenas.Success)
        {
            LimpiarTemporales();
            return resultadoAntenas;
        }

        ConfirmarCarga();

        RegistrarInfo("XML validado correctamente. Carga confirmada.");
        transaction.Commit();

        return new IngestionResult
        {
            Success = true,
            Message = "XML validado correctamente. Satélites polares insertados en AVL y red satelital insertada en matriz ortogonal.",
            ProcessedNodes = procesados
        };
    }

    private IngestionResult ValidarSatelitesEcuatoriales(XmlDocument doc, ref int procesados)
    {
        XmlNodeList? satelites = doc.SelectNodes("//constelaciones_ecuatoriales/satelite");

        if (satelites == null || satelites.Count == 0)
        {
            return CrearError("Debe existir al menos un satélite ecuatorial.", procesados);
        }

        foreach (XmlNode satelite in satelites)
        {
            string id = satelite.Attributes?["id"]?.Value ?? "";
            string nombre = satelite.SelectSingleNode("nombre")?.InnerText ?? "";
            string ip = satelite.SelectSingleNode("enlace_ip")?.InnerText ?? "";

            if (!validator.ValidarSateliteEcuatorial(id, nombre, ip))
            {
                return CrearError(
                    $"Satélite ecuatorial inválido. ID='{id}', Nombre='{nombre}', IP='{ip}'.",
                    procesados
                );
            }

            if (totalRedTemporal >= redTemporal.Length)
            {
                return CrearError("Se superó el límite temporal de nodos de red permitidos.", procesados);
            }

            int columna = ExtraerNumeroFinal(id);
            SatelitePlano nodoPlano = new SatelitePlano(1, columna, id, ip);

            redTemporal[totalRedTemporal] = nodoPlano;
            totalRedTemporal++;

            RegistrarInfo($"Satélite ecuatorial validado temporalmente para matriz: {id}");
            procesados++;
        }

        return CrearExitoTemporal(procesados);
    }

    private IngestionResult ValidarSatelitesPolares(XmlDocument doc, ref int procesados)
    {
        XmlNodeList? polares = doc.SelectNodes("//orbitas_polares/polar");

        if (polares == null || polares.Count == 0)
        {
            return CrearError("Debe existir al menos una órbita polar.", procesados);
        }

        foreach (XmlNode polar in polares)
        {
            string polarId = polar.Attributes?["id"]?.Value ?? "";

            if (!validator.ValidarPolarId(polarId))
            {
                return CrearError($"Órbita polar inválida. ID='{polarId}'.", procesados);
            }

            XmlNodeList? satelites = polar.SelectNodes("satelite");

            if (satelites == null || satelites.Count == 0)
            {
                return CrearError(
                    $"Debe existir al menos un satélite dentro de la órbita polar '{polarId}'.",
                    procesados
                );
            }

            foreach (XmlNode satelite in satelites)
            {
                string id = satelite.Attributes?["id"]?.Value ?? "";
                string nombre = satelite.SelectSingleNode("nombre")?.InnerText ?? "";
                string frecuenciaTexto = satelite.SelectSingleNode("frecuencia")?.InnerText ?? "";

                if (!validator.ValidarSatelitePolar(id, nombre, frecuenciaTexto))
                {
                    return CrearError(
                        $"Satélite polar inválido. ID='{id}', Nombre='{nombre}', Frecuencia='{frecuenciaTexto}'.",
                        procesados
                    );
                }

                if (totalSatelitesTemporales >= satelitesTemporales.Length)
                {
                    return CrearError("Se superó el límite temporal de satélites polares permitidos.", procesados);
                }

                double frecuencia = double.Parse(frecuenciaTexto);
                RegistroSatelite registro = new RegistroSatelite(id, nombre, frecuencia);

                satelitesTemporales[totalSatelitesTemporales] = registro;
                totalSatelitesTemporales++;

                RegistrarInfo($"Satélite polar validado temporalmente para AVL: {id}");
                procesados++;
            }
        }

        return CrearExitoTemporal(procesados);
    }

    private IngestionResult ValidarAntenasTerrestres(XmlDocument doc, ref int procesados)
    {
        XmlNodeList? antenas = doc.SelectNodes("//antenas_terrestres/antena");

        if (antenas == null || antenas.Count == 0)
        {
            return CrearError("Debe existir al menos una antena terrestre.", procesados);
        }

        foreach (XmlNode antena in antenas)
        {
            string id = antena.Attributes?["id"]?.Value ?? "";
            string nombre = antena.SelectSingleNode("nombre")?.InnerText ?? "";
            string coordenadas = antena.SelectSingleNode("coordenadas")?.InnerText ?? "";
            string ipNodo = antena.SelectSingleNode("ip_nodo")?.InnerText ?? "";

            if (!validator.ValidarAntenaTerrestre(id, nombre, coordenadas, ipNodo))
            {
                return CrearError(
                    $"Antena terrestre inválida. ID='{id}', Nombre='{nombre}', Coordenadas='{coordenadas}', IP='{ipNodo}'.",
                    procesados
                );
            }

            if (totalRedTemporal >= redTemporal.Length)
            {
                return CrearError("Se superó el límite temporal de nodos de red permitidos.", procesados);
            }

            int fila = ObtenerFilaDesdeCoordenadas(coordenadas);
            int columna = ObtenerColumnaDesdeCoordenadas(coordenadas);

            SatelitePlano nodoPlano = new SatelitePlano(fila, columna, id, ipNodo);

            redTemporal[totalRedTemporal] = nodoPlano;
            totalRedTemporal++;

            RegistrarInfo($"Antena terrestre validada temporalmente para matriz: {id}");
            procesados++;
        }

        return CrearExitoTemporal(procesados);
    }

    private void ConfirmarCarga()
    {
        for (int i = 0; i < totalSatelitesTemporales; i++)
        {
            catalogoSatelites.Insertar(satelitesTemporales[i]);
            RegistrarInfo($"Satélite polar insertado en AVL: {satelitesTemporales[i].SatelliteId}");
            satelitesTemporales[i] = null;
        }

        for (int i = 0; i < totalRedTemporal; i++)
        {
            redPlano.Insertar(redTemporal[i]);
            RegistrarInfo($"Nodo insertado en matriz ortogonal: {redTemporal[i].Id}");
            redTemporal[i] = null;
        }

        totalSatelitesTemporales = 0;
        totalRedTemporal = 0;
    }

    private void LimpiarTemporales()
    {
        for (int i = 0; i < totalSatelitesTemporales; i++)
        {
            satelitesTemporales[i] = null;
        }

        for (int i = 0; i < totalRedTemporal; i++)
        {
            redTemporal[i] = null;
        }

        totalSatelitesTemporales = 0;
        totalRedTemporal = 0;
    }
    private int ObtenerFilaDesdeCoordenadas(string coordenadas)
    {
        string[] partes = coordenadas.Split(',');

        if (partes.Length != 2)
        {
            return 0;
        }

        if (double.TryParse(partes[0], out double fila))
        {
            return (int)fila;
        }

        return 0;
    }

    private int ObtenerColumnaDesdeCoordenadas(string coordenadas)
    {
        string[] partes = coordenadas.Split(',');

        if (partes.Length != 2)
        {
            return 0;
        }

        if (double.TryParse(partes[1], out double columna))
        {
            return (int)columna;
        }

        return 0;
    }

    private int ExtraerNumeroFinal(string id)
    {
        string numeros = "";

        for (int i = id.Length - 1; i >= 0; i--)
        {
            if (char.IsDigit(id[i]))
            {
                numeros = id[i] + numeros;
            }
            else
            {
                break;
            }
        }

        if (int.TryParse(numeros, out int resultado))
        {
            return resultado;
        }

        return 0;
    }

    private void RegistrarInfo(string mensaje)
    {
        logs.InsertarLog(new LogAuditoria("INFO", mensaje));
    }

    private IngestionResult CrearError(string mensaje, int procesados)
    {
        transaction.Rollback(mensaje);
        logs.InsertarLog(new LogAuditoria("ERROR", mensaje));

        return new IngestionResult
        {
            Success = false,
            Message = mensaje,
            ProcessedNodes = procesados
        };
    }

    private IngestionResult CrearExitoTemporal(int procesados)
    {
        return new IngestionResult
        {
            Success = true,
            Message = "Validación parcial correcta.",
            ProcessedNodes = procesados
        };
    }

    public LogAuditoria[] ObtenerLogs()
    {
        return logs.Recorrer();
    }

    public string GenerarDotLogs()
    {
        return graphviz.GenerarDotLogs(logs.Recorrer());
    }

    public string GenerarDotResultado(IngestionResult resultado)
    {
        return graphviz.GenerarDotResultado(resultado);
    }

    public string ObtenerEstadoTransaccion()
    {
        return transaction.ObtenerEstado();
    }

    public string GenerarTablaRedSatelital()
    {
        return redPlano.GenerarTablaDinamica();
    }
}