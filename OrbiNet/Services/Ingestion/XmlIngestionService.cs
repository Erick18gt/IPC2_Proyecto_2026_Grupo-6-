namespace OrbiNet.Services.Ingestion;

using System.Xml;
using IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraRegistroSatelite;
using IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraLogAuditoria;

public class XmlIngestionService
{
    private readonly RegexValidtorService validator = new RegexValidtorService();
    private readonly AVLRegistroSatelite catalogoSatelites = new AVLRegistroSatelite();
    private readonly ListaLogAuditoria logs = new ListaLogAuditoria();

    private RegistroSatelite[] satelitesTemporales = new RegistroSatelite[100];
    private int totalSatelitesTemporales = 0;

    public IngestionResult CargarXml(string xmlContent)
    {
        totalSatelitesTemporales = 0;

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
            totalSatelitesTemporales = 0;
            return resultadoPolares;
        }

        IngestionResult resultadoAntenas = ValidarAntenasTerrestres(doc, ref procesados);
        if (!resultadoAntenas.Success)
        {
            totalSatelitesTemporales = 0;
            return resultadoAntenas;
        }

        ConfirmarCarga();

        RegistrarInfo("XML validado correctamente. Carga confirmada.");

        return new IngestionResult
        {
            Success = true,
            Message = "XML validado correctamente. Satélites polares insertados en AVL.",
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

            RegistrarInfo($"Satélite ecuatorial validado: {id}");
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

                RegistrarInfo($"Satélite polar validado temporalmente: {id}");
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

            RegistrarInfo($"Antena terrestre validada: {id}");
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

        totalSatelitesTemporales = 0;
    }

    private void RegistrarInfo(string mensaje)
    {
        logs.InsertarLog(new LogAuditoria("INFO", mensaje));
    }

    private IngestionResult CrearError(string mensaje, int procesados)
    {
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
}