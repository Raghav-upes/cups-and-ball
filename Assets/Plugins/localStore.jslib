mergeInto(LibraryManager.library, {

saveUsername: function(key, value) {
    localStorage.setItem(UTF8ToString(key),UTF8ToString(value));
    console.log("Saved to localStorage: " + UTF8ToString(key) + " - " + UTF8ToString(key));
},

loadUsername: function() {



    var returnStr = localStorage.getItem("userName");
    if(returnStr===null || returnStr==="")
    {
    returnStr="0";
    }
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
        console.log(buffer);
    return buffer;


},


deleteUsername: function() {
    console.log("Delete to localStorage: " + localStorage.getItem("userName"));
    localStorage.removeItem("userName");

},

});