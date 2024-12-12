export function copyToClipboard(text) {
    navigator.clipboard.writeText(text);
}

export function getLocalStorageValue(key) {
    return localStorage.getItem(key);
}

export function setLocalStorageValue(key, value) {
    localStorage.setItem(key, value);
}