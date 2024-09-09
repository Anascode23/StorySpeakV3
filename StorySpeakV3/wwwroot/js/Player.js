let songs = [
    {
        song_name: "One More Time",
        artist: "Daft Punk",
        url: "./songs/onemoretime.mp3"
    }
];

let audioPlayer = document.getElementById('audio-player');
let songNameElement = document.getElementById('song-name');
let artistElement = document.getElementById('artist');

// Function to load song
window.onload = function () {
    loadSong(songs[0])
}
function loadSong(song) {
    songNameElement.textContent = song.song_name;
    artistElement.textContent = song.artist;
    audioPlayer.src = song.url;
}

// Play button triggers TTS
//document.getElementById('play-btn').addEventListener('click', function () {
//    responsiveVoice.speak(extractedText, "UK English Male");
//});

// Pause button stops TTS
//document.getElementById('pause-btn').addEventListener('click', function () {
//    responsiveVoice.cancel();
//});
