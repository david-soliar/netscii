function hideContainer(container) {
    const element = document.querySelector(`#${container}-container`);
    element.style.display = 'none';
    element.disabled = true;
}

function randomColor() {
    const hue = Math.floor(Math.random() * 360);
    return `hsl(${hue}, 70%, 60%)`;
}

function colorizeText(id) {
    const el = document.getElementById(id);
    if (!el) return;

    const text = el.textContent;
    el.textContent = '';

    for (const char of text) {
        const span = document.createElement('span');
        span.textContent = char;
        span.style.color = randomColor();
        el.appendChild(span);
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const toggleBtn = document.getElementById('themeToggle');

    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
        setTheme(savedTheme);
    } else {
        setTheme('dark');
    }

    toggleBtn.addEventListener('click', () => {
        const currentTheme = document.documentElement.getAttribute('data-bs-theme');
        const newTheme = currentTheme === 'light' ? 'dark' : 'light';
        setTheme(newTheme);
        localStorage.setItem('theme', newTheme);
    });

    function setTheme(theme) {
        document.documentElement.setAttribute('data-bs-theme', theme);
        toggleBtn.textContent = theme === 'dark' ? '☀️ Light' : '🌙 Dark';
    }

    colorizeText('colorful-text');
});
