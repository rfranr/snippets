<?php
	//TODO: get the best candidate
	$minimal_bound = 44;
	
	$source_string = "api/_login_/_authenticate_\napi/{empreses}/delete/{idEmpresa}\napi/{login}/{authenticate}/{username}/{password}\napi/backoffice/list/{businessObject}\napi/backoffice/update\napi/caracteristica/get/{codi}\napi/codisqr/assignacioqrs/\napi/codisqr/assignacioregistre/{codiRegistre}\napi/codisqr/assignacioregistre/codiregistre/{codiRegistre}/codiQr/{codiQR}\napi/codisqr/disponibles\napi/codisqr/exportar/{cantidad}\napi/codisqr/exportarduplicats\napi/codisqr/exportarlote/id/{id}\napi/codisqr/generar/{cantidad}\napi/codisqr/listlots\napi/codisqr/lliures\napi/codisqr/lotrecepcionat/{idlote}\napi/codisqr/registreid/{codiQr}\napi/codisqr/unlliure\napi/documentacio/add\napi/documentacio/get/{idDocument}\napi/duplicat/gestio\napi/duplicat/search\napi/empreses/add\napi/empreses/get/{idEmpresa}\napi/empreses/list\napi/empreses/search\napi/empreses/update\napi/exportar/consultaregistres\napi/helper/list/{businessObject}/{param?}/{param1?}/{param2?}/{param3?}\napi/login/claims\napi/login/info/{idUser}\napi/login/info2/{idUser}\napi/mail/assegurancavencut/\napi/mail/disposendadhesius/\napi/mail/minimoqrs/\napi/mail/nocomplimentrequisits/\napi/mail/propervencimentasseguranca/\napi/mail/requeririnformacio/\napi/registresvehicles/action/\napi/registresvehicles/imprimirdocumentacio/\napi/registresvehicles/pagat\napi/registresvehicles/search\napi/registresvehicles/updateall\napi/sollicitudregistre/{businessObject}/{codi}/\napi/sollicitudregistre/add\napi/sollicitudregistre/adddocuments\napi/tipusvehicles/{addcaracterisca}\napi/tipusvehicles/addvehicle\napi/tipusvehicles/associarcaracteristica\napi/tipusvehicles/caracteristiques/classe/{classe}/tipus/{tipus?}\napi/tipusvehicles/delete\napi/tipusvehicles/listnoms/{value?}\napi/tipusvehicles/remove\napi/tipusvehicles/removevehicle\napi/tipusvehicles/update\napi/tipusvehicles/updatevehicles";

	$search_string = "http://172.17.17.135:8081/api/v1/backoffice/action\nhttp://172.17.17.135:8081/api/v1/backoffice/add\nhttp://172.17.17.135:8081/api/v1/backoffice/delete\nhttp://172.17.17.135:8081/api/v1/backoffice/list\nhttp://172.17.17.135:8081/api/v1/backoffice/search\nhttp://172.17.17.135:8081/api/v1/backoffice/udpate\nhttp://172.17.17.135:8081/api/v1/consultavehicles/action\nhttp://172.17.17.135:8081/api/v1/consultavehicles/get\nhttp://172.17.17.135:8081/api/v1/consultavehicles/search\nhttp://172.17.17.135:8081/api/v1/duplicats/action\nhttp://172.17.17.135:8081/api/v1/duplicats/get\nhttp://172.17.17.135:8081/api/v1/duplicats/search\nhttp://172.17.17.135:8081/api/v1/duplicats/update\nhttp://172.17.17.135:8081/api/v1/empreses/action\nhttp://172.17.17.135:8081/api/v1/empreses/add\nhttp://172.17.17.135:8081/api/v1/empreses/delete\nhttp://172.17.17.135:8081/api/v1/empreses/get\nhttp://172.17.17.135:8081/api/v1/empreses/list\nhttp://172.17.17.135:8081/api/v1/empreses/search\nhttp://172.17.17.135:8081/api/v1/empreses/update\nhttp://172.17.17.135:8081/api/v1/fitxavehicle/action\nhttp://172.17.17.135:8081/api/v1/fitxavehicle/get\nhttp://172.17.17.135:8081/api/v1/fitxavehicle/update\nhttp://172.17.17.135:8081/api/v1/helper/list\nhttp://172.17.17.135:8081/api/v1/helper/qr\nhttp://172.17.17.135:8081/api/v1/login/authenticate\nhttp://172.17.17.135:8081/api/v1/registresvehicles/action\nhttp://172.17.17.135:8081/api/v1/registresvehicles/add\nhttp://172.17.17.135:8081/api/v1/registresvehicles/delete\nhttp://172.17.17.135:8081/api/v1/registresvehicles/get\nhttp://172.17.17.135:8081/api/v1/registresvehicles/list\nhttp://172.17.17.135:8081/api/v1/registresvehicles/search\nhttp://172.17.17.135:8081/api/v1/registresvehicles/update\nhttp://172.17.17.135:8081/api/v1/tipusvehicles/addcaracteristica\nhttp://172.17.17.135:8081/api/v1/tipusvehicles/addvehicle\nhttp://172.17.17.135:8081/api/v1/tipusvehicles/associarcaracteristica\nhttp://172.17.17.135:8081/api/v1/tipusvehicles/caracteristiques\nhttp://172.17.17.135:8081/api/v1/tipusvehicles/delete\nhttp://172.17.17.135:8081/api/v1/tipusvehicles/remove\nhttp://172.17.17.135:8081/api/v1/tipusvehicles/update";



	$in = explode("\n", $source_string);
	$match = explode("\n", $search_string);
	$candidates = array ();
	foreach ($in as $key => $value1) {
		foreach ($match as $key => $value2) {
			$s = similar_text($value1, $value2, $percent);
			if ( $percent >= $minimal_bound) {
				//$candidates[$key] = 
				echo "percent :=" . $percent ; 
				echo "\t" ;
				echo "find :=" . $value1 ;
				echo "\t" ;
				echo "match :=" . $value2 ; 
				echo "\t = " ;
				echo "result :=" . $s;
				echo "\n" ;
			}
		}
	}

?>
