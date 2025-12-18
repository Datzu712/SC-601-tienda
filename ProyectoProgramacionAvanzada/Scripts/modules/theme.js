// todo: add jsoc (alta pereza)
export class ThemeManager {
    toggleButton = null
    currentIcon = null

    /**
     * Creates an instance of ThemeManager.
     * @param {string|HTMLElement} toggleButtonOrId The toggle button element or its ID
     */
    constructor(toggleButtonOrId) {
        if (toggleButtonOrId) {
            if (typeof toggleButtonOrId === 'string') {
                this.toggleButton = document.getElementById(toggleButtonOrId);
            } else if (toggleButtonOrId instanceof HTMLElement) {
                this.toggleButton = toggleButtonOrId;
            }
            this.currentIcon = this.toggleButton?.querySelector('i');
        }
    }

    init() {
        //this.loadTheme(); already loaded in the layout
        this.bindEvents();
        this.updateIcon();
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
        new Promise((resolve) => setTimeout(() => {
            App.notifications.showToast(`Switched to ${newTheme} theme`, { delay: 1000 });
            resolve();
        }, 100));
    }

    handleSystemThemeChange(event) {
        if (!localStorage.getItem('theme')) {
            const theme = event.matches ? 'dark' : 'light';
            document.documentElement.setAttribute('data-bs-theme', theme);
            this.updateIcon();
        }
    }

    updateIcon() {
        if (!this.currentIcon) return;
        const currentTheme = this.getCurrentTheme();
        this.currentIcon.className = currentTheme === 'dark' ? 'fas fa-sun' : 'fas fa-moon';
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
