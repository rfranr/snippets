#/bin/bash

## usage example:
# $ { cat << EOF
#	http://myexample.exampe.com/endpoint
#	EOF
#  } | ./discover_methods.sh
#  
#  usage example
#  
# $ echo -e "https://bhagavadgita.io/api/v1 \n\
# https://api.thecatapi.com/v1/categories" | ./discover_methods.sh
# 
# https://bhagavadgita.io/api/v1
# GET 404 Not Found
# POST 404 Not Found
# PUT 404 Not Found
# DELETE 404 Not Found
# https://api.thecatapi.com/v1/categories
# GET 200 OK
# POST 405 Method Not Allowed
# PUT 405 Method Not Allowed
# DELETE 405 Method Not Allowed

curl_path='/c/Program\ Files/Git/mingw64/bin/curl.exe'

http_error_codes=(
		[200]="OK"
		[201]="Created"
		[202]="Accepted"
		[203]="Non-Authoritative Information (since HTTP/1.1)"
		[204]="No Content"
		[205]="Reset Content"
		[206]="Partial Content (RFC 7233)"
		[207]="Multi-Status (WebDAV; RFC 4918)"
		[208]="Already Reported (WebDAV; RFC 5842)"
		[226]="IM Used (RFC 3229)"
		[300]="Multiple Choices"
		[301]="Moved Permanently"
		[302]="Found (Previously \"Moved temporarily\")"
		[303]="See Other (since HTTP/1.1)"
		[304]="Not Modified (RFC 7232)"
		[305]="Use Proxy (since HTTP/1.1)"
		[306]="Switch Proxy"
		[307]="Temporary Redirect (since HTTP/1.1)"
		[308]="Permanent Redirect (RFC 7538)"
		[400]="Bad Request"
		[401]="Unauthorized (RFC 7235)"
		[402]="Payment Required"
		[403]="Forbidden"
		[404]="Not Found"
		[405]="Method Not Allowed"
		[406]="Not Acceptable"
		[407]="Proxy Authentication Required (RFC 7235)"
		[408]="Request Timeout"
		[409]="Conflict"
		[410]="Gone"
		[411]="Length Required"
		[412]="Precondition Failed (RFC 7232)"
		[413]="Payload Too Large (RFC 7231)"
		[414]="URI Too Long (RFC 7231)"
		[415]="Unsupported Media Type"
		[416]="Range Not Satisfiable (RFC 7233)"
		[417]="Expectation Failed"
		[418]="I'm a teapot (RFC 2324, RFC 7168)"
		[421]="Misdirected Request (RFC 7540)"
		[422]="Unprocessable Entity (WebDAV; RFC 4918)"
		[423]="Locked (WebDAV; RFC 4918)"
		[424]="Failed Dependency (WebDAV; RFC 4918)"
		[426]="Upgrade Required"
		[428]="Precondition Required (RFC 6585)"
		[429]="Too Many Requests (RFC 6585)"
		[431]="Request Header Fields Too Large (RFC 6585)"
		[451]="Unavailable For Legal Reasons (RFC 7725)"
		[500]="Internal Server Error"
		[501]="Not Implemented"
		[502]="Bad Gateway"
		[503]="Service Unavailable"
		[504]="Gateway Timeout"
		[505]="HTTP Version Not Supported"
		[506]="Variant Also Negotiates (RFC 2295)"
		[507]="Insufficient Storage (WebDAV; RFC 4918)"
		[508]="Loop Detected (WebDAV; RFC 5842)"
		[510]="Not Extended (RFC 2774)"
		[511]="Network Authentication Required (RFC 6585)"
)

# read stdin or file
while read line
do
  echo -e "$line"
  for method in GET POST PUT DELETE ;
  do
  	curl="$curl_path -s -o  /dev/null -w "%{http_code}" -X $method $line"
    result=`eval "$curl"`
    echo $method $result ${http_error_codes[$result]}
  done
done < "${1-/dev/stdin}"


