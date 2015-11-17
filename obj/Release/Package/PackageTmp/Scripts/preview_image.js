// common variables
var iBytesUploaded = 0;
var iBytesTotal = 0;
var iPreviousBytesLoaded = 0;
var iMaxFilesize = 1048576; // 1MB
var oTimer = 0;
var sResultFileSize = '';

function bytesToSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB'];
    if (bytes == 0) return 'n/a';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return (bytes / Math.pow(1024, i)).toFixed(1) + ' ' + sizes[i];
};


function fileSelected() {
    //Setup error message
    document.getElementById('errorImage').style.display = 'none';

    // get selected file element
    var oFile = document.getElementById('image_file').files[0];

    // filter for image files
    var rFilter = /^(image\/jpg|image\/gif|image\/jpeg|image\/png|image\/tiff)$/i;
    if (!rFilter.test(oFile.type)) {
        document.getElementById('errorImage').innerHTML = "写真のタイプが違います。";
        document.getElementById('errorImage').style.display = 'block';
        return;
    }

    // little test for filesize
    if (oFile.size > iMaxFilesize) {
        document.getElementById('errorImage').innerHTML = "写真のサイズが大きすぎます。";
        document.getElementById('errorImage').style.display = 'block';
        return;
    }

    // get preview element
    var oImage = document.getElementById('img_preview');

    // prepare HTML5 FileReader
    var oReader = new FileReader();
    oReader.onload = function (e) {

        // e.target.result contains the DataURL which we will use as a source of the image
        oImage.src = e.target.result;
        oImage.onload = function () { // onload event handler
            $("#dataURL").val(e.target.result);
            // display step 2
            //$('.step2').fadeIn(500);


            // destroy Jcrop if it is existed
            /*if (typeof jcrop_api != 'undefined') {
                jcrop_api.destroy();
                jcrop_api = null;
                $('#img_preview').width(oImage.naturalWidth);
                $('#img_preview').height(oImage.naturalHeight);
            }
 
                // initialize Jcrop
                $('#img_preview').Jcrop({
                    minSize: [100, 75], // min crop size
                    aspectRatio : 4/3, // keep aspect ratio 1:1
                    bgFade: true, // use fade effect
                    bgOpacity: .3, // fade opacity
                    setSelect: [ 175, 100, 400, 300 ]
                }, function(){
                    // use the Jcrop API to get the real image size
                    var bounds = this.getBounds();
                    boundx = bounds[0];
                    boundy = bounds[1];
 
                    // Store the Jcrop API in the jcrop_api variable
                    jcrop_api = this;
                });*/

        };
    };

    // read selected file as DataURL
    oReader.readAsDataURL(oFile);
}