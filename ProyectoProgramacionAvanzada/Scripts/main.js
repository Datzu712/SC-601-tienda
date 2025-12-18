import { MODULE_TOKENS } from "./modules/constants.js";
import { Sidebar } from "./modules/sidebar.js";
import { ThemeManager } from "./modules/theme.js";
import { ToastManager } from './modules/toast.js';
import { ModalManager } from './modules/modals.js';
import { App } from "./modules/app.js";

const app = new App()
    .registerModule(MODULE_TOKENS.THEME, new ThemeManager('themeToggle'))
    .registerModule(MODULE_TOKENS.NOTIFICATIONS, new ToastManager('toast-container'))
    .registerModule(MODULE_TOKENS.SIDEBAR, new Sidebar('sidebar', 'sidebarToggle'))
    .registerModule(MODULE_TOKENS.MODALS, new ModalManager('confirmModal'));

await app.init();

window.App = {
    get notifications() { return app.get(MODULE_TOKENS.NOTIFICATIONS); },
    get modal() { return app.get(MODULE_TOKENS.MODALS); },
    get theme() { return app.get(MODULE_TOKENS.THEME); },
};
