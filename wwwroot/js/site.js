
async function copy() {
    const cpb = document.getElementById("result").innerHTML;
    navigator.clipboard.writeText(cpb).then(() => {
        alert("Copied to clipboard!");
    }).catch(err => {
        alert("Failed to copy: " + err);
    });
}

if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
    console.log('Dark mode');
} else {
    console.log('Light mode');
}
