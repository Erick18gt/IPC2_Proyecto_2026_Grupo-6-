namespace OrbiNet.Services.Ingestion;
using System.Text.RegularExpressions;

public class RegexValidtorService
{
    // validaciones de datos individuales
    public bool ValidarSateliteId(string id)
    {   if(string.IsNullOrWhiteSpace(id))
        {
            return false;
        }
        return Regex.IsMatch(id, @"^SAT-(ECU|POL)-\d{4}$");
    }
    public bool ValidarAntenaId(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }
        return Regex.IsMatch(id, @"^ANT-[A-Z]{3}-\d{4}$");
    }
    public bool ValidarPolarId(string id)
    {   if(string.IsNullOrWhiteSpace(id))
        {
            return false;
        }
        return Regex.IsMatch(id, @"^POLAR-[A-Z]+-[A-Z]$");


    }
    public bool ValidarNombre(string nombre)
    {
        return !string.IsNullOrWhiteSpace(nombre);
    }
    public bool ValidarIpv4(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
        {
            return false;
        }
        return Regex.IsMatch(ip, @"^(?:(?:25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])\.){3}(?:25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])$");
    }
    public bool ValidarCoordenadas(string coordenadas)
    {
        if (string.IsNullOrWhiteSpace(coordenadas))
        {
            return false;
        }
        return Regex.IsMatch(coordenadas, @"^-?\d{1,2}\.\d{4,6},-?\d{1,3}\.\d{4,6}$");
    }
    public bool ValidarFrecuencia(string frecuencia)
    {
        if (string.IsNullOrWhiteSpace(frecuencia))
        {
            return false;
        }
        if(!double.TryParse(frecuencia, out double valor))
        {
            return false;
        }

        return valor > 0;
    }
    public bool ValidarSateliteEcuatorial(string id, string nombre, string ip)
    {
        return ValidarSateliteId(id)
            && ValidarNombre(nombre)
            && ValidarIpv4(ip);
    }

    public bool ValidarSatelitePolar(string id, string nombre, string frecuencia)
    {
        return ValidarSateliteId(id)
            && ValidarNombre(nombre)
            && ValidarFrecuencia(frecuencia);
    }

    public bool ValidarAntenaTerrestre(string id, string nombre, string coordenadas, string ipNodo)
    {
        return ValidarAntenaId(id)
            && ValidarNombre(nombre)
            && ValidarCoordenadas(coordenadas)
            && ValidarIpv4(ipNodo);
    }

}
