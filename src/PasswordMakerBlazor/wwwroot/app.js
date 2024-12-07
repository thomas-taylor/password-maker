function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(
        function () {
            console.log("Text copied to clipboard");
        },
        function (err) {
            console.error("Could not copy text: ", err);
        }
    );
}

function setLocalStorageValue(key, value) {
    localStorage.setItem(key, value);
}

function getLocalStorageValue(key) {
    return localStorage.getItem(key);
}