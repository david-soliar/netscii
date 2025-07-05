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
