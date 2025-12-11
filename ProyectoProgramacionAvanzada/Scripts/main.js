import { ToastManager } from './modules/toast.js';

window.App = {
    toast: ToastManager.getInstance(document.getElementById('toast-container')),
} 