/*
*
*    over: https://openlibrary.org/developers/api
*/
/*
avneesh05
cutmuetia
marmorag
https://openlibrary.org/subjects/person:immanuel_kant_(1724-1804)


http://openlibrary.org/people/george/lists.json
http://openlibrary.org/people/marmorag/lists.json

http://openlibrary.org/search/lists.json?q=book&limit=20&offset=0


http://openlibrary.org/people/george08/lists.json
*/


const request = require('request-promise');
const jp = require('jsonpath');

const urls = [];

function Result (url, data ) {
    return ( url, data )
}

const results = []

let i=0;
let totalNumRequests = 30
for ( i=0; i<totalNumRequests; i++) {
    let limit = 20;
    let offset = limit * i
    let url = `http://openlibrary.org/search/lists.json?q=book&limit=${limit}&offset=${offset}`;
    urls.push (url)
}

console.dir ( urls )

let options =  {json:true}
//const promises = urls.map(url => request(url, options));

let numRun = 0;
let numRequestsFetched = 0
let numRequestsFetchedKO = 0
let numberOfConcurrentRequests = 15

let currentRequests = ""

function maleRequests (urls) {
    while ( urls.length > 0 && numRun < numberOfConcurrentRequests) {
        let url = urls.shift()
        currentRequests += url + ",";
        numRun ++
        request(url, options)
        .then ( (data) =>{
            numRun--
            numRequestsFetched ++
            maleRequests (urls);
            
            currentRequests = currentRequests.replace(url + ",", "");
            results.push ( Result(url, data) )

            //console.dir ( "------------------------------------------" )
            //console.dir ( url )
            //console.dir ( data )
            //console.dir ( "------------------------------------------" )
        })
        .catch ( (error) => {
            numRequestsFetchedKO ++;
        })
    } 
}

maleRequests (urls)

function log () {
    process.stdout.clearLine();
    process.stdout.cursorTo(0);

    process.stdout.write ( " :: Running Requests = " + numRun )
    process.stdout.write ( " :: Requests OK " + numRequestsFetched )
    process.stdout.write ( " :: Requests OK " + numRequestsFetchedKO )
    //process.stdout.write ( " :: Currents ( " + currentRequests + ")" )

    

    //process.nextTick ( log() )
    if ( totalNumRequests == (numRequestsFetched + numRequestsFetchedKO) ) {
        clearInterval ( interval )
        endProcess ()
    }
}

let interval = setInterval(log, 100);

function endProcess () {
    
    process.stdout.write ("\nTotal number of results is: " + results.length )
    let r = 0
    results.forEach ( result => {
        console.dir ( "Result " + ++r)
        console.dir ( result, {depth:10} )
    })
}
