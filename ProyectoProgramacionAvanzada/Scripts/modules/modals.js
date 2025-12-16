export class ModalManager {
    constructor(modalId) {
        this.modalId = modalId;
        this.ensureModalExists();
    }
    
    ensureModalExists() {
        if (!document.getElementById(this.modalId)) {
            throw new Error('Modal not found');
        }
    }

    /**
     * Shows a confirmation modal (Be aware of XSS)
     * @param {Object} options - Modal options
     * @param {string} options.title - Modal title (optional, default "Confirmacion")
     * @param {string} options.message - Modal message
     * @param {string} options.confirmText - Confirm button text (optional, default "Confirmar")
     * @param {string} options.cancelText - Cancel button text (optional, default "Cancelar")
     * @param {string} options.confirmClass - CSS class for confirm button (optional, default "btn-primary")
     * @param {string} options.icon - Font Awesome icon (optional, default "fa-exclamation-circle")
     * @param {Function} options.onConfirm - Callback to execute on confirmation
     * @param {Function} options.onCancel - Callback to execute on cancellation (optional)
     */
    showConfirmation(options) {
        const {
            title = 'Confirmation',
            message = 'Are you sure you want to continue?',
            confirmText = 'Confirm',
            cancelText = 'Cancel',
            confirmClass = 'btn-primary',
            icon = 'fa-exclamation-circle',
            onConfirm = () => {},
            onCancel = () => {}
        } = options;
        
        const modalTitle = document.getElementById(`${this.modalId}Title`);
        const modalBody = document.getElementById(`${this.modalId}Body`);
        const confirmBtn = document.getElementById(`${this.modalId}ConfirmBtn`);
        const cancelBtn = document.getElementById(`${this.modalId}CancelBtn`);
        const modalIcon = modalTitle.previousElementSibling;

        modalTitle.textContent = title;
        modalBody.innerHTML = message;
        confirmBtn.innerHTML = `<i class="fas fa-check me-1"></i> ${confirmText}`;
        cancelBtn.innerHTML = `<i class="fas fa-times me-1"></i> ${cancelText}`;
        
        confirmBtn.className = `btn ${confirmClass}`;

        // Update icon
        if (modalIcon && modalIcon.classList.contains('fas')) {
            modalIcon.className = `fas ${icon} me-2`;
        }
        
        const modalElement = document.getElementById(this.modalId);
        const modal = new bootstrap.Modal(modalElement);
        
        const newConfirmBtn = confirmBtn.cloneNode(true);
        confirmBtn.parentNode.replaceChild(newConfirmBtn, confirmBtn);

        const newCancelBtn = cancelBtn.cloneNode(true);
        cancelBtn.parentNode.replaceChild(newCancelBtn, cancelBtn);
        
        newConfirmBtn.addEventListener('click', () => {
            modal.hide();
            onConfirm();
        });
        
        newCancelBtn.addEventListener('click', () => {
            modal.hide();
            onCancel();
        });
        
        modal.show();

        return modal;
    }

    /**
     * Shows a delete confirmation modal (red variant)
     * @param {Object} options - Modal options
     * @param {string} options.title - Modal title (optional, default "Confirm Deletion")
     * @param {string} options.message - Modal message
     * @param {Function} options.onConfirm - Callback to execute on confirmation
     */
    showDeleteConfirmation(options) {
        return this.showConfirmation({
            title: 'Confirm Deletion',
            icon: 'fa-trash-alt',
            confirmText: 'Delete',
            confirmClass: 'btn-danger',
            ...options
        });
    }

    /**
     * Shows a warning confirmation modal (yellow variant)
     * @param {Object} options - Modal options
     * @param {string} options.title - Modal title (optional, default "Warning")
     * @param {string} options.message - Modal message
     * @param {Function} options.onConfirm - Callback to execute on confirmation
     */
    showWarningConfirmation(options) {
        return this.showConfirmation({
            title: 'Warning',
            icon: 'fa-exclamation-triangle',
            confirmClass: 'btn-warning',
            ...options
        });
    }

    /**
     * Shows a success confirmation modal (green variant)
     * @param {Object} options - Modal options
     * @param {string} options.title - Modal title (optional, default "Confirm")
     * @param {string} options.message - Modal message
     * @param {Function} options.onConfirm - Callback to execute on confirmation
     */
    showSuccessConfirmation(options) {
        return this.showConfirmation({
            title: 'Confirm',
            icon: 'fa-check-circle',
            confirmClass: 'btn-success',
            ...options
        });
    }

    /**
     * Gets a singleton instance of ModalManager
     * @param {string} modalDivId - Modal container ID (optional)
     * @returns {ModalManager}
     */
    static getInstance(modalDivId) {
        if (!ModalManager.instance) {
            ModalManager.instance = new ModalManager(modalDivId);
        }
        return ModalManager.instance;
    }
}

