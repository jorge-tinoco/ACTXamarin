* ARCHIVOS DE CONFIGURACIÓN POR AMBIENTES

Dentro de la carpeta Resources/Configuration se encuentran archivos con el mismo juego de 
confifuraciones para cada ambiente en formato json. El nombre de estos archivos sigue el formato de 
config_AMBIENTE.txt, donde ambiente hace referencia al ambiente al que apuntan las configuraciones. 

A continuación se describe el proposito de cada archivo:
- config_dev.txt: configuración para utilizar mientras se está en desarrollo.
- config_lab.txt: configuración para utilizar contra ambientes de lab.
- config_tl.txt: configuración para utilizar contra ambientes de tl.
- config_ti.txt: configuración para utilizar contra ambientes de ti.
- config_prod.txt: configuración para utilizar contra ambientes de producción.



* CONFIGURAR CUAL ES EL ARCHIVO DE CONFIGURACIÓN ACTUAL

Se debe modificar el contenido del archivo "Resources/Configuration/current_config.txt". 
Los valores posibles son los siguientes: "dev", "lab", "tl", "ti", "prod".

Ejemplo: Si queremos que nuestra aplicación utilice las configuraciones de producción 
el contenido del archivo current_config.txt debería ser "prod" (sin las comillas).

El funcionmiento es el siguiente, cuando la aplicación abre obtiene el ambiente que se 
quiere utilizar desde el archivo current_config.txt, luego con este dato obtiene el archivo 
deseado que contiene todas las configuraciones. 



* USO DE LAS CONFIGURACIONES DENTRO DE LA APLICACIÓN

Para cada valor que va a contener el archivo de configuración se crea una constante dentro 
del archivo Resources/Configuration/ConfigurationKeys.cs. 
Ejemplo:
public const string  TMAP_ProxyUrl_KEY = "TMAP_ProxyUrl";
public const string  LOGGER_Url_KEY="LOGGER_Url";

A continuación se muestra como obtener el valor que contiene cada una de estas configuraciones.

Ejemplo:
var proxyUrl = SICRAMConfiguration.GetInstance().Get(ConfigurationKeys.TMAP_ProxyUrl_KEY);
var logUrl = SICRAMConfiguration.GetInstance().Get(ConfigurationKeys.LOGGER_Url_KEY);