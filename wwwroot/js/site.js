async function copyToClipboard() {
    try {
        const content = document.getElementById("result").innerHTML;
        await navigator.clipboard.writeText(content);
        alert("Copied to clipboard!");
    } catch (err) {
        alert("Failed to copy: " + err);
    }
}

function openResultInNewTab() {
    const resultHtml = document.getElementById('result').innerHTML;

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
                        background-color: ${document.getElementById('result').children[0].style.backgroundColor || '#fff'};
                        padding: 1em;
                    }
                </style>
            </head>
            <body>${resultHtml}</body>
        </html>
    `);

    newWindow.document.close();
}

document.addEventListener("DOMContentLoaded", () => {
    const useColorCheckbox = document.getElementById('useBackgroundColor');
    const backgroundInput = document.getElementById('background');
    const colorDisplay = document.getElementById('colorValueDisplay');

    function rgbToHex(rgb) {
        const result = rgb.match(/\d+/g);
        if (!result) return '#ffffff';
        return '#' + result.slice(0, 3).map(x => {
            const hex = parseInt(x).toString(16);
            return hex.length === 1 ? '0' + hex : hex;
        }).join('');
    }

    function updateColorDisplay() {
        colorDisplay.textContent = backgroundInput.disabled ? "None" : backgroundInput.value.toUpperCase();
    }

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
});

(function initializeThemeToggle() {
    const toggleBtn = document.getElementById('themeToggle');
    const htmlEl = document.documentElement;

    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
        setTheme(savedTheme);
    } else {
        const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
        setTheme(prefersDark ? 'dark' : 'light');
    }

    toggleBtn.addEventListener('click', () => {
        const currentTheme = htmlEl.getAttribute('data-bs-theme');
        const newTheme = currentTheme === 'light' ? 'dark' : 'light';
        setTheme(newTheme);
        localStorage.setItem('theme', newTheme);
    });

    function setTheme(theme) {
        htmlEl.setAttribute('data-bs-theme', theme);
        toggleBtn.textContent = theme === 'dark' ? '☀️ Light' : '🌙 Dark';
    }
})();
