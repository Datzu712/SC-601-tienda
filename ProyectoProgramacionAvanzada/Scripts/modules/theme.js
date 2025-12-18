// todo: add jsoc (alta pereza)
export class ThemeManager {
    constructor() {
        this.toggleButton = null;
        this.icon = null;
    }

    init() {
        //this.loadTheme(); already loaded in the layout
        this.bindElements();
        this.bindEvents();
        this.updateIcon();
    }

    bindElements() {
        this.toggleButton = document.getElementById('theme-toggle');
        this.icon = this.toggleButton?.querySelector('i');
    }

    bindEvents() {
        if (!this.toggleButton) {
            console.debug('No theme toggle button found.');
            return;
        }

        this.toggleButton.addEventListener('click', () => this.handleToggle());
        
        window.matchMedia('(prefers-color-scheme: dark)')
            .addEventListener('change', (e) => this.handleSystemThemeChange(e));
    }

    handleToggle() {
        const newTheme = this.toggleTheme();
        this.updateIcon();
        this.dispatchThemeChange(newTheme);
    }

    handleSystemThemeChange(event) {
        if (!localStorage.getItem('theme')) {
            const theme = event.matches ? 'dark' : 'light';
            document.documentElement.setAttribute('data-bs-theme', theme);
            this.updateIcon();
        }
    }

    updateIcon() {
        if (!this.icon) return;
        const currentTheme = this.getCurrentTheme();
        this.icon.className = currentTheme === 'dark' ? 'fas fa-sun' : 'fas fa-moon';
    }

    dispatchThemeChange(theme) {
        window.dispatchEvent(new CustomEvent('themeChanged', { detail: { theme } }));
    }

    loadTheme() {
        const savedTheme = localStorage.getItem('theme');
        if (savedTheme) {
            document.documentElement.setAttribute('data-bs-theme', savedTheme);
        } else {
            const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
            document.documentElement.setAttribute('data-bs-theme', prefersDark ? 'dark' : 'light');
        }
    }

    getCurrentTheme() {
        return document.documentElement.getAttribute('data-bs-theme');
    }

    toggleTheme() {
        const currentTheme = this.getCurrentTheme();
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        document.documentElement.setAttribute('data-bs-theme', newTheme);
        localStorage.setItem('theme', newTheme);
        return newTheme;
    }

    destroy() {
        this.toggleButton?.removeEventListener('click', this.handleToggle);
    }
    
    static getInstance() {
        return new ThemeManager();
    }
}
