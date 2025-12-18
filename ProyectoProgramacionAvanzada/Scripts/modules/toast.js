/**
 * CSS classes for different toast types.
 * @typedef {Object} ToastTypes
 * @property {string | undefined} success - Bootstrap class for success toasts
 * @property {string | undefined} error - Bootstrap class for error toasts
 * @property {string | undefined} warning - Bootstrap class for warning toasts
 * @property {string | undefined} info - Bootstrap class for info toasts
 */

/**
 * Default toast option values.
 * @typedef {Object} ToastConfig
 * @property {string | undefined} title - Default toast title
 * @property {number | undefined} delay - Default auto-hide delay in ms
 * @property {boolean | undefined} autohide - Should toast auto-hide?
 * @property {() => void} onHide - Callback function when toast hides
 * @property {'success'|'error'|'warning'|'info' | undefined} type - Default toast type
 */

/**
 * Default configuration options for toast notifications.
 * @typedef {Object} ToastManagerConfig
 * @property {ToastTypes | undefined} toastTypes - CSS classes for different toast types
 * @property {ToastConfig | undefined} toastConfig - Default values for toast options
 */

/**
 * @type {ToastManagerConfig} - Default configuration for ToastManager
 */
export const TOAST_CONFIG = Object.freeze({
    toastTypes: Object.freeze({
        success: 'text-bg-success',
        error: 'text-bg-danger',
        warning: 'text-bg-warning',
        info: 'text-bg-info'
    }),
    toastConfig: Object.freeze({
        title: 'Notification',
        delay: 5000,
        autohide: true,
        type: 'info'
    }),
});

export class ToastManager {
    /**
     * @type {ToastManager}
     */
    static #instance;
    #toastIdsIncremental = 1;

    /**
     * @type {HTMLDivElement} - Div container of all toasts
     */
    #rootToastContainer = null;
    
    /**
     * @type {ToastManagerConfig} - Default configuration options
     */
    #defaultOptions;
    
    /**
     * Private constructor - Use ToastManager.getInstance() instead
     * @param {HTMLElement | undefined} rootToastContainer - The toast HTML element
     * @param {ToastManagerConfig | undefined} defaultOptions - Default configuration options
     * @returns {ToastManager}
     */
    constructor(rootToastContainer, defaultOptions = TOAST_CONFIG) {
        if (ToastManager.#instance) throw new Error('Toast already instantiated. Use ToastManager.getInstance() to access the singleton instance.');
        
        if (!rootToastContainer || !(rootToastContainer instanceof HTMLDivElement)) {
            throw new Error('Root toast container element is required to initialize Toast.');
        }
        
        this.#defaultOptions = defaultOptions;
        this.#rootToastContainer = rootToastContainer;
        
        ToastManager.#instance = this;
    }

    /**
     * Show a bootstrap toast in the user interface
     * @param {string} message - The message to display in the toast body
     * @param {ToastConfig | undefined} options - Toast configuration options
     * @returns {void}
     * @example
     * // Show a simple toast
     * showToast('Operation successful');
     *
     * @example
     * // Show a toast with custom options
     * showToast('Product saved successfully', {
     *   title: 'success',
     *   delay: 3000,
     *   type: 'success'
     * });
     */
    showToast(message, options) {
        this.#validateElement();
        
        const {
            title = this.#defaultOptions.toastConfig.title,
            delay = this.#defaultOptions.toastConfig.delay,
            autohide = this.#defaultOptions.toastConfig.autohide,
            type
        } = options ?? {};
        
        const toastElement = this.#createToastElement(message, { title, type });
        this.#rootToastContainer.appendChild(toastElement);
        
        const toastInstance = bootstrap.Toast.getOrCreateInstance(toastElement, { delay, autohide });
        
        toastInstance.show();
        toastElement.addEventListener('hidden.bs.toast', () => {
            toastElement.remove();
        });
    }

    /**
     * Hide all currently displayed toasts inside the root toast container
     * @returns {void}
     */
    hideToast() {
        this.#validateElement();
        
        const toasts = this.#rootToastContainer.querySelectorAll('.toast.show');
        toasts.forEach(toastElement => {
            const toastInstance = bootstrap.Toast.getInstance(toastElement);
            if (toastInstance) {
                toastInstance.hide();
            }
        });

    }

    #validateElement() {
        if (!this.#rootToastContainer || !document.body.contains(this.#rootToastContainer)) {
            throw new Error('Toast element not found in DOM');
        }
    }

    /**
     * Get the singleton instance of the ToastManager class or create it if it doesn't exist
     * @param {HTMLElement | undefined} toastElement - The toast container HTML element
     * @returns {ToastManager}
     */
    static getInstance(toastElement) {
        if (!ToastManager.#instance) {
            ToastManager.#instance = new ToastManager(toastElement);
        }
        
        return ToastManager.#instance; 
    }

    /**
     * Create a toast HTML element dynamically
     * @param {string} message - The message to display in the toast body
     * @param {ToastConfig | undefined} options - Toast configuration options
     * @private
     */
    #createToastElement(message, options) {
        const toastId = `app-toast-${++this.#toastIdsIncremental}`;
        const toastDiv = document.createElement('div');
        toastDiv.id = toastId;
        toastDiv.className = 'toast mt-2';
        toastDiv.setAttribute('role', 'alert');
        toastDiv.setAttribute('aria-live', 'polite');
        toastDiv.setAttribute('aria-atomic', 'true');

        if (options.onHide && typeof options.onHide === 'function') {
            toastDiv.addEventListener('hidden.bs.toast', options.onHide);
        }

        if (options.type && this.#defaultOptions.toastTypes[options.type]) {
            toastDiv.classList.add(this.#defaultOptions.toastTypes[options.type]);
        }
        
        toastDiv.innerHTML = `
            <div class="toast-header">
                <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body">
                
            </div>
        `;
        
        if (options.title) {
            const strong = document.createElement('strong');
            strong.className = 'me-auto';
            strong.textContent = options.title;
            const toastHeader = toastDiv.querySelector('.toast-header');
       
            toastDiv.querySelector('.toast-header').insertBefore(strong, toastHeader.firstChild);
        }
        
        toastDiv.querySelector('.toast-body').textContent = message;
        
        return toastDiv;
    }
}
