
export class Sidebar {
    /**
     * @type {HTMLElement|null}
     */
    #sidebarElement = null;
    #sidebarButton = null;


    /**
     * @param { HTMLDivElement | string | undefined } sidebarElementOrId - Sidebar element or its ID
     * @param { HTMLButtonElement | string | undefined } sidebarButtonOrId - Sidebar toggle button or its ID
     * @param sidebarElementOrId
     */
    constructor(sidebarElementOrId, sidebarButtonOrId) {
        if (typeof sidebarElementOrId === 'string') {
            this.#sidebarElement = document.getElementById(sidebarElementOrId);
        } else if (sidebarElementOrId instanceof HTMLDivElement) {
            this.#sidebarElement = sidebarElementOrId;
        } else {
            this.#sidebarElement = document.getElementById("sidebar");
        }
        
        if (typeof sidebarButtonOrId === 'string') {
            this.#sidebarButton = document.getElementById(sidebarButtonOrId);
        } else if (sidebarButtonOrId instanceof HTMLButtonElement) {
            this.#sidebarButton = sidebarButtonOrId;
        } else {
            this.#sidebarButton = document.getElementById("sidebarToggle");
        }
    }

    init() {
        if (!(this.#sidebarElement instanceof HTMLElement)) {
            throw new Error("Sidebar element doesn't exist or is invalid.");
        }
        
        if (!(this.#sidebarButton instanceof HTMLElement)) {
            throw new Error("Sidebar toggle button doesn't exist or is invalid.");
        }
        
        this.#bindEvents();
    }
    
    #bindEvents() {
        this.#sidebarButton.addEventListener('click', this.toggleSidebar.bind(this));
    }
    
    toggleSidebar() {
        this.#sidebarElement.classList.toggle('show');
    }
    
    destroy() {
        this.#sidebarButton.removeEventListener('click', this.toggleSidebar);
    }

    static getInstance () {
        return new Sidebar();
    }
}