// AJAX request to upload the PDF and extract the text
$.ajax({
    url: '/Readpdf',  // Make sure this matches your route in C# controller
    type: 'POST',
    data: { text: extractedText },
    contentType: false,  // Needed for file upload
    processData: false,
    success: function (response) {

        convertTextToSpeech(response);
    },
    error: function (xhr, status, error) {
        console.error('Error extracting text:', error);
    }
});


function convertTextToSpeech(text) {
    $.ajax({
        url: '@Url.Action("TextToSpeech", "ReadPdf")',
        method: 'POST',
        data: { text: text },
        success: function (response) {

            responsiveVoice.speak(response.cleanedText, "US English Female");
        }


    })
}


