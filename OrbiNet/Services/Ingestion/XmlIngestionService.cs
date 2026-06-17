namespace OrbiNet.Services.Ingestion;

using System.Xml;

public class XmlIngestionService
{
    private readonly RegexValidtorService validator = new RegexValidtorService();

    public IngestionResult CargarXml(string xmlContent)
    {
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
            return resultadoPolares;
        }

        IngestionResult resultadoAntenas = ValidarAntenasTerrestres(doc, ref procesados);
        if (!resultadoAntenas.Success)
        {
            return resultadoAntenas;
        }

        return new IngestionResult
        {
            Success = true,
            Message = "XML validado correctamente. La carga puede continuar.",
            ProcessedNodes = procesados
        };
    }

    private IngestionResult ValidarSatelitesEcuatoriales(XmlDocument doc, ref int procesados)
    {
        XmlNodeList? satelites = doc.SelectNodes("//constelaciones_ecuatoriales/satelite");

        if (satelites == null)
        {
            return CrearError("No se pudo leer la sección de satélites ecuatoriales.", procesados);
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

            procesados++;
        }

        return CrearExitoTemporal(procesados);
    }

    private IngestionResult ValidarSatelitesPolares(XmlDocument doc, ref int procesados)
    {
        XmlNodeList? polares = doc.SelectNodes("//orbitas_polares/polar");

        if (polares == null)
        {
            return CrearError("No se pudo leer la sección de órbitas polares.", procesados);
        }

        foreach (XmlNode polar in polares)
        {
            string polarId = polar.Attributes?["id"]?.Value ?? "";

            if (!validator.ValidarPolarId(polarId))
            {
                return CrearError(
                    $"Órbita polar inválida. ID='{polarId}'.",
                    procesados
                );
            }

            XmlNodeList? satelites = polar.SelectNodes("satelite");

            if (satelites == null)
            {
                return CrearError(
                    $"No se pudieron leer satélites dentro de la órbita polar '{polarId}'.",
                    procesados
                );
            }

            foreach (XmlNode satelite in satelites)
            {
                string id = satelite.Attributes?["id"]?.Value ?? "";
                string nombre = satelite.SelectSingleNode("nombre")?.InnerText ?? "";
                string frecuencia = satelite.SelectSingleNode("frecuencia")?.InnerText ?? "";

                if (!validator.ValidarSatelitePolar(id, nombre, frecuencia))
                {
                    return CrearError(
                        $"Satélite polar inválido. ID='{id}', Nombre='{nombre}', Frecuencia='{frecuencia}'.",
                        procesados
                    );
                }

                procesados++;
            }
        }

        return CrearExitoTemporal(procesados);
    }

    private IngestionResult ValidarAntenasTerrestres(XmlDocument doc, ref int procesados)
    {
        XmlNodeList? antenas = doc.SelectNodes("//antenas_terrestres/antena");

        if (antenas == null)
        {
            return CrearError("No se pudo leer la sección de antenas terrestres.", procesados);
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

            procesados++;
        }

        return CrearExitoTemporal(procesados);
    }

    private IngestionResult CrearError(string mensaje, int procesados)
    {
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