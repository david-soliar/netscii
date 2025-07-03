
async function copy() {
    const cpb = document.getElementById("result").innerHTML;
    navigator.clipboard.writeText(cpb).then(() => {
        alert("Copied to clipboard!");
    }).catch(err => {
        alert("Failed to copy: " + err);
    });
}
