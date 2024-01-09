function toggleDarkMode() {
    const body = document.body;
    body.classList.toggle('dark-mode');

    // Save user preference
    const preferredMode = body.classList.contains('dark-mode') ? 'dark' : 'light';
    localStorage.setItem('preferredMode', preferredMode);
}