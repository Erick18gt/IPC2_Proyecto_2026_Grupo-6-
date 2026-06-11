namespace OrbiNet.Services.Ingestion;

using System.Xml;

public class XmlIngestionService
{ public void CargarXml(string XmlContent)
    { XmlDocument doc = new XmlDocument();
        // Evitar la resolución de entidades externas para prevenir ataques XXE

        doc.XmlResolver = null;

        //Cargamos el contenido del XML en el XmlDocument
        doc.LoadXml(XmlContent);
        //Llamamos a los metodos para leer cada nodo del XML
        LeerSatelitesEcuatoriales(doc);
        LeerSatelitesPolares(doc);
        LeerAntenasTerrestres(doc);
    }

    private void LeerSatelitesEcuatoriales(XmlDocument doc)
    {   
        XmlNodeList? satelites = doc.SelectNodes("//constelaciones_ecuatoriales/satelite");

        if (satelites == null) {
            return;
        }
        foreach(XmlNode satelite in satelites)
        {
           string id = satelite.Attributes?["id"]?.Value ?? "";
           string nombre = satelite.SelectSingleNode("nombre")?.InnerText ?? "";
           string ip = satelite.SelectSingleNode("enlace_ip")?.InnerText ?? "";

            Console.WriteLine("SATÉLITE ECUATORIAL");
            Console.WriteLine($"ID: {id}");
            Console.WriteLine($"Nombre: {nombre}");
            Console.WriteLine($"IP: {ip}");
            Console.WriteLine();
        }
        
    } 
    private void LeerSatelitesPolares(XmlDocument doc)
    {   XmlNodeList? satelites = doc.SelectNodes("//orbitas_polares/polar/satelite");
        if (satelites == null)
        {
            return;
        }
        foreach (XmlNode satelite in satelites)
        {
            string id = satelite.Attributes?["id"]?.Value ?? "";
            string nombre = satelite.SelectSingleNode("nombre")?.InnerText ?? "";
            string frecuencia = satelite.SelectSingleNode("frecuencia")?.InnerText ?? "";
            Console.WriteLine("SATÉLITE POLAR");
            Console.WriteLine($"ID: {id}");
            Console.WriteLine($"Nombre: {nombre}");
            Console.WriteLine($"Frecuencia: {frecuencia}");
            Console.WriteLine();
        }

    }
    private void LeerAntenasTerrestres(XmlDocument doc)
    {
        XmlNodeList? antenas = doc.SelectNodes("//antenas_terrestres/antena");
        if (antenas == null) {
            return;
        }
        foreach(XmlNode antena in antenas)
        {
            string id = antena.Attributes?["id"]?.Value ?? "";
            string nombre = antena.SelectSingleNode("nombre")?.InnerText ?? "";
            string coordenadas = antena.SelectSingleNode("coordenadas")?.InnerText ?? "";
            string ipNodo = antena.SelectSingleNode("ip_nodo")?.InnerText ?? "";
            Console.WriteLine("ANTENA TERRESTRE");
            Console.WriteLine($"ID: {id}");
            Console.WriteLine($"Nombre: {nombre}");
            Console.WriteLine($"Coordenadas: {coordenadas}");
            Console.WriteLine($"IP Nodo: {ipNodo}");
            Console.WriteLine();
        
        }


    }
}





