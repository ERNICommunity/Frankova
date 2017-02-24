var unirest = require('unirest');
var url = 'http://epd2017iot.azurewebsites.net/api/faces/authenticate';

module.exports.evaluate = function(file, onSuccess) {
    unirest
        .post(url)
        .headers({'Content-Type': 'multipart/form-data'})
        .field('parameter', 'value') // Form field 
        .attach('face.jpg', file) // Attachment 
        .end(function (response) {
            var body = response.body;
            console.log(body); 
            onSuccess(body); 
        });
};