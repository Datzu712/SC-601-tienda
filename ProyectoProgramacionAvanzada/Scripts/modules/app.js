export class App {
    constructor() {
        this.modules = new Map();
        this.initialized = false;
    }

    registerModule(name, module) {
        let m = null;
        if ('getInstance' in module && typeof module.getInstance === 'function') {
            m = module.getInstance();
        } else {
            m = module;
        }
        
        this.modules.set(name, m);
        return this;
    }

    async init() {
        if (this.initialized) return;
        
        const theme = this.modules.get('theme');
        if (theme) theme.loadTheme();
        
        await this.waitForDOM();
        
        for (const [name, module] of this.modules) {
            if (typeof module.init === 'function') {
                try {
                    await module.init();
                } catch (error) {
                    console.error(`Error while trying to initialize module "${name}":`, error);
                }
            }
        }

        this.initialized = true;
    }

    waitForDOM() {
        return new Promise((resolve) => {
            if (document.readyState === 'loading') {
                document.addEventListener('DOMContentLoaded', resolve);
            } else {
                resolve();
            }
        });
    }

    get(moduleName) {
        return this.modules.get(moduleName);
    }
    
    detroy() {
        for (const [name, module] of this.modules) {
            if (typeof module.destroy === 'function') {
                try {
                    module.destroy();
                } catch (error) {
                    console.error(`Error while trying to destroy module "${name}":`, error);
                }
            }
        }
        this.modules.clear();
        this.initialized = false;
    }
}
