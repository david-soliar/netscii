async function copyToClipboard() {
    try {
        const content = document.getElementById("result").innerHTML;
        await navigator.clipboard.writeText(content);
        alert("Copied to clipboard!");
    } catch (err) {
        alert("Failed to copy: " + err);
    }
}

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
