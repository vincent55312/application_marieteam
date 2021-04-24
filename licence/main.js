const express = require('express')
const app = express()
const fs = require('fs')
var dateFormat = require('dateformat');
var port = 8080

const { createLogger, format, transports } = require('winston');

const logger = createLogger({
  level: 'info',
  exitOnError: false,
  format: format.json(),
  transports: [
    new transports.File({ filename: `apilogs.log` }),
  ],
});

module.exports = logger;


function getTime(){
    var date = dateFormat(new Date(), "yyyy-mm-dd H:MM:ss");
    return "["+date+"]"
}

function LicenceExist(key){
    var exist = false
    let file = fs.readFileSync('licensing.json')
    let licences = JSON.parse(file)

    licences.forEach(element => {
        if(key === element.licence)
            exist = true
    });
    return exist
}

function log (key, exist){
    if(exist){
        console.log(getTime() + " Connected with licence : " + key)
        logger.log('info', getTime() + " Connected with licence : " + key)
    }
    else{
        console.log(getTime() + " Failed to connect to licence : "+ key)
        logger.log('info',getTime() + " Failed to connect to licence : "+ key)
    }
}

app.get('/api/:key', (req,res) => {
    var key = req.params.key
    var exist = LicenceExist(key)
    log(key, exist)
    res.send(exist)
})

app.listen(port, () => {
    console.log(getTime() + " Listening server on "+ port)
    logger.log('info', getTime() + " Listening server on "+ port);

})