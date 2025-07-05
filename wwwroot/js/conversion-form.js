document.addEventListener("DOMContentLoaded", () => {
    const useColorCheckbox = document.getElementById('useBackgroundColor');
    const backgroundInput = document.getElementById('background');

    if (!backgroundInput.value || backgroundInput.value === '#000000' || backgroundInput.value === '#ffffff') {
        const bodyBg = window.getComputedStyle(document.body).backgroundColor;
        backgroundInput.value = rgbToHex(bodyBg);
    }

    useColorCheckbox.addEventListener('change', () => {
        backgroundInput.disabled = !useColorCheckbox.checked;
        updateColorDisplay();
    });

    backgroundInput.disabled = !useColorCheckbox.checked;
    updateColorDisplay();

    backgroundInput.addEventListener('input', () => {
        updateColorDisplay();
    });

    document.querySelectorAll('.btn-click-effect').forEach(button => {
        button.addEventListener('click', e => {
            const rect = button.getBoundingClientRect();
            const x = e.clientX - rect.left;
            const y = e.clientY - rect.top;

            button.style.setProperty('--click-x', x + 'px');
            button.style.setProperty('--click-y', y + 'px');
            console.log("???");
        });
    });
});

function rgbToHex(rgb) {
    const result = rgb.match(/\d+/g);
    if (!result) return '#ffffff';
    return '#' + result.slice(0, 3).map(x => {
        const hex = parseInt(x).toString(16);
        return hex.length === 1 ? '0' + hex : hex;
    }).join('');
}

function updateColorDisplay() {
    const backgroundInput = document.getElementById('background');
    document.getElementById('colorValueDisplay').textContent = backgroundInput.disabled ? "None" : backgroundInput.value.toUpperCase();
}

async function copyToClipboard(button, tempText) {
    try {
        await navigator.clipboard.writeText(document.getElementById("result-container").innerHTML);

        const originalText = button.textContent;
        await new Promise(r => setTimeout(r, 100));

        button.textContent = tempText;
        await new Promise(r => setTimeout(r, 750));

        button.textContent = originalText;

    } catch (err) {
        alert("Failed to copy: " + err);
    }
}


function openResultInNewTab() {
    const content = document.getElementById("result-container").innerHTML;

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
                            padding: 1em;
                        }
                    </style>
                </head>
                <body>${content}</body>
            </html>
        `);

    newWindow.document.close();
}
