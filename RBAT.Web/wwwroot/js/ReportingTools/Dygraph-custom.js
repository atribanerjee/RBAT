// Util functions to prevent Dygraph-extra errors:

// Added a bug fix to the original file as per ths post:
https://stackoverflow.com/questions/43784534/dygraphs-export-image-createcanvas-is-not-a-function
Dygraph.update = function (self, o) {
    if (typeof (o) != 'undefined' && o !== null) {
        for (var k in o) {
            if (o.hasOwnProperty(k)) {
                self[k] = o[k];
            }
        }
    }
    return self;
};
Dygraph.createCanvas = function () {
    return document.createElement('canvas');
}