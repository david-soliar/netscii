document.addEventListener("DOMContentLoaded", () => {
    const resultElement = document.getElementById("result");

    async function copyToClipboard() {
        try {
            const content = resultElement.innerHTML;
            await navigator.clipboard.writeText(content);
            alert("Copied to clipboard!");
        } catch (err) {
            alert("Failed to copy: " + err);
        }
    }

    function openResultInNewTab() {
        const content = resultElement.innerHTML;

        const newWindow = window.open();

        if (!newWindow) {
            alert('Opening new window with result was blocked by your browser');
            return;
        }

        newWindow.document.write(`
            <html>
                <head>
                    <title>netscii - Result</title>
                    <style>
                        body {
                            background-color: ${resultElement.children[0].style.backgroundColor || '#fff'};
                            padding: 1em;
                        }
                    </style>
                </head>
                <body>${content}</body>
            </html>
        `);

        newWindow.document.close();
    }
});
