var unirest = require('unirest');
var url = 'http://epd2017iot.azurewebsites.net/api/faces/authenticate';
var path = require("path");

module.exports.evaluate = function(file, onSuccess) {
    unirest
        .post(url)
        .headers({'Content-Type': 'multipart/form-data'})
        .field('parameter', 'value') // Form field 
        .attach(file, path.join(__dirname, file)) // Attachment 
        .end(function (response) { 
            onSuccess(response.body); 
        });
};