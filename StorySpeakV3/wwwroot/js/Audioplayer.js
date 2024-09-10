// AJAX request to upload the PDF and extract the text
$.ajax({
    url: '/Readpdf',  // Make sure this matches your route in C# controller
    type: 'POST',
    data: formData,
    contentType: false,  // Needed for file upload
    processData: false,  // Prevents jQuery from processing the data
    success: function (response) {
        // Now send the extracted text for TTS
        convertTextToSpeech(response);
    },
    error: function (xhr, status, error) {
        console.error('Error extracting text:', error);
    }
});

// Function to trigger TTS using responsiveVoice.js
function convertTextToSpeech(text) {
    $.ajax({
        url: '@Url.Action("TextToSpeech", "ReadPdf")',  // Adjust to your correct route
        method: 'POST',
        data: { text: text },
        success: function (response) {
            // Trigger the TTS system to speak the cleaned text
            responsiveVoice.speak(response.cleanedText, "US English Female");
        }


    })
}


